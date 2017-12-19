using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.CreditReport.Analysis.Model
{
    public class CREDIT_ALL
    {
        public CREDIT_PERSONALINFO Personal { get; set; }
        public IList<CREDIT_LIVINGINFO> Livings { get; set; }
        public IList<CREDIT_JOBINFO> Jobs { get; set; }
        public CREDIT_DEBTSTATISTIC Debt { get; set; }
        public IList<CREDIT_DAIKUAN_ALL> Daikuans { get; set; }
        public IList<CREDIT_DAIJIKA_ALL> Daijikas { get; set; }
        public IList<CREDIT_RECORD_DETAILS> Records { get; set; }
    }
}
