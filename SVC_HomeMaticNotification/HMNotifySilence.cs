using System;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The HMNotifySilence is similar to the HMNotifyCondition. It also can be used to prevent 
    /// some events of raising a notification by setting up one of this conditions. The difference 
    /// is to define a time span in which not notification should be sent, instead of choosing 
    /// devices or variables for blocking a transmission.
    /// It contains logic to determine if the silence time is right now.
    /// </summary>
    public class HMNotifySilence
    {
        private int silenceStartHour = 0;
        private int silenceStartMinute = 0;
        private int silenceEndHour = 0;
        private int silenceEndMinute = 0;
        private DayOfWeek silenceDay = DayOfWeek.Monday;

        public HMNotifySilence(int startHour, int startMinute, int endHour, int endMinute, DayOfWeek day)
        {
            silenceStartHour = startHour;
            silenceStartMinute = startMinute;
            silenceEndHour = endHour;
            silenceEndMinute = endMinute;
            silenceDay = day;
        }

        public HMNotifySilence(DateTime start, DateTime end)
        {
            if((end - start).TotalHours > 24 || end.DayOfWeek != start.DayOfWeek)
            {
                throw new HMNException("Setup of notification silences of greater than 24 hours or differnt days are not supported.", null);
            }
            else
            {
                silenceStartHour = start.Hour;
                silenceStartMinute = start.Minute;
                silenceEndHour = end.Hour;
                silenceEndMinute = end.Minute;
                silenceDay = start.DayOfWeek;    
            }
        }

        public bool IsSilenceNow()
        {
            if ((silenceStartHour * 60 + silenceStartMinute) < (silenceEndHour * 60 + silenceEndMinute))
            {
                DateTime now = DateTime.Now;
                if(now.DayOfWeek == silenceDay)
                {
                    if ((now.Hour >= silenceStartHour && now.Minute >= silenceStartMinute) &&
                        (now.Hour <= silenceEndHour && now.Minute <= silenceEndMinute))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return String.Format("Silence on {0} from {1:D2}:{2:D2}h to {3:D2}:{4:D2}h", silenceDay, silenceStartHour, silenceStartMinute, silenceEndHour, silenceEndMinute);
        }
    }
}
