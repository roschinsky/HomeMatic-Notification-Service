using System;

namespace TRoschinsky.Service.HomeMaticNotification
{
    public class HMNotifyDestination
    {
        /// <summary>
        /// A HMNotifyDestination defines a recipient of Homematic notifications. It's 
        /// important to choose one of the possilble notify providers and set it up with 
        /// a valid Push-ApiKey or other destination address.
        /// </summary>
        public enum NotifyProviderType
        {
            Pushalot,
            Pushover,
            Email
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string DestinationAddress { get; set; }
        public NotifyProviderType NotifyProvider { get; set; }

        public override string ToString()
        {
            return String.Format("Notify-Destination #{0} '{1}' sends via {3} to {2}", Id, Name, Owner, NotifyProvider);
        }
    }
}
