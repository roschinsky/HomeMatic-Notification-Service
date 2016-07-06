﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The class NotificationPushover is a specific implementation for 
    /// sending push notifications by a service called Pushover; see website 
    /// of provider for more information: https://pushover.net/api
    /// </summary>
    public class NotificationPushover : Notification
    {
        private const string appKey = "asVtVWnwRNEuHE9Us2i3dbkXGWQ94K";

        public NotificationPushover(string apiKey, string message, string title)
            : base(apiKey, message, title)
        {
            Initialize();
        }

        public NotificationPushover(string apiKey, string message, string title, bool isImportant, bool isSilent)
            : base(apiKey, message, title, isImportant, isSilent)
        {
            Initialize();
        }

        private void Initialize()
        {
            apiUrl = "https://api.pushover.net/1/messages.json";
        }

        public override bool Send()
        {
            try
            {
                int priority = 0;
                if (isImportant) { priority = 1; }
                else if (isSilent) { priority = -1; }

                using (var client = new WebClient())
                {
                    NameValueCollection payload = new NameValueCollection();
                    payload["token"] = appKey;
                    payload["user"] = rcpt;
                    payload["title"] = title.Length > 250 ? title.Substring(0, 250) : title;
                    payload["message"] = message.Length > 1024 ? message.Substring(0, 1024) : message;
                    if (priority != 0)
                    {
                        payload["priority"] = isSilent.ToString().ToUpper();
                    }

                    byte[] response = client.UploadValues(apiUrl, payload);
                    using (StreamReader reader = new StreamReader(new MemoryStream(response)))
                    {
                        NotificationWebResponse = reader.ReadToEnd();
                    }
                }

                NotificationSuccessfulSend = true;
                return true;
            }
            catch (WebException exWeb)
            {
                using (StreamReader reader = new StreamReader(exWeb.Response.GetResponseStream()))
                {
                    NotificationWebResponse = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                // TODO: Implement error handling
            }

            NotificationSuccessfulSend = false;
            return false;
        }
    }
}
