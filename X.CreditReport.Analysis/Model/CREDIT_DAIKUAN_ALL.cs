using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.CreditReport.Analysis.Model
{
    public class CREDIT_DAIKUAN_ALL : CREDIT_DAIKUAN
    {
        public CREDIT_DAIKUAN_BACK Back { get; set; }
        public IList<CREDIT_DAIKUAN_OVERDUE> Overdues { get; set; }
    }
}
