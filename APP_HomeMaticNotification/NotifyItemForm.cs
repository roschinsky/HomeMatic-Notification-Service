using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRoschinsky.Service.HomeMaticNotification
{
    public partial class NotifyItemForm : Form
    {
        private HMNotifyItem currentNotifyItem;

        public NotifyItemForm(HMNotifyItem notifyItem)
        {
            InitializeComponent();

            currentNotifyItem = notifyItem;
        }

        private void TestSilenceTimesForm_Load(object sender, EventArgs e)
        {
            Text = String.Format("Details for {0}:{1}", currentNotifyItem.DeviceAddress, currentNotifyItem.DeviceChannel);
            propertyGrid1.SelectedObject = currentNotifyItem;
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            richTextBoxSimResult.Clear();

            int stepCount = 0;
            DateTime currentDateTime = dateTimePickerFrom.Value;
            while (dateTimePickerTo.Value > currentDateTime && stepCount < numericUpDownSteps.Value)
            {
                stepCount++;
                richTextBoxSimResult.AppendText(String.Format("\n#{0:D3} - {1}...\n", stepCount, currentDateTime));
                foreach (HMNotifySilence silence in currentNotifyItem.SilenceTimes)
                {
                    richTextBoxSimResult.AppendText(String.Format("\t{0}: {1}\n", silence, (silence.IsSilenceNow(currentDateTime) ? "zZZ (silence)" : "Send (message)")));
                    richTextBoxSimResult.Select(richTextBoxSimResult.Text.LastIndexOf("\t"), richTextBoxSimResult.Text.Length);
                    if (silence.IsSilenceNow(currentDateTime))
                    {
                        richTextBoxSimResult.SelectionBackColor = Color.MistyRose;
                        richTextBoxSimResult.SelectionColor = Color.DarkRed;
                    }
                    else
                    {
                        richTextBoxSimResult.SelectionColor = Color.DarkGreen;
                    }
                    

                }
                currentDateTime = currentDateTime.AddMinutes((int)numericUpDownIntervall.Value);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab == tabPage2)
            {
                dateTimePickerFrom.Value = DateTime.Now;
                dateTimePickerTo.Value = DateTime.Now.AddDays(1);

                if (currentNotifyItem != null)
                {
                    if (currentNotifyItem.SilenceTimes != null && currentNotifyItem.SilenceTimes.Length > 0)
                    {
                        listViewDefST.Items.Clear();
                        foreach (HMNotifySilence silence in currentNotifyItem.SilenceTimes)
                        {
                            listViewDefST.Items.Add(silence.ToString());
                        }

                        return;
                    }
                    else
                    {
                        MessageBox.Show("There are no silence times defined for this item.", "No silence times...");
                    }
                }

                buttonSimulate.Enabled = false;
                tabControl1.SelectedTab = tabPage1;
            }
        }
    }
}
