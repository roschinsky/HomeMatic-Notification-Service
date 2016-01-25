﻿using System;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The class Notification represents the abstract base class of all notification 
    /// templates the application is able use.
    /// </summary>
    public abstract class Notification
    {
        protected string apiUrl;
        protected string apiKey;
        protected string message;
        protected string title;
        protected string source = "HomeMaticNotification";
        protected bool isImportant;
        protected bool isSilent;
        public bool NotificationSuccessfulSend = false;
        public string NotificationWebResponse = String.Empty;


        public Notification(string apiKey, string message, string title, bool isImportant, bool isSilent)
        {
            this.apiKey = apiKey;
            this.message = message;
            this.title = title;
            this.isImportant = isImportant;
            this.isSilent = isSilent;
        }

        public Notification(string apiKey, string message, string title)
        {
            this.apiKey = apiKey;
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