using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderUI.Email
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string ConfirmationEmailAddress { get; set; }
    }
}
