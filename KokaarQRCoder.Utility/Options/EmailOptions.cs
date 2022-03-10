using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KokaarQrCoder.Utility.Options
{
    public class EmailOptions
    {
        public const string ConfigSectionName = "EmailOptions";

        public bool WillSendEmail { get; set; }
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
