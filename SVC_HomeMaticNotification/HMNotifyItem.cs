using System;
using System.Collections.Generic;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The HMNotifyItem is the primary data object used by the HMNotifier class. It contains 
    /// all configurational data like scope, name, type as well as the persistence of the last 
    /// occurred notification.
    /// </summary>
    public class HMNotifyItem
    {
        public enum ItemType
        {
            Device,
            Variable,
            Undefined
        }

        public string Scope { get; set; }
        public string Name { get; set; }
        public string DeviceAddress { get; set; }
        public string DeviceChannel { get; set; }
        public int VariableId { get; set; }
        public string ValueKey { get; set; }
        public bool IsImportant { get; set; }
        public bool IsSilent { get; set; }
        public bool SendNotification { get { return GetSendNotificationState(); } }
        public int SendNotificationTo { get; set; }
        public string PreventNotificationStatus { get; set; }
        private TimeSpan queryIntervall = new TimeSpan(0, 0, 0, 5);
        public TimeSpan QueryIntervall { get { return queryIntervall; } }
        public int QueryIntervallSeconds { get { return (int)queryIntervall.TotalSeconds; } set { queryIntervall = new TimeSpan(0, 0, value); } }
        public HMNotification LastNotification { get; set; }
        
        private List<HMNotifySilence> silenceTimes = new List<HMNotifySilence>();
        public HMNotifySilence[] SilenceTimes { get { return silenceTimes.ToArray(); } set { silenceTimes.AddRange(value); } }
        public bool IsSilenceTime { get { return GetSilence(); } }

        private List<HMNotifyCondition> conditions = new List<HMNotifyCondition>();
        public HMNotifyCondition[] Conditions { get { return conditions.ToArray(); } }
        public ItemType HMNotifyType
        {
            get
            {
                return (String.IsNullOrEmpty(DeviceAddress) && String.IsNullOrEmpty(DeviceChannel) ?
                    (VariableId <= 0 ? ItemType.Undefined : ItemType.Variable) : ItemType.Device);
            }
        }


        public HMNotifyItem()
        {

        }


        private bool GetSendNotificationState()
        {
            try
            {
                if(GetSilence())
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return true;
            }
        }

        public void AddSilenceTime(HMNotifySilence silence)
        {
            silenceTimes.Add(silence);
        }

        public void AddCondition(HMNotifyCondition condition)
        {
            conditions.Add(condition);
        }

        private bool GetSilence()
        {
            if (silenceTimes.Count > 0)
            {
                foreach (HMNotifySilence silence in silenceTimes)
                {
                    if (silence.IsSilenceNow())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            return String.Format("Notify-{3} in {0}: {1} - {2}", Scope, Name, LastNotification, HMNotifyType);
        }
    }
}
