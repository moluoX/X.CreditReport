using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.CreditReport.Analysis.Model
{
    public class CREDIT_DAIJIKA_ALL : CREDIT_DAIJIKA
    {
        public CREDIT_DAIJIKA_BACK Back { get; set; }
        public IList<CREDIT_DAIJIKA_OVERDUE> Overdues { get; set; }
    }
}
