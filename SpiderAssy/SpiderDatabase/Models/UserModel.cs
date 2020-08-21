using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderDatabase.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string CompanyName { get; set; }

        public int PermissionLevel { get; set; }
    }
}
