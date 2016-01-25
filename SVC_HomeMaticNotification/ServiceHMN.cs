using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using TRoschinsky.Service.HomeMaticNotification;

namespace SVC_HomeMaticNotification
{
    /// <summary>
    /// It's the service contoller class. Here the configuration data from settings is 
    /// obtained, the logging is set up and the main logic inside the HMNotifier is triggered 
    /// by a timer job.
    /// This class also handles startup and shutdown of the service component.
    /// </summary>
    public partial class ServiceHMN : ServiceBase
    {
        private string eventLogSource = "HMNotificationService";
        private string eventLogName = "Application";

        private HMNotifier notifier;
        private System.Timers.Timer queryTimer;
        private System.Timers.Timer reconnectTimer;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceHMN()
        {
            try
            {
                // Setup logging
                AutoLog = false;
                
                if(!String.IsNullOrWhiteSpace(Properties.Settings.Default.ServiceEventSource))
                {
                    eventLogSource = Properties.Settings.Default.ServiceEventSource;
                }

                if (!EventLog.SourceExists(eventLogSource))
                {
                    EventLog.CreateEventSource(eventLogSource, eventLogName);
                    ExitCode = 100;
                    Stop();
                }

                // Initialize service
                InitializeComponent();

                queryTimer = new System.Timers.Timer();
                double interval = Double.Parse(Properties.Settings.Default.NotifierQueryTimerSec);
                if (interval < 2 || interval > 120)
                {
                    queryTimer.Interval = 2000;
                }
                else
                {
                    queryTimer.Interval = interval * 1000;
                }
                queryTimer.Elapsed += queryTimer_Elapsed;

                reconnectTimer = new System.Timers.Timer();
                reconnectTimer.Interval = Double.Parse(Properties.Settings.Default.NotifierReconnectTimerSec) * 1000;
                reconnectTimer.Elapsed += reconnectTimer_Elapsed;
            }
            catch(Exception)
            {
                ExitCode = 101;
                Stop();
            }
        }

