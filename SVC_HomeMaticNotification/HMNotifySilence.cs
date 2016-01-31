using System;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The HMNotifySilence is similar to the HMNotifyCondition. It also can be used to prevent 
    /// some events of raising a notification by setting up one of this conditions. The difference 
    /// is to define a time span in which a notification should not be sent, instead of choosing 
    /// devices or variables for blocking a transmission.
    /// It contains logic to determine if the silence time is right now.
    /// </summary>
    public class HMNotifySilence
    {
        private TimeSpan silenceStart;
        private TimeSpan silenceEnd;
        private DayOfWeek silenceDay = DayOfWeek.Monday;

        public HMNotifySilence(int startHour, int startMinute, int endHour, int endMinute, DayOfWeek day)
        {
            silenceStart = new TimeSpan(startHour, startMinute, 0);
            silenceEnd = new TimeSpan(endHour, endMinute, 0);
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
                silenceStart = new TimeSpan(start.Hour, start.Hour, 0);
                silenceEnd = new TimeSpan(end.Hour, end.Hour, 0);
                silenceDay = start.DayOfWeek;    
            }
        }

        public bool IsSilenceNow(DateTime currentDateTime)
        {
            if (currentDateTime.DayOfWeek == silenceDay)
            {
                // day is a match so lets compare the time of the day
                TimeSpan currentTimeSpan = currentDateTime.TimeOfDay;

                if (silenceStart < silenceEnd)
                {
                    // start comes before end, so lets compare straight foreward
                    return silenceStart <= currentTimeSpan && currentTimeSpan <= silenceEnd;
                }
                else
                {
                    // otherwise start is after end, so we need to inverse the comparison
                    return !(silenceEnd < currentTimeSpan && currentTimeSpan < silenceStart);
                }
            }

            return false;
        }

        public bool IsSilenceNow()
        {
            return IsSilenceNow(DateTime.Now);
        }

        public override string ToString()
        {
            return String.Format("Silence on {0} from {1:D2}:{2:D2}h to {3:D2}:{4:D2}h", silenceDay, silenceStart.Hours, silenceStart.Minutes, silenceEnd.Hours, silenceEnd.Minutes);
        }
    }
}
