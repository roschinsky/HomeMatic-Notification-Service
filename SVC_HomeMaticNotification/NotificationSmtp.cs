using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The class NotificationSmtp is a specific implementation for 
    /// sending eMail notifications by SMTP protocol; see RFC 5321
    /// </summary>
    public class NotificationSmtp : Notification
    {
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        public string Link { get; set; }
        public Tuple<string, int, string, string, string, bool> SmtpConfig { get; set; }

        public NotificationSmtp(string mailAddress, string message, string subject)
            : base(mailAddress, message, subject)
        {
            Initialize();
        }

        public NotificationSmtp(string mailAddress, string message, string subject, bool isImportant, bool isSilent)
            : base(mailAddress, message, subject, isImportant, isSilent)
        {
            Initialize();
        }

        private void Initialize()
        {
            
        }

        public override bool Send()
        {
            try
            {
                using (var client = new SmtpClient(SmtpConfig.Item1))
                {
                    // Setup port if set
                    if(SmtpConfig.Item2 > 0 && SmtpConfig.Item2 != 25 && SmtpConfig.Item2 < 65536)
                    {
                        client.Port = SmtpConfig.Item2;
                    }

                    // Setup credentials if set
                    if(!String.IsNullOrWhiteSpace(SmtpConfig.Item4) && !String.IsNullOrWhiteSpace(SmtpConfig.Item5))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(SmtpConfig.Item3, SmtpConfig.Item4);
                    }

                    // Setup SSL if needed
                    if(SmtpConfig.Item6)
                    {
                        client.EnableSsl = true;
                    }

                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Timeout = 5000;

                    MailMessage payload = new MailMessage(SmtpConfig.Item5, rcpt);
                    payload.Subject = title.Length > 250 ? title.Substring(0, 250) : title;
                    payload.BodyEncoding = UTF8Encoding.UTF8;
                    payload.IsBodyHtml = false;
                    payload.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    payload.Body = message.Length > 32768 ? message.Substring(0, 32768) : message;
                    if (!String.IsNullOrEmpty(source))
                    {
                        payload.Headers.Add("HMC-Src", source.Length > 25 ? source.Substring(0, 25) : source);
                    }
                    if (!String.IsNullOrEmpty(Link))
                    {
                        payload.Headers.Add("HMC-Lnk", Link.Length > 1000 ? Link.Substring(0, 1000) : Link);
                    }

                    if(isImportant)
                    {
                        payload.Priority = MailPriority.High;
                    }
                    else if(isSilent)
                    {
                        payload.Priority = MailPriority.Low;
                    }
                    else
                    {
                        payload.Priority = MailPriority.Normal;
                    }

                    client.Send(payload);
                }

                Log.Add(new Common.JournalEntry(String.Format("Notification was send to '{0}'.", rcpt), this.GetType().Name));
                NotificationSuccessfulSend = true;
                return true;
            }
            catch (SmtpException exMail)
            {
                Log.Add(new Common.JournalEntry(String.Format("Notification failed by server with code '{0}'.", exMail.StatusCode), this.GetType().Name, exMail));
            }
            catch (Exception ex)
            {
                Log.Add(new Common.JournalEntry("Notification failed unexpectedly.", this.GetType().Name, ex));
            }

            NotificationSuccessfulSend = false;
            return false;
        }
    }
}
