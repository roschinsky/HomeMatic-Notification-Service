using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The class NotificationPushalot is a specific implementation for 
    /// sending push notifications by a service called Pushalot; see website 
    /// of provider for more information: https://pushalot.com/api
    /// </summary>
    public class NotificationPushalot : Notification
    {
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        public string Link { get; set; }

        public NotificationPushalot(string apiKey, string message, string title)
            : base(apiKey, message, title)
        {
            Initialize();
        }

        public NotificationPushalot(string apiKey, string message, string title, bool isImportant, bool isSilent)
            : base(apiKey, message, title, isImportant, isSilent)
        {
            Initialize();
        }

        private void Initialize()
        {
            apiUrl = "https://pushalot.com/api/sendmessage";
        }

        public override bool Send()
        {
            try
            {
                using (var client = new WebClient())
                {
                    NameValueCollection payload = new NameValueCollection();
                    payload["AuthorizationToken"] = rcpt;
                    payload["Title"] = title.Length > 250 ? title.Substring(0, 250) : title;
                    payload["Body"] = message.Length > 32768 ? message.Substring(0, 32768) : message;
                    payload["IsImportant"] = isImportant.ToString().ToUpper();
                    payload["IsSilent"] = isSilent.ToString().ToUpper();
                    payload["Source"] = source.Length > 25 ? source.Substring(0, 25) : source;
                    payload["Link"] = Link.Length > 1000 ? Link.Substring(0, 1000) : Link;

                    // client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

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
