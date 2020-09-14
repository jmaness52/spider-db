using SpiderDatabase;
using SpiderDatabase.Procedures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpiderBusinessLogic.LookupTables
{
    public class GateVoltages
    {
        public List<decimal> Voltages;

        public GateVoltages(IDataAccess data)
        {
            LoadLookupData(data);
        }

        private async Task LoadLookupData(IDataAccess data)
        {
            Voltages = await data.LoadData<decimal, dynamic>(FetchData.Get_Gate_Voltages, new { });
        }
    }
}
