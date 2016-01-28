using System;
using System.Windows.Forms;
using TRoschinsky.Service.HomeMaticNotification;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// It's the tester for the service that comes with a Windows Forms application. It is quite simple 
    /// and behaves exactly like running the service including processing of the HomeMaticNotification 
    /// style configuration file by using the service implementation (HMNotifier)
    /// </summary>
    public partial class MainForm : Form
    {
        private HMNotifier notifier;
        private Timer timer;
        private DateTime lastQueryTime;

        public MainForm()
        {
            InitializeComponent();

            // Initialize timer with 2 seconds
            timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += timer_Tick;
        }

        /// <summary>
        /// Main logic for updating frontend when timer is running
        /// </summary>
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (notifier != null)
                {
                    // Run event collection of notifier
                    notifier.CollectEvents();

                    // Check for events recorded since last execution and adds them to a textbox
                    if (notifier.Notifications.Length > 0)
                    {
                        foreach (HMNotification notification in notifier.Notifications)
                        {
                            if (notification.TimeStamp >= lastQueryTime)
                            {
                                richTextBoxEvents.Text += String.Format("{0} - {2} @ {1}: {3} was {5}pushed ({4})",
                                    notification.TimeStamp,
                                    notification.Scope,
                                    notification.Name,
                                    notification.DataPoint.Value,
                                    notification.DeviceAddress,
                                    (notification.NotificationSent ? String.Empty : "not ")) + Environment.NewLine;
                            }
                        }
                    }

                    // Remember time of last execution
                    lastQueryTime = DateTime.Now;

                    // Check for errors recorded within last execution interval and adds them to a textbox
                    foreach (Exception error in notifier.GetRecentErrors())
                    {
                        string message = error.Message;
                        if (error.InnerException != null)
                        {
                            message += " [Inner: " + error.InnerException.Message + "];";
                        }
                        if (error.StackTrace != null)
                        {
                            message += " [Stack: " + error.StackTrace + "];";
                        }

                        richTextBoxLog.Text += message + Environment.NewLine;
                    }

                    // Writes current configuration of notifier including results of last notify-execution to a list view
                    listViewDevices.Items.Clear();
                    foreach (HMNotifyItem notifyItem in notifier.NotifyItems)
                    {
                        ListViewItem item = null;

                        if (notifyItem.LastNotification != null && notifyItem.LastNotification.DataPoint != null)
                        {
                            item = new ListViewItem(notifyItem.DeviceAddress);
                            item.SubItems.Add(notifyItem.Scope);
                            item.SubItems.Add(notifyItem.Name);
                            item.SubItems.Add(notifyItem.IsImportant ? "VIP" : "Normal");
                            item.SubItems.Add(String.Format("{0} ({1})", notifyItem.IsSilenceTime ? "zZZ" : "Send", notifyItem.SilenceTimes.Length));
                            item.SubItems.Add(String.Concat(notifyItem.Conditions.Length));
                            item.SubItems.Add(notifyItem.PreventNotificationStatus);
                            item.SubItems.Add(notifyItem.LastNotification.TimeStamp.ToString());
                            item.SubItems.Add(notifyItem.LastNotification.DataPoint.ValueString);
                            item.SubItems.Add(notifyItem.LastNotification.NotificationSent.ToString());
                            item.SubItems.Add(String.Concat(notifyItem.SendNotificationTo));
                        }
                        else
                        {
                            item = new ListViewItem(notifyItem.DeviceAddress);
                            item.SubItems.Add(notifyItem.Scope);
                            item.SubItems.Add(notifyItem.Name);
                        }
                        listViewDevices.Items.Add(item);
                    }

                    // Writes status to tool strip
                    toolStripStatusLabel1.Text = String.Format("Notifier is {0}connected; Events fired: {1}", (notifier.IsConnected ? String.Empty : "not "), notifier.WorkerRunCount);
                }

            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = String.Format("Notifier is {0}connected; Events fired: {1} - last with error!", (notifier.IsConnected ? String.Empty : "not "), notifier.WorkerRunCount);
                richTextBoxLog.AppendText(String.Format("{0}: {1}{2}" , DateTime.Now, ex.Message, Environment.NewLine));
            }
        }

        #region UI handlers and stuff

        /// <summary>
        /// Eventhandler for "Connect"- and "Stop"-button
        /// Initializes the notifier and controls the timer execution
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnConnect.Text == Properties.Resources.btnConnectCaptionStart)
                {
                    // "Start" - initialize a new notifier with given URL and starts timer execution
                    lastQueryTime = DateTime.Now;
                    

                    HMNotifierConfig config = new HMNotifierConfig()
                    {
                        HmcUrl = txtUrl.Text,
                        NotifierQueryFullRequestSec = "10",
                        NotifierConfigFile = Properties.Settings.Default.NotifierConfigFile,
                        NotificationSmtpHost = Properties.Settings.Default.NotificationSmtpHost,
                        NotificationSmtpPort = Properties.Settings.Default.NotificationSmtpPort,
                        NotificationSmtpMailFrom = Properties.Settings.Default.NotificationSmtpMailFrom
                    };

                    notifier = new HMNotifier(config);
                    //notifier = new HMNotifier(txtUrl.Text, "10", Properties.Settings.Default.NotifierConfigFile);

                    if (notifier != null && notifier.IsConnected)
                    {
                        timer.Start();
                        txtUrl.Enabled = false;
                        richTextBoxEvents.Enabled = richTextBoxLog.Enabled = listViewDevices.Enabled = true;
                        btnConnect.Text = Properties.Resources.btnConnectCaptionStop;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = String.Format("Connection to '{0}' failed...", txtUrl.Text);
                    }
                }
                else
                {
                    // "Stop" - halts timer execution
                    if (timer.Enabled)
                    {
                        timer.Stop();
                    }
                    txtUrl.Enabled = true;
                    richTextBoxEvents.Enabled = richTextBoxLog.Enabled = listViewDevices.Enabled = false;
                    btnConnect.Text = Properties.Resources.btnConnectCaptionStart;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unexpected error while connecting...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Eventhandler for changing the checkbox "Simulate send"
        /// </summary>
        private void checkBoxSimulateSend_CheckedChanged(object sender, EventArgs e)
        {
            if (notifier != null)
            {
                notifier.JustSimulateSending = checkBoxSimulateSend.Checked;
            }
        }

        /// <summary>
        /// Eventhandler for closing the application; does some clean up
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            if(notifier != null)
            {
                notifier.Dispose();
            }
        }

        #endregion
    }
}
