using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderBusinessLogic.Models
{
    public class DiePackAddModel
    {
        public int WaferNumber { get; set; }

        public int GateVoltageId { get; set; }

        public int AnodeVoltageId { get; set; }

        public int Quantity { get; set; }
    }
}
