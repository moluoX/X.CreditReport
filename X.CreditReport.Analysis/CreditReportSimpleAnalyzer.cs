using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.CreditReport.Analysis.Model;
using X.CreditReport.Analysis.ModelSimple;

namespace X.CreditReport.Analysis
{
    public class CreditReportSimpleAnalyzer
    {
        #region 解析征信报告

        public static CreditReportSimple Analyze(string path)
        {
            Regex regex;
            Match match;

            //pdf -> string
            var text = ReadPdf(path);
            text = Pretreatment(text);

            //定位，截取
            var all = new CreditReportSimple();
            {
                int start, end;
                regex = new Regex(@".*个人信用报告.*");
                match = regex.Match(text);
                start = match.Index;

                regex = new Regex(@".*机构查询记录明细.*");
                match = regex.Match(text);
                end = match.Index;

                if (end > 0 && start < end)
                    all = AnalysisDebt(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@".*机构查询记录明细.*");
                match = regex.Match(text);
                start = match.Index + match.Length;

                regex = new Regex(@".*本人查询记录明细.*");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Records = AnalysisRecordDetails(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@".*本人查询记录明细.*");
                match = regex.Match(text);
                start = match.Index + match.Length;

                end = text.Length;

                if (start > 0 && end > 0 && start < end)
                    all.RecordsSelf = AnalysisRecordDetailsSelf(text.Substring(start, end - start));
            }
            return all;
        }

        private static CreditReportSimple AnalysisDebt(string textAll)
        {
            var m = new CreditReportSimple();

            #region 征信空白
            var regex = new Regex(@"发放的");
            var match = regex.Match(textAll);
            m.IsEmpty = !match.Success;
            #endregion

            #region 坏账
            regex = new Regex(@"呆账/资产处置/担保人代偿/冻结/止付/核销");
            match = regex.Match(textAll);
            m.IsBreakingBad = match.Success;
            #endregion

            return m;
        }

        private static IList<CREDIT_RECORD_DETAILS> AnalysisRecordDetails(string textAll)
        {
            var list = new List<CREDIT_RECORD_DETAILS>();

            //分割每行，取每行数据
            var rows = SplitRowSerial(textAll, lineBreakReplace: "");
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var m = new CREDIT_RECORD_DETAILS();
                list.Add(m);

                //编号
                m.SN = i + 1;
                //查询日期
                var regex = new Regex(@"\d{4}年\d{1,2}月\d{1,2}日");
                var match = regex.Match(row);
                if (match.Success)
                {
                    m.QUERYDATE = match.Value.XToDateTimeOrNull();
                    row = regex.Replace(row, "").Trim();
                }
                //查询操作员
                m.OPERATOR = GetUntilSpaceFromRow(ref row, PositionInRow.Start);
                //查询原因
                m.REMARK = row.Replace(" ", "");

                //处理查询操作员与查询原因之间无空格的情况
                if (string.IsNullOrWhiteSpace(m.REMARK))
                {
                    regex = new Regex(QUERY_REASON_PATTERN);
                    match = regex.Match(m.OPERATOR);
                    if (match.Success)
                    {
                        m.REMARK = match.Value;
                        m.OPERATOR = regex.Replace(m.OPERATOR, "");
                    }
                }
            }

            return list;
        }

        private static IList<CREDIT_RECORD_DETAILS> AnalysisRecordDetailsSelf(string textAll)
        {
            var list = new List<CREDIT_RECORD_DETAILS>();

            //分割每行，取每行数据
            var rows = SplitRowSerial(textAll, lineBreakReplace: "");
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var m = new CREDIT_RECORD_DETAILS();
                list.Add(m);

                //编号
                m.SN = i + 1;
                //查询日期
                var regex = new Regex(@"\d{4}年\d{1,2}月\d{1,2}日");
                var match = regex.Match(row);
                if (match.Success)
                {
                    m.QUERYDATE = match.Value.XToDateTimeOrNull();
                    row = regex.Replace(row, "").Trim();
                }
                //查询操作员
                regex = new Regex(@"本人查询.*");
                match = regex.Match(row);
                if (match.Success)
                {
                    m.OPERATOR = match.Value.Replace(" ", "");
                    row = regex.Replace(row, "").Trim();
                }
                //查询原因
                m.REMARK = row.Replace(" ", "");
            }

