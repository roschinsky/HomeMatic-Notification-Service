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
        public string DeviceAddress { get; set; }
        public HMDeviceDataPoint DataPoint { get; set; }
        private DateTime timeStamp = DateTime.Now;
        public DateTime TimeStamp { get { return timeStamp; } }
        public bool NotificationSent { get; set; }

        public override string ToString()
        {
            if (DataPoint != null)
            {
                return String.Format("{2}: {4} @{3}", Scope, Name, DeviceAddress, TimeStamp, DataPoint.Value);
            }
            else
            {
                return String.Format("{2}: Unknown @{3}", Scope, Name, DeviceAddress, TimeStamp);
            }
        }
    }
}
