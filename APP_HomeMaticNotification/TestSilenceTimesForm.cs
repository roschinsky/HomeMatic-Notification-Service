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
    public partial class TestSilenceTimesForm : Form
    {
        private HMNotifyItem currentNotifyItem;

        public TestSilenceTimesForm(HMNotifyItem notifyItem)
        {
            InitializeComponent();

            currentNotifyItem = notifyItem;
        }

        private void TestSilenceTimesForm_Load(object sender, EventArgs e)
        {
            dateTimePickerFrom.Value = DateTime.Now;
            dateTimePickerTo.Value = DateTime.Now.AddDays(1);

            if(currentNotifyItem != null)
            {
                Text = String.Format("Test silences for {0}:{1}", currentNotifyItem.DeviceAddress, currentNotifyItem.DeviceChannel);

                if(currentNotifyItem.SilenceTimes != null && currentNotifyItem.SilenceTimes.Length > 0)
                {
                    listViewDefST.Items.Clear();
                    foreach(HMNotifySilence silence in currentNotifyItem.SilenceTimes)
                    {
                        listViewDefST.Items.Add(silence.ToString());                        
                    }

                    return;
                }
                else
                {
                    richTextBoxSimResult.AppendText("There are no silence times defined for this item.");
                    richTextBoxSimResult.Enabled = false;
                }
            }

            buttonSimulate.Enabled = false;
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            richTextBoxSimResult.Clear();

            int stepCount = 0;
            DateTime currentDateTime = dateTimePickerFrom.Value;
            while(dateTimePickerTo.Value > currentDateTime && stepCount < numericUpDownSteps.Value)
            {
                stepCount++;
                richTextBoxSimResult.AppendText(String.Format("\n#{0:D3} - {1}...\n", stepCount, currentDateTime));
                foreach(HMNotifySilence silence in currentNotifyItem.SilenceTimes)
                {
                    richTextBoxSimResult.AppendText(String.Format("\t{0}: {1}\n", silence, (silence.IsSilenceNow(currentDateTime) ? "zZZ" : "Send")));
                }
                currentDateTime = currentDateTime.AddMinutes((int)numericUpDownIntervall.Value);
            }
        }
    }
}
