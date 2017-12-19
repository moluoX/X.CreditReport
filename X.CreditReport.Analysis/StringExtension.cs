using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.CreditReport.Analysis
{
    internal static class StringExtension
    {
        public static DateTime? XToDateTimeOrNull(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            DateTime d;
            if (DateTime.TryParse(s, out d))
                return d;
            return null;
        }

        public static string XToString(this DateTime? s, string format)
        {
            if (!s.HasValue)
                return "";
            return s.Value.ToString(format);
        }

        public static int XToInt(this string s)
        {
            int d;
            if (int.TryParse(s, out d))
                return d;
            return 0;
        }

        public static int? XToIntOrNull(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            int d;
            if (int.TryParse(s, out d))
                return d;
            return null;
        }

        public static decimal? XToDecimalOrNull(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            decimal d;
            if (decimal.TryParse(s, out d))
                return d;
            return null;
        }
    }
}
