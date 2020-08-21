using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderDatabase.Models
{
    public class UserPermissionsModel
    {
        public int Id { get; set; }

        public string LevelName { get; set; }

        public int PermissionLevel { get; set; }
    }
}
