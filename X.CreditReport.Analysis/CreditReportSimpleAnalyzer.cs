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

                if (start > 0 && end > 0 && start < end)
                    all = AnalysisDebt(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@".*机构查询记录明细.*");
                match = regex.Match(text);
                start = match.Index + match.Length;

                end = text.Length;

                if (start > 0 && end > 0 && start < end)
                    all.Records = AnalysisRecordDetails(text.Substring(start, end - start));
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
            int start1, end1;
            int start2, end2;
            int count1;

            #region 机构查询记录明细
            start1 = 0;

            var regex = new Regex(@"本人查询记录明细");
            var match = regex.Match(textAll);
            end1 = start2 = match.Index;

            var text1 = "\n" + textAll.Substring(start1, end1 - start1);

            //分割每行，取每行数据
            var rows1 = SplitRowSerial(text1);
            count1 = rows1.Count;
            for (int i = 0; i < rows1.Count; i++)
            {
                var row = rows1[i];
                var m = new CREDIT_RECORD_DETAILS();
                list.Add(m);

                //编号
                m.SN = i + 1;
                //查询日期
                regex = new Regex(@"\d{4}\.\d{2}\.\d{2}");
                match = regex.Match(row);
                if (match.Success)
                {
                    m.QUERYDATE = match.Value.XToDateTimeOrNull();
                    row = regex.Replace(row, "").Trim();
                }
                //查询原因
                m.REMARK = GetUntilSpaceFromRow(ref row, PositionInRow.End);
                //查询操作员
                m.OPERATOR = row.Replace(" ", "");
            }
            #endregion

            #region 本人查询记录明细
            end2 = textAll.Length;

            var text2 = "\n" + textAll.Substring(start2, end2 - start2);

            //分割每行，取每行数据
            var rows2 = SplitRowSerial(text2);
            for (int i = 0; i < rows2.Count; i++)
            {
                var row = rows2[i];
                var m = new CREDIT_RECORD_DETAILS();
                list.Add(m);

                //编号
                m.SN = count1 + i + 1;
                //查询日期
                regex = new Regex(@"\d{4}\.\d{2}\.\d{2}");
                match = regex.Match(row);
                if (match.Success)
                {
                    m.QUERYDATE = match.Value.XToDateTimeOrNull();
                    row = regex.Replace(row, "").Trim();
                }
                //查询原因
                m.REMARK = GetUntilSpaceFromRow(ref row, PositionInRow.End);
                //查询操作员
                m.OPERATOR = row.Replace(" ", "");
            }
            #endregion

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

            //去说明
            regex = new Regex(@"^说\s*明", RegexOptions.Multiline);
            var match = regex.Match(text);
            var start = match.Index;
            regex = new Regex(@"请致电全国客户服务热线400-810-8866.*");
            match = regex.Match(text);
            var end = match.Index + match.Length;
            if (start > 0 && end > 0 && start < end)
                text = text.Substring(0, start) + text.Substring(end);

            return text;
        }

        /// <summary>
        /// 切出行，行号是顺序的，如1 2 3 4 5
        /// </summary>
        /// <param name="text"></param>
        /// <param name="numberSplitor">序号分割符</param>
        /// <param name="mergeLine">是否合并为一行(用空格替代换行)</param>
        /// <returns></returns>
        private static List<string> SplitRowSerial(string text, string numberSplitor = @"\s+", bool mergeLine = true)
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
                    textRow = textRow.Replace("\n", " ");
                rows.Add(textRow.Trim());
            }
            return rows;
        }

        /// <summary>
        /// 从行文本中截取信息更新日期，格式 2014. 02. 27
        /// </summary>
        /// <returns></returns>
        private static DateTime? GetUpdateDateFromRow(ref string row)
        {
            var regex = new Regex(@"\d{4}\.\s*\d{2}\.\s*\d{2}");
            var match = regex.Match(row);
            DateTime? r = null;
            if (match.Success)
            {
                r = match.Value.XToDateTimeOrNull();
                row = regex.Replace(row, "").Trim();
            }
            return r;
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

        /// <summary>
        /// 从行文本头或尾开始截取年
        /// </summary>
        /// <param name="row"></param>
        /// <param name="position"></param>
        /// <param name="checkIngnore">是否先从行文本中截取—</param>
        /// <returns></returns>
        private static string GetYearFromRow(ref string row, PositionInRow position, bool checkIngnore = true)
        {
            if (checkIngnore && GetIgnoreSymbolFromRow(ref row, position))
                return IGNORE_SYMBOL;

            Regex regex;
            switch (position)
            {
                case PositionInRow.Start:
                    regex = new Regex(@"^[12]\d{3}");
                    break;
                case PositionInRow.End:
                    regex = new Regex(@"[12]\d{3}$");
                    break;
                default:
                    throw new NotImplementedException("GetYearFromRow不支持此position");
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

        private const string JOB_COMPANYNAME_PATTERN = @"^.+公司\s+
^.+部\s+
^.+店\s+";
        /// <summary>
        /// 配置：工作单位正则表达式
        /// </summary>
        /// <returns></returns>
        private static string[] GetConfigJobCompanyPatterns()
        {
            return JOB_COMPANYNAME_PATTERN.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private const string IGNORE_SYMBOL = "—";

        #endregion
    }
}
