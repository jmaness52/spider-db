using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpiderBusinessLogic.Models
{
    public class AddUserModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAdress { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public int PermissionLevelID { get; set; }
    }
}
