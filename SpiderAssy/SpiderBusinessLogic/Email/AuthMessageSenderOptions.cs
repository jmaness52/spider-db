﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderBusinessLogic.Email
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }

        public string SendGridKey { get; set; }

        public string AdminEmailAddress { get; set; }

        public string SenderEmailAddress { get; set; }
    }
}
