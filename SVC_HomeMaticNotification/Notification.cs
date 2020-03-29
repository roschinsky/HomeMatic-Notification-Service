using System;
using System.Collections.Generic;
using TRoschinsky.Common;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The class Notification represents the abstract base class of all notification 
    /// templates the application is able use.
    /// </summary>
    public abstract class Notification
    {
        protected string apiUrl;
        protected string rcpt;
        protected string message;
        protected string title;
        protected string source = "HomeMaticNotification";
        protected bool isImportant;
        protected bool isSilent;
        public List<JournalEntry> Log = new List<JournalEntry>();
        public bool NotificationSuccessfulSend = false;
        public string NotificationResponse = String.Empty;


        public Notification(string rcptTo, string message, string title, bool isImportant, bool isSilent)
        {
            this.rcpt = rcptTo;
            this.message = message;
            this.title = title;
            this.isImportant = isImportant;
            this.isSilent = isSilent;
        }

        public Notification(string rcptTo, string message, string title)
        {
            this.rcpt = rcptTo;
            this.message = message;
            this.title = title;
            this.isImportant = false;
            this.isSilent = false;
        }

        public virtual bool Send()
        {
            return false;
            throw new NotImplementedException("No send method implemented in abstract Notification class; please use specific Notification type class!");
        }
    }
}
