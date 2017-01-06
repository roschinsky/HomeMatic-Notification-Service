using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRoschinsky.Service.HomeMaticNotification
{
    public class HMNotifierConfig
    {
        public string NotifierConfigFile { get; set; }
        public string HmcUrl { get; set; }
        public int NotifierQueryFullRequestSec { get; set; }
        public string NotificationSmtpHost { get; set; }
        public int NotificationSmtpPort { get; set; }
        public string NotificationSmtpCredUser { get; set; }
        public string NotificationSmtpCredPw { get; set; }
        public string NotificationSmtpMailFrom { get; set; }
        public bool NotificationSmtpUseSSL { get; set; }
    }
}
