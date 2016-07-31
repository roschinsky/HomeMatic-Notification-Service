using System;
using TRoschinsky.Lib.HomeMaticXmlApi;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// A HMNotification represents an occurred event discovered by the logic 
    /// of the HomeMaticNotification service in HMNotifier. The HMNotification 
    /// contains all the specific information about this event.
    /// </summary>
    public class HMNotification
    {
        public string Scope { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public HMDeviceDataPoint DataPoint { get; set; }
        public HMSystemVariable Variable { get; set; }
        private DateTime timeStamp = DateTime.Now;
        public DateTime TimeStamp { get { return timeStamp; } }
        public bool NotificationSent { get; set; }

        public override string ToString()
        {
            if (DataPoint != null)
            {
                return String.Format("{1}: {3} @{2}", Name, Address, TimeStamp, DataPoint.Value);
            }
            else if(Variable != null)
            {
                return String.Format("IseID #{1}: {0} @{2}", Name, Address, TimeStamp);
            }
            else
            {
                return String.Format("{1}: {0}? @{2}", Name, Address, TimeStamp);
            }
        }
    }
}