            return list;
        }

        #endregion

        #region 工具

        private static string ReadPdf(string path)
        {
            PDDocument doc = PDDocument.load(path);
            PDFTextStripper stripper = new PDFTextStripper();
            string text = stripper.getText(doc);
            return text;
        }

        private static string Pretreatment(string text)
        {
            //var r = PDFTextParser.Default.ParseText(text);
            //return r;

            //去分页
            var regex = new Regex(@".*第\d*页.*共\d*页.*");
            text = regex.Replace(text, "");

            //去1. 2. 3. 4.
            regex = new Regex(@"\d+\.\s*\n");
            text = regex.Replace(text, "");

            //去
            var patterns = GetConfigTrimPatterns();
            foreach (var pattern in patterns)
            {
                regex = new Regex(pattern);
                text = regex.Replace(text, "");
            }

            return text;
        }

        /// <summary>
        /// 切出行，行号是顺序的，如1 2 3 4 5
        /// </summary>
        /// <param name="text"></param>
        /// <param name="numberSplitor">序号分割符</param>
        /// <param name="mergeLine">是否合并为一行(用lineBreakReplace替代换行符)</param>
        /// <param name="lineBreakReplace">替代换行符</param>
        /// <returns></returns>
        private static List<string> SplitRowSerial(string text, string numberSplitor = @"\s+", bool mergeLine = true, string lineBreakReplace = " ")
        {
            //分割每行
            var splitors = new List<Tuple<int, int>>();
            Regex regex;
            Match match;
            for (int i = 1; ; i++)
            {
                regex = new Regex(@"\n+" + i + numberSplitor);
                match = regex.Match(text);
                if (!match.Success)
                    break;
                splitors.Add(new Tuple<int, int>(match.Index, match.Index + match.Length));
            }

            //取每行文本，换行转空格
            var rows = new List<string>();
            for (int i = 0; i < splitors.Count; i++)
            {
                var startThis = splitors[i].Item2;
                var endThis = splitors.Count > i + 1 ? splitors[i + 1].Item1 : text.Length;
                var textRow = text.Substring(startThis, endThis - startThis);
                if (mergeLine)
                    textRow = textRow.Replace("\r\n", lineBreakReplace).Replace("\n", lineBreakReplace);
                rows.Add(textRow.Trim());
            }
            return rows;
        }

        /// <summary>
        /// 从行文本中截取—
        /// </summary>
        /// <returns></returns>
        private static bool GetIgnoreSymbolFromRow(ref string row, PositionInRow position)
        {
            Regex regex;
            switch (position)
            {
                case PositionInRow.Start:
                    regex = new Regex(@"^" + IGNORE_SYMBOL + @"\s*");
                    break;
                case PositionInRow.End:
                    regex = new Regex(@"\s*" + IGNORE_SYMBOL + @"$");
                    break;
                default:
                    regex = new Regex(@"\s" + IGNORE_SYMBOL + @"\s+");
                    break;
            }
            var match = regex.Match(row);
            bool r = false;
            if (match.Success)
            {
                r = true;
                row = regex.Replace(row, "").Trim();
            }
            return r;
        }

        /// <summary>
        /// 从行文本头或尾开始截取文本直到空白符
        /// </summary>
        /// <param name="row"></param>
        /// <param name="position"></param>
        /// <param name="checkIngnore">是否先从行文本中截取—</param>
        /// <returns></returns>
        private static string GetUntilSpaceFromRow(ref string row, PositionInRow position, bool checkIngnore = true)
        {
            if (checkIngnore && GetIgnoreSymbolFromRow(ref row, position))
                return IGNORE_SYMBOL;

            Regex regex;
            switch (position)
            {
                case PositionInRow.Start:
                    regex = new Regex(@"^\S+\s*");
                    break;
                case PositionInRow.End:
                    regex = new Regex(@"\s*\S+$");
                    break;
                default:
                    throw new NotImplementedException("GetUntilSpaceFromRow不支持此position");
            }
            var match = regex.Match(row);
            string r = "";
            if (match.Success)
            {
                r = match.Value.Trim();
                row = regex.Replace(row, "").Trim();
            }
            return r;
        }

        #endregion

        #region 配置

        private const string TRIM_PATTERN = @".*说\s*明.*\r?\n
.*本报告中的信息是依据.*\r?\n
.*整合的全过程中保持客观.*\r?\n
.*影响您信用评价的主要信息.*\r?\n
.*可访问征信中心门户网站.*\r?\n
.*可联系数据提供单位.*\r?\n
.*请妥善保管.*\r?\n
.*请致电全国客户服务热线.*\r?\n";
        /// <summary>
        /// 配置：工作单位正则表达式
        /// </summary>
        /// <returns></returns>
        private static string[] GetConfigTrimPatterns()
        {
            return TRIM_PATTERN.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private const string IGNORE_SYMBOL = "—";

        private const string QUERY_REASON_PATTERN = @"保前审查|保后管理|贷款审批|贷后管理|担保资格审查|信用卡审批";

        #endregion
    }
}
