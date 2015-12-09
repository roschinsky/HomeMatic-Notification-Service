using System;

namespace TRoschinsky.Service.HomeMaticNotification
{
    public class HMNotifyDestination
    {
        /// <summary>
        /// A HMNotifyDestination defines a recipient of Homematic notifications. It's 
        /// important to choose one of the possilble push providers and set it up with 
        /// a valid ApiKey.
        /// </summary>
        public enum PushProviderType
        {
            Pushalot,
            Pushover
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string ApiKey { get; set; }
        public PushProviderType PushProvider { get; set; }

        public override string ToString()
        {
            return String.Format("Notify-Destination #{0} '{1}' sends via {3} to {2}", Id, Name, Owner, PushProvider);
        }
    }
}
