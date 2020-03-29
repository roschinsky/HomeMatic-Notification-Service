using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The class NotificationTelegram is a specific implementation for 
    /// sending messages via a Telegram messenger bot; see website 
    /// of provider for more information: https://core.telegram.org/bots/api
    /// </summary>
    public class NotificationTelegram : Notification
    {
        // The bot I use here is https://telegram.me/HomematicNotificationBot; feel free to create your 
        // own telegram bot and change the bot key here
        private const string botKey = "269686691:AAFsAA9n4qNUKyYb2PYHac3SpR0GLaBUDDQ";

        // Emojis
        private string icoAppSymbol = Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x8F, 0xA0 }); //\u1F3E0
        private string icoNotifySilent = Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x94, 0x95 }); //\u1F515
        private string icoNotifyNormal = Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x94, 0x94 }); //\u1F514
        private string icoNotifyImportant = Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x93, 0xA2 }); //\u1F4E2
        private string icoCheck = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9C, 0x94 }); //\u2714
        private string icoCross = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9C, 0x96 }); //\u2716
        private string icoQuMark = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9D, 0x93 }); //\u2753
        private string icoExMark = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9D, 0x97 }); //\u2757
        private string icoThumbUp = Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x91, 0x8D }); //\u1F44D
        private string icoThumbDown = Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x91, 0x8E }); //\u1F44E


        public NotificationTelegram(string chatId, string message, string title)
            : base(chatId, message, title)
        {
            Initialize();
        }

        public NotificationTelegram(string chatId, string message, string title, bool isImportant, bool isSilent)
            : base(chatId, message, title, isImportant, isSilent)
        {
            Initialize();
        }

        private void Initialize()
        {
            apiUrl = String.Format("https://api.telegram.org/bot{0}/sendMessage", botKey);
        }

        public override bool Send()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += ValidateCertificate;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    NameValueCollection payload = new NameValueCollection();
                    payload["chat_id"] = rcpt;
                    payload["text"] = FormatMessage();
                    payload["parse_mode"] = "Markdown";
                    if (isSilent && !isImportant)
                    {
                        payload["disable_notification"] = isSilent.ToString().ToUpper();
                    }

                    byte[] response = client.UploadValues(apiUrl, "POST", payload);
                    using (StreamReader reader = new StreamReader(new MemoryStream(response)))
                    {
                        NotificationResponse = reader.ReadToEnd();
                    }
                }

                Log.Add(new Common.JournalEntry(String.Format("Notification was send to '{0}'.", rcpt), this.GetType().Name));
                NotificationSuccessfulSend = true;
                return true;
            }
            catch (WebException exWeb)
            {
                if(exWeb.Response != null)
                {
                    using (StreamReader reader = new StreamReader(exWeb.Response.GetResponseStream()))
                    {
                        NotificationResponse = reader.ReadToEnd();
                    }
                }
                else
                {
                    NotificationResponse = exWeb.Message;
                }
                Log.Add(new Common.JournalEntry("Notification failed: " + NotificationResponse, this.GetType().Name, exWeb));
            }
            catch (Exception ex)
            {
                Log.Add(new Common.JournalEntry("Notification failed: " + NotificationResponse, this.GetType().Name, ex));
            }
            finally
            {
                ServicePointManager.ServerCertificateValidationCallback -= ValidateCertificate;
            }

            NotificationSuccessfulSend = false;
            return false;
        }

        private string FormatMessage()
        {
            string result = String.Empty;
            try
            {
                string headline = String.Format("{0}{1} *{2}*\n", icoAppSymbol, (isImportant ? icoNotifyImportant : (isSilent ? icoNotifySilent : icoNotifyNormal)), title);
                result += headline;

                int endOfFirstLine = message.IndexOf("\n");
                int startOfLastLine = message.LastIndexOf("\n");

                string firstLine = String.Format("_{0}_", message.Substring(0, endOfFirstLine));
                firstLine = firstLine.Replace("'True'", icoCheck);
                firstLine = firstLine.Replace("'False'", icoCross);
                result += firstLine;

                string checks = String.Format("```{0}```", message.Substring(endOfFirstLine, startOfLastLine - endOfFirstLine));
                result += checks;
            }
            catch (Exception ex)
            {
                Log.Add(new Common.JournalEntry("Formatting message failed.", this.GetType().Name, ex));
            }

            return result;
        }

        #region Helper

        /// <summary>
        /// Certificate validation
        /// </summary>
        private bool ValidateCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // if certificate is invalid, log error and return false
            if (error != SslPolicyErrors.None)
            {
                Log.Add(new Common.JournalEntry(String.Format("Certificate '{0}' policy error: '{1}'", cert.Subject, error), this.GetType().Name, true));
                return false;
            }
            return true;
        }

        #endregion
    }
}
