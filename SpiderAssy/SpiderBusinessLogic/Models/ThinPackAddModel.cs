using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderBusinessLogic.Models
{
    public class ThinPackAddModel
    {
        public int LotCode { get; set; }

        public int GateVoltageId { get; set; }

        public int AnodeVoltageId { get; set; }

        public int Quantity { get; set; }
    }
}
