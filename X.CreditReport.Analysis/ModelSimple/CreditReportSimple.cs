using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.CreditReport.Analysis.Model;

namespace X.CreditReport.Analysis.ModelSimple
{
    public class CreditReportSimple
    {
        public bool? IsEmpty { get; set; }
        public bool? IsBreakingBad { get; set; }
        public IList<CREDIT_RECORD_DETAILS> Records { get; set; }
        public IList<CREDIT_RECORD_DETAILS> RecordsSelf { get; set; }
    }
}
