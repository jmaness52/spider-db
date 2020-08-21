using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderDatabase.Procedures
{
    public static class FetchData
    {
        public static string Get_All_UserPermissions => "CALL Get_All_UserPermissions();";
    }
}
