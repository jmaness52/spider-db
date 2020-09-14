using SpiderDatabase;
using SpiderDatabase.Procedures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpiderBusinessLogic.LookupTables
{
    public class AnodeVoltages
    {
        public List<int> Voltages;

        public AnodeVoltages(IDataAccess data)
        {
            LoadLookupData(data);
        }

        private async Task LoadLookupData(IDataAccess data)
        {
            Voltages = await data.LoadData<int, dynamic>(FetchData.Get_Anode_Voltages, new { });
        }
    }
}