        /// <summary>
        /// Service is starting...
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                RunNotificationPolling();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Starting service failed: " + ex.Message, EventLogEntryType.Error);
                ExitCode = 110;
                Stop();
            }
        }

        /// <summary>
        /// Service triggers the main logic by running it by a timer job...
        /// </summary>
        private void RunNotificationPolling()
        {
            try
            {
                Exception[] errors = null;

                HMNotifierConfig config = new HMNotifierConfig() {
                    HmcUrl = String.Format("{0}", Properties.Settings.Default.HmcUrl.TrimEnd('/')),
                    NotifierQueryFullRequestSec = Properties.Settings.Default.NotifierQueryFullRequestSec,
                    NotifierConfigFile = Properties.Settings.Default.NotifierConfigFile, 
                    NotificationSmtpHost = Properties.Settings.Default.NotificationSmtpHost,
                    NotificationSmtpPort = Properties.Settings.Default.NotificationSmtpPort,
                    NotificationSmtpCredUser = Properties.Settings.Default.NotificationSmtpCredUser,
                    NotificationSmtpCredPw = Properties.Settings.Default.NotificationSmtpCredPw,
                    NotificationSmtpMailFrom = Properties.Settings.Default.NotificationSmtpMailFrom
                };

                notifier = new HMNotifier(config);
                //notifier = new HMNotifier(String.Format("{0}", Properties.Settings.Default.HmcUrl.TrimEnd('/')), 
                //    Properties.Settings.Default.NotifierQueryFullRequestSec, 
                //    Properties.Settings.Default.NotifierConfigFile);
                Thread.Sleep(5000);

                if (notifier != null && notifier.IsConnected)
                {
                    queryTimer.Start();
                    EventLog.WriteEntry(eventLogSource, "Notification polling was started.", EventLogEntryType.Information);
                }
                else
                {
                    EventLog.WriteEntry(eventLogSource, "Notification polling was not started... running reconnect timer now.", EventLogEntryType.Error);
                    reconnectTimer.Start();
                }

                if (notifier != null)
                {
                    errors = notifier.GetRecentErrors();
                    if (errors != null && errors.Length > 0)
                    {
                        EventLog.WriteEntry(eventLogSource, "Some errors occured within notifier instance...", EventLogEntryType.Warning);
                        foreach (Exception error in errors)
                        {
                            EventLog.WriteEntry(eventLogSource, error.Message, EventLogEntryType.Error);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Setting up polling failed: " + ex.Message, EventLogEntryType.Error);
                ExitCode = 120;
                Stop();
            }
        }

        /// <summary>
        /// Service executes an refresh of the connection and configuration...
        /// </summary>
        private void reconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                reconnectTimer.Stop();
                RunNotificationPolling();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Reconnect timer fired but failed: " + ex.Message, EventLogEntryType.Error);
                ExitCode = 130;
                Stop();
            }
        }

        /// <summary>
        /// Service executes the main logic by running it by the elapsed-event of a timer job...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (notifier != null)
                {
                    notifier.CollectEvents();

                    Exception[] errors = notifier.GetRecentErrors();
                    if (errors != null && errors.Length > 0)
                    {
                        EventLog.WriteEntry(eventLogSource, "Some errors occured while query timer fired...", EventLogEntryType.Warning);
                        foreach (Exception error in errors)
                        {
                            try
                            {
                                string messageDetails = String.Format("*** Exception type: {0}\n*** Message: {1}\n\n", error.GetType().Name, error.Message);
                                if (error.InnerException != null)
                                {
                                    messageDetails += String.Format("*** Inner exception type: {0}\n*** Message: {1}\n\n", error.GetType().Name, error.Message);
                                    if (!String.IsNullOrWhiteSpace(error.InnerException.StackTrace))
                                    {
                                        messageDetails += String.Format("- Inner stack trace: {0}", error.InnerException.StackTrace);
                                    }
                                }
                                EventLog.WriteEntry(eventLogSource, messageDetails, EventLogEntryType.Error);
                            }
                            catch (Exception ex)
                            {
                                EventLog.WriteEntry(eventLogSource, "Failed to write error to log: " + ex.Message, EventLogEntryType.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Query timer fired but failed: " + ex.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Service stops...
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                if (queryTimer != null)
                {
                    queryTimer.Stop();
                    queryTimer.Dispose();
                }

                if (reconnectTimer != null)
                {
                    reconnectTimer.Stop();
                    reconnectTimer.Dispose();
                }

                EventLog.WriteEntry(eventLogSource, "Notifying ended - service stopped...", EventLogEntryType.Information);
                EventLog.WriteEntry(eventLogSource,
                    String.Format("Notifier was running for {0} time(s), was triggered for {1} time(s) and has {2} logged event(s).", notifier.WorkerRunCount, notifier.WorkerTriggeredCount, notifier.Notifications.LongLength),
                    EventLogEntryType.Information);

                if (notifier != null)
                {
                    notifier.Dispose();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Stopping service failed: " + ex.Message, EventLogEntryType.Error);
                ExitCode = 140;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Service is paused...
        /// </summary>
        protected override void OnPause()
        {
            try
            {
                if (notifier != null && notifier.IsConnected)
                {
                    queryTimer.Stop();
                    EventLog.WriteEntry(eventLogSource, "Notifying paused...", EventLogEntryType.Information);
                    EventLog.WriteEntry(eventLogSource,
                        String.Format("Notifier was running for {0} time(s), was triggered for {1} time(s) and has {2} logged event(s).", notifier.WorkerRunCount, notifier.WorkerTriggeredCount, notifier.Notifications.LongLength),
                        EventLogEntryType.Information);
                }
                else
                {
                    reconnectTimer.Stop();
                    EventLog.WriteEntry(eventLogSource, "Notifying paused...", EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Pausing service failed: " + ex.Message, EventLogEntryType.Error);
                ExitCode = 150;
                Stop();
            }
        }

        /// <summary>
        /// Service is comming up again...
        /// </summary>
        protected override void OnContinue()
        {
            try
            {
                if (notifier != null && notifier.IsConnected)
                {
                    EventLog.WriteEntry(eventLogSource, "Resuming notifying...", EventLogEntryType.Information);
                    queryTimer.Start();
                }
                else
                {
                    EventLog.WriteEntry(eventLogSource, "Resuming reconnecting...", EventLogEntryType.Information);
                    RunNotificationPolling();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogSource, "Resuming service failed: " + ex.Message, EventLogEntryType.Error);
                ExitCode = 160;
                Stop();
            }
        }
    }
}
