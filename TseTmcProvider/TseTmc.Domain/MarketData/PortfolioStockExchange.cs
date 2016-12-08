using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TseTmc.Domain
{
  
        public enum PortfolioStockExchange
        {
            ABD = 1,
            [Description("TSE")]
            TSE = 2,
            [Description("IFB")]
            IFB = 3,
            [Description("IRENEX")]
            IRENEX = 4,
            [Description("IRENEX")]
            IME = 5,
            [Description("Unknown")]
            Unknown = 6
        }
}
