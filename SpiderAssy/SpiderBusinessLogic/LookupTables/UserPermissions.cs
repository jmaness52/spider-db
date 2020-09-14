using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderDatabase;
using SpiderBusinessLogic.Models;
using SpiderDatabase.Procedures;

namespace SpiderBusinessLogic.LookupTables
{
    public class UserPermissions
    {
        private const string AdminLevel = "Admin";
        private const string StandardLevel = "Standard User";
        public List<UserPermissionsModel> Permissions { get; set; }

        public UserPermissionsModel Admin => Permissions.Where(x => x.LevelName.Equals(AdminLevel)).FirstOrDefault();

        public UserPermissionsModel Standard => Permissions.Where(x => x.LevelName.Equals(StandardLevel)).FirstOrDefault();

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
