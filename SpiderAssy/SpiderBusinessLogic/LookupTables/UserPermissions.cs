using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderDatabase;
using SpiderDatabase.Models;
using SpiderDatabase.Procedures;

namespace SpiderBusinessLogic.LookupTables
{
    public class UserPermissions
    {
        private const string AdminLevel = "Admin";
        public static List<UserPermissionsModel> Permissions { get; set; }

        public static UserPermissionsModel Admin => Permissions.Where(x => x.LevelName.Equals(AdminLevel)).FirstOrDefault();
        public UserPermissions(IDataAccess data)
        {
            LoadLookupData(data);
        }

        private async Task LoadLookupData(IDataAccess data)
        {
            Permissions = await data.LoadData<UserPermissionsModel, dynamic>(FetchData.Get_All_UserPermissions, new { });
        }
    }
}
