using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.CreditReport.Analysis.Model;

namespace X.CreditReport.Analysis
{
    public class CreditReportAnalyzer
    {
        #region 解析征信报告

        public static CREDIT_ALL Analyze(string path)
        {
            Regex regex;
            Match match;

            //pdf -> string
            var text = ReadPdf(path);
            text = Pretreatment(text);

            //定位，截取
            var all = new CREDIT_ALL();
            {
                int start, end;
                regex = new Regex(@"报告编号");
                match = regex.Match(text);
                start = match.Index;

                regex = new Regex(@"配偶信息");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Personal = AnalysisPersonal(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@"\n+\s*居住信息\s*\n+");
                match = regex.Match(text);
                start = match.Index + match.Length;

                regex = new Regex(@"\n+\s*职业信息\s*\n+");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Livings = AnalysisLiving(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@"\n+\s*职业信息\s*\n+");
                match = regex.Match(text);
                start = match.Index + match.Length;

                regex = new Regex(@"\n+[\s|二]*信息概要\s*\n+");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Jobs = AnalysisJob(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@".*授信及负债信息概要");
                match = regex.Match(text);
                start = match.Index;

                regex = new Regex(@".*信贷交易信息明细");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Debt = AnalysisDebtStatistic(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@"信贷交易信息明细\s贷款");
                match = regex.Match(text);
                start = match.Index + match.Length;

                regex = new Regex(@"\n\s*贷记卡\s*\n");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Daikuans = AnalysisDaikuan(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@"\n\s*贷记卡\s*\n");
                match = regex.Match(text);
                start = match.Index + match.Length;

                regex = new Regex(@"\n.*公共信息明细.*\n");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Daijikas = AnalysisDaijika(text.Substring(start, end - start));
            }
            {
                int start, end;
                regex = new Regex(@".*机构查询记录明细.*");
                match = regex.Match(text);
                start = match.Index + match.Length;

                regex = new Regex(@".*报告说明.*");
                match = regex.Match(text);
                end = match.Index;

                if (start > 0 && end > 0 && start < end)
                    all.Records = AnalysisRecordDetails(text.Substring(start, end - start));
            }
            return all;
        }

        private static CREDIT_PERSONALINFO AnalysisPersonal(string textAll)
        {
            var m = new CREDIT_PERSONALINFO();
            var regex = new Regex(@"(?<=报告时间\s*[：:]\s*)\d{4}\.\d{2}\.\d{2}\s\d{2}:\d{2}:\d{2}");
            var match = regex.Match(textAll);
            if (match.Success)
            {
                m.REPORTDATE = match.Value.XToDateTimeOrNull() ?? DateTime.Now;
            }

            #region 姓名行
            int start1, end1;
            regex = new Regex(@".*被查询者证件号码.*");
            match = regex.Match(textAll);
            start1 = match.Index + match.Length;

            regex = new Regex(@".*个人基本信息.*");
            match = regex.Match(textAll, start1);
            end1 = match.Index;

            if (start1 >= 0 && end1 > 0 && start1 < end1)
            {
                var text = textAll.Substring(start1, end1 - start1).Replace("\n", " ").Trim();
                //被查询者姓名
                m.CUSTNAME = GetUntilSpaceFromRow(ref text, PositionInRow.Start);
                //身份证号
                regex = new Regex(@"\d{17}[\d|x]|\d{15}");
                match = regex.Match(text);
                if (match.Success)
                {
                    m.IDCARD = match.Value;
                    text = text.Substring(match.Index + match.Length);
                }
                //查询原因
                m.CHECKCAUSE = GetUntilSpaceFromRow(ref text, PositionInRow.End);
            }
            #endregion

            #region 性别行
            int start2, end2;
            regex = new Regex(@".*手机号码.*");
            match = regex.Match(textAll, end1);
            start2 = match.Index + match.Length;

            regex = new Regex(@".*数据发生.*");
            match = regex.Match(textAll, start2);
            end2 = match.Index;

            if (start2 > 0 && end2 > 0 && start2 < end2)
            {
                var text = textAll.Substring(start2, end2 - start2).Replace("\n", " ").Trim();
                //性别
                regex = new Regex(@"男性|女性");
                match = regex.Match(text);
                if (match.Success)
                {
                    m.GENDER = match.Value;
                    text = text.Substring(match.Index + match.Length);
                }
                //出生日期
                regex = new Regex(@"\d{4}\.\d{2}\.\d{2}");
                match = regex.Match(text);
                if (match.Success)
                {
                    m.BIRTHDAY = match.Value.XToDateTimeOrNull();
                    text = text.Substring(match.Index + match.Length);
                }
                //手机号码
                regex = new Regex(@"0?(13|14|15|17|18|19)[0-9]{9}");
                match = regex.Match(text);
                if (match.Success)
                {
                    m.CELLPHONE = match.Value;
                    text = regex.Replace(text, "");
                }
                //婚姻状况
                m.MARRYSTATUS = text.Trim();
            }
            #endregion

            #region 单位电话行
            int start3, end3;
            regex = new Regex(@".*学位.*");
            match = regex.Match(textAll, end2);
            start3 = match.Index + match.Length;

            regex = new Regex(@".*数据发生.*");
            match = regex.Match(textAll, start3);
            end3 = match.Index;

            if (start3 > 0 && end3 > 0 && start3 < end3)
            {
                var text = textAll.Substring(start3, end3 - start3).Replace("\n", " ").Trim();
                //单位电话
                m.COMPANYTEL = GetUntilSpaceFromRow(ref text, PositionInRow.Start);
                //住宅电话
                m.HOMETEL = GetUntilSpaceFromRow(ref text, PositionInRow.Start);
                //学位
                m.DEGREE = GetUntilSpaceFromRow(ref text, PositionInRow.End);
                //学历
                m.EDUCATION = text.Trim();
            }
            #endregion

            #region 通讯地址行
            int start4, end4;
            regex = new Regex(@".*户痛奢地址.*");
            match = regex.Match(textAll, end3);
            start4 = match.Index + match.Length;

            regex = new Regex(@".*数据发生.*");
            match = regex.Match(textAll, start4);
            end4 = match.Index;

            if (start4 > 0 && end4 > 0 && start4 < end4)
            {
                var text = textAll.Substring(start4, end4 - start4).Replace("\n", " ").Trim();
                //通讯地址
                m.POSTALADDR = GetUntilSpaceFromRow(ref text, PositionInRow.Start);
                //户籍地址
                m.HOUSEHOLDADDR = text.Trim();
            }
            #endregion

            return m;
        }

        private static IList<CREDIT_LIVINGINFO> AnalysisLiving(string textAll)
        {
            var list = new List<CREDIT_LIVINGINFO>();

            int start, end;

            var regex = new Regex(@"信息更新日期\s*\n+");
            var match = regex.Match(textAll);
            start = match.Index + match.Length;

            regex = new Regex(@"\n+.*数据发生机构名称\s*\n+");
            match = regex.Match(textAll);
            end = match.Index;

            if (start >= end)
                return list;
            var text = "\n" + textAll.Substring(start, end - start);

            //分割每行，取每行数据
            var rows = SplitRowSerial(text);
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var m = new CREDIT_LIVINGINFO();
                list.Add(m);

                //编号
                m.SN = i + 1;
                //信息更新日期
                m.UPDATEDATE = GetUpdateDateFromRow(ref row);
                //居住状况 从行尾往前追溯到空格
                regex = new Regex(@"[^\s]+$");
                match = regex.Match(row);
                m.STATUS = match.Value;
                //居住地址 取剩下的，去掉空格
                m.ADDRESS = regex.Replace(row, "").Replace(" ", "");
            }

            return list;
        }

        private static IList<CREDIT_JOBINFO> AnalysisJob(string textAll)
        {
            var list = new List<CREDIT_JOBINFO>();

            #region 工作单位部分
            int start1, end1;

            var regex = new Regex(@"单位地址\s*\n");
            var match = regex.Match(textAll);
            start1 = match.Index + match.Length;

            regex = new Regex(@".*(编号\s职业\s行业|进入本单).*");
            match = regex.Match(textAll);
            end1 = match.Index;

            if (start1 >= end1)
                return list;
            var text1 = "\n" + textAll.Substring(start1, end1 - start1);

            //分割每行，取每行数据
            var rows1 = SplitRowSerial(text1);
            var jobCompanyPatterns = GetConfigJobCompanyPatterns();
            for (int i = 0; i < rows1.Count; i++)
            {
                var row = rows1[i];
                var m = new CREDIT_JOBINFO();
                list.Add(m);

                //编号
                m.SN = i + 1;
                //单位地址是否未填
                if (GetIgnoreSymbolFromRow(ref row, PositionInRow.End))
                {
                    m.COMPANYNAME = regex.Replace(row, "").Replace(" ", "").Trim();
                    m.COMPANYADDR = "—";
                    continue;
                }
                //工作单位是否未填
                if (GetIgnoreSymbolFromRow(ref row, PositionInRow.Start))
                {
                    m.COMPANYNAME = IGNORE_SYMBOL;
                    m.COMPANYADDR = regex.Replace(row, "").Replace(" ", "").Trim();
                    continue;
                }
                //工作单位
                bool matched = false;
                foreach (var pattern in jobCompanyPatterns)
                {
                    //先用配置匹配
                    regex = new Regex(pattern);
                    match = regex.Match(row);
                    if (match.Success)
                    {
                        matched = true;
                        m.COMPANYNAME = match.Value.Replace(" ", "").Trim();
                        row = regex.Replace(row, "");
                        break;
                    }
                }
                if (!matched)
                {
                    //未匹配到，从第一个空格分割
                    var spaceIndex = row.IndexOf(" ");
                    if (spaceIndex > 0)
                    {
                        matched = true;
                        m.COMPANYNAME = row.Substring(0, spaceIndex).Trim();
                        row = row.Substring(spaceIndex);
                    }
                }
                if (!matched)
                {
                    //仍未匹配到，本行全部文本都作为工作单位
                    m.COMPANYNAME = row.Replace(" ", "").Trim();
                    row = "";
                }
                //单位地址
                m.COMPANYADDR = row.Replace(" ", "").Trim();
            }
            #endregion

            #region 职业信息部分
            int start2, end2;

            regex = new Regex(@"信息更新日期\s*\n+");
            match = regex.Match(textAll);
            start2 = match.Index + match.Length;

            regex = new Regex(@"\n+.*数据发生机构名称\s*\n+");
            match = regex.Match(textAll);
            end2 = match.Index;

            if (start2 >= end2)
                return list;
            var text2 = "\n" + textAll.Substring(start2, end2 - start2);

            //分割每行，取每行数据
            var rows2 = SplitRowSerial(text2);
            for (int i = 0; i < rows2.Count; i++)
            {
                var row = rows2[i];
                CREDIT_JOBINFO m;
                if (list.Count > i)
                {
                    m = list[i];
                }
                else
                {
                    m = new CREDIT_JOBINFO();
                    list.Add(m);
                }

                //编号
                m.SN = i + 1;
                //信息更新日期
                m.UPDATEDATE = GetUpdateDateFromRow(ref row);
                //年份
                m.INTOYEAR = GetYearFromRow(ref row, PositionInRow.End);
                //职称
                m.PROTITLE = GetUntilSpaceFromRow(ref row, PositionInRow.End);
                //职务
                m.POSITIONNAME = GetUntilSpaceFromRow(ref row, PositionInRow.End);
                //行业
                m.INDUSTRYNAME = GetUntilSpaceFromRow(ref row, PositionInRow.End);
                //职业
                m.JOBNAME = GetUntilSpaceFromRow(ref row, PositionInRow.End);
            }
            #endregion

            return list;
        }

        private static CREDIT_DEBTSTATISTIC AnalysisDebtStatistic(string textAll)
        {
            var m = new CREDIT_DEBTSTATISTIC();

            #region 未结清贷款信息汇总
            int start1, end1;

            var regex = new Regex(@"还款\s*\n+");
            var match = regex.Match(textAll);
            start1 = match.Index + match.Length;

            regex = new Regex(@"\n+.*未销户贷记卡信息汇总");
            match = regex.Match(textAll);
            end1 = match.Index;

            if (start1 < end1)
            {
                var text1 = textAll.Substring(start1, end1 - start1).Trim();

                //取数据
                var text1Arr = text1.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (text1Arr.Length > 3)
                    m.LOANTOTAL = text1Arr[3].XToDecimalOrNull();
                if (text1Arr.Length > 4)
                    m.LOANLEAVE = text1Arr[4].XToDecimalOrNull();
                if (text1Arr.Length > 5)
                    m.LOAN6MONTHAVG = text1Arr[5].XToDecimalOrNull();
            }
            #endregion

            #region 未销户贷记卡信息汇总
            regex = new Regex(@"\n+.+$");//最后一行
            match = regex.Match(textAll.TrimEnd());
            var text2 = match.Value.Trim();

            //取数据
            var text2Arr = text2.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (text2Arr.Length > 3)
                m.CREDITCARDTOTAL = text2Arr[3].XToDecimalOrNull();
            if (text2Arr.Length > 4)
                m.SINGLECARDMAX = text2Arr[4].XToDecimalOrNull();
            if (text2Arr.Length > 5)
                m.SINGLECARDMIN = text2Arr[5].XToDecimalOrNull();
            if (text2Arr.Length > 6)
                m.CREDITCARDUSE = text2Arr[6].XToDecimalOrNull();
            if (text2Arr.Length > 7)
                m.CARD6MONTHAVG = text2Arr[7].XToDecimalOrNull();
            #endregion

            return m;
        }

        private static IList<CREDIT_DAIKUAN_ALL> AnalysisDaikuan(string textAll)
        {
            var list = new List<CREDIT_DAIKUAN_ALL>();
            Regex regex;
            Match match;
            int startat = 0;

            //分割每行，取每笔数据
            var items = SplitRowSerial(textAll, @"\.", false);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i].Replace('，', ',').Replace('。', ',');
                var m = new CREDIT_DAIKUAN_ALL();
                list.Add(m);

                //编号
                m.ORDERNUM = i + 1;

                #region 文字说明部分
                {
                    regex = new Regex(@"\n+\s*账户状态");
                    match = regex.Match(item);
                    var text = item.Substring(0, match.Index > 0 ? match.Index : item.Length).Replace("\n", "").Replace('.', ',').Replace(" ", "").Trim();
                    startat = 0;

                    //贷款发放日
                    regex = new Regex(@"^\d{4}年\d{2}月\d{2}日");
                    match = regex.Match(text);
                    m.BEGINDATE = match.Value.XToDateTimeOrNull();
                    //贷款机构名称
                    regex = new Regex(@"[“""][^“”""]+[”""]");
                    match = regex.Match(text);
                    if (match.Success)
                    {
                        m.ORGANIZENAME = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length;
                    }
                    //贷款合同金额
                    regex = new Regex(@"发放的\d+[,\d]*\d+");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.CONTRACTMONEY = match.Value.Substring(3, match.Value.Length - 3).XToDecimalOrNull();
                        startat = match.Index + match.Length;
                    }
                    //币种
                    regex = new Regex(@"[（(].+?[)）]");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.CURRENCY = match.Value.Substring(1, match.Value.Length - 2).Replace(",", "");
                        startat = match.Index + match.Length - 1;
                    }
                    //贷款种类
                    regex = new Regex(@"[)）][^,]+,");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.DAIKUANTYPE = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length;
                    }
                    //业务号
                    regex = new Regex(@"业务号[^,]+");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.BUSINESSCODE = match.Value.Substring(3, match.Value.Length - 3);
                        startat = match.Index + match.Length;
                    }
                    //担保方式
                    regex = new Regex(@",[^,]*,");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.ASSURETYPE = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length - 1;
                    }
                    //还款期数
                    regex = new Regex(@",[^,]*,");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.BACKTERMS = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length - 1;
                    }
                    //还款频率
                    regex = new Regex(@",[^,]*,");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.BACKCYCLE = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length - 1;
                    }
                    //贷款到期日
                    regex = new Regex(@"\d{4}年\d{2}月\d{2}日到期");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.ENDDATE = match.Value.Substring(0, match.Value.Length - 2).XToDateTimeOrNull();
                        startat = match.Index + match.Length;
                    }
                    //状态截止日
                    regex = new Regex(@"截至\d{4}年\d{2}月\d{2}日");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.ENDDAY = match.Value.Substring(2, match.Value.Length - 2).XToDateTimeOrNull();
                        startat = match.Index + match.Length;
                    }
                    //账户状态
                    regex = new Regex(@"账户状态为[^,]+");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.ACCOUNTSTATE = match.Value.Substring(5, match.Value.Length - 5).Replace("\"", "").Replace("“", "").Replace("”", "");
                    }
                }
                #endregion

                #region 账户状态部分
                regex = new Regex(@"最近一次.*");
                match = regex.Match(item);
                if (match.Success)
                {
                    int start = match.Index + match.Length;
                    regex = new Regex(@"\n未还本金");
                    match = regex.Match(item);
                    int end = match.Index;
                    if (start < end)
                    {
                        //找到数据行，找包含日期(如:2017.12.12)的一行
                        regex = new Regex(@".*\d{4}\.\d{2}.\d{2}.*");
                        match = regex.Match(item, start, end - start);
                        var text = match.Value.Trim();

                        var textArr = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (textArr.Length > 0)
                            m.ACCOUNTSTATE = textArr[0];
                        if (textArr.Length > 1)
                            m.FIVELEVEL = textArr[1];
                        if (textArr.Length > 2)
                            m.BALANCEMONEY = textArr[2].XToDecimalOrNull();
                        if (textArr.Length > 3)
                            m.REMAINTERMS = textArr[3].XToIntOrNull();
                        if (textArr.Length > 4)
                            m.CURRENTMONEY = textArr[4].XToDecimalOrNull();
                        if (textArr.Length > 5)
                            m.BACKDAY = textArr[5];
                        if (textArr.Length > 6)
                            m.FACTMONEY = textArr[6].XToDecimalOrNull();
                        if (textArr.Length > 7)
                            m.LASTBACKDATE = textArr[7];
                    }
                }
                #endregion

                int backIndex, backIndex2;
                regex = new Regex(@".*还款记录");
                match = regex.Match(item);
                backIndex = match.Index;
                backIndex2 = match.Index + match.Length;
                int overdueIndex;
                regex = new Regex(@".*逾期记录");
                match = regex.Match(item);
                overdueIndex = match.Index;

                #region 逾期部分
                regex = new Regex(@"以上未还本金");
                match = regex.Match(item);
                if (match.Success)
                {
                    int start = match.Index + match.Length;
                    int end = backIndex > 0 ? backIndex : (overdueIndex > 0 ? overdueIndex : item.Length);

                    //找到数据行，找只包含数字或空格的一行
                    regex = new Regex(@"\n\s*\d[\d\s]*\n");
                    if (start < end)
                    {
                        match = regex.Match(item, start, end - start);
                        var text = match.Value.Trim();

                        var textArr = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (textArr.Length > 0)
                            m.OVERDUETERMS = textArr[0].XToIntOrNull();
                        if (textArr.Length > 1)
                            m.OVERDUEMONEY = textArr[1].XToDecimalOrNull();
                        if (textArr.Length > 2)
                            m.OVERDUE31 = textArr[2].XToDecimalOrNull();
                        if (textArr.Length > 3)
                            m.OVERDUE61 = textArr[3].XToDecimalOrNull();
                        if (textArr.Length > 4)
                            m.OVERDUE91 = textArr[4].XToDecimalOrNull();
                        if (textArr.Length > 5)
                            m.OVERDUE181 = textArr[5].XToDecimalOrNull();
                    }
                }
                #endregion

                #region 还款记录部分
                if (backIndex > 0)
                {
                    m.Back = new CREDIT_DAIKUAN_BACK();
                    //还款记录标题
                    regex = new Regex(@".*还款记录");
                    match = regex.Match(item);
                    m.Back.BACKTITLE = match.Value.Replace("\n", "");

                    int end = overdueIndex > 0 ? overdueIndex : item.Length;
                    var text = item.Substring(backIndex2, end - backIndex2).Replace("\n", "").Trim();

                    var textArr = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var type = typeof(CREDIT_DAIKUAN_BACK);
                    for (int j = 0; j < textArr.Length; j++)
                    {
                        if (j >= 24) break;
                        var propertyName = "MONTH" + (j + 1).ToString("00");
                        type.GetProperty(propertyName).SetValue(m.Back, textArr[j]);
                    }
                }
                #endregion

                #region 逾期记录部分
                if (overdueIndex > 0)
                {
                    m.Overdues = new List<CREDIT_DAIKUAN_OVERDUE>();
                    regex = new Regex(@"逾期金额\n");
                    match = regex.Match(item, overdueIndex);
                    var text = item.Substring(match.Index + match.Length).Trim('\n');
                    var textArr = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var row in textArr)
                    {
                        var rowArr = row.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (rowArr.Length == 0 || rowArr[0] == IGNORE_SYMBOL)
                            continue;
                        var overdue = new CREDIT_DAIKUAN_OVERDUE();
                        m.Overdues.Add(overdue);
                        overdue.OVERDUEMONTH = rowArr[0];
                        if (rowArr.Length > 1)
                            overdue.OVERDUETERMS = rowArr[1].XToIntOrNull();
                        if (rowArr.Length > 2)
                            overdue.OVERDUEMONEY = rowArr[2].XToDecimalOrNull();

                        if (rowArr.Length < 4 || rowArr[3] == IGNORE_SYMBOL)
                            continue;
                        var overdue2 = new CREDIT_DAIKUAN_OVERDUE();
                        m.Overdues.Add(overdue2);
                        overdue2.OVERDUEMONTH = rowArr[3];
                        if (rowArr.Length > 4)
                            overdue2.OVERDUETERMS = rowArr[4].XToIntOrNull();
                        if (rowArr.Length > 5)
                            overdue2.OVERDUEMONEY = rowArr[5].XToDecimalOrNull();
                    }
                }
                #endregion
            }
            return list;
        }

        private static IList<CREDIT_DAIJIKA_ALL> AnalysisDaijika(string textAll)
        {
            var list = new List<CREDIT_DAIJIKA_ALL>();
            Regex regex;
            Match match;
            int startat = 0;

            //分割每行，取每笔数据
            var items = SplitRowSerial('\n' + textAll, @"\.", false);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i].Replace('，', ',').Replace('。', ',');
                var m = new CREDIT_DAIJIKA_ALL();
                list.Add(m);

                //编号
                m.ORDERNUM = i + 1;

                #region 文字说明部分
                {
                    regex = new Regex(@"\n+\s*账户状态");
                    match = regex.Match(item);
                    var text = item.Substring(0, match.Index > 0 ? match.Index : item.Length).Replace("\n", "").Replace('.', ',').Replace(" ", "").Trim();
                    startat = 0;

                    //发卡日期
                    regex = new Regex(@"^\d{4}年\d{2}月\d{2}日");
                    match = regex.Match(text);
                    m.GRANTDATE = match.Value.XToDateTimeOrNull();
                    //发卡机构
                    regex = new Regex(@"[“‘""][^“”‘’""]+[”’""]");
                    match = regex.Match(text);
                    if (match.Success)
                    {
                        m.ORGANIZENAME = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length;
                    }
                    //币种
                    regex = new Regex(@"[（(].+[)）]");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.CURRENCY = match.Value.Substring(1, match.Value.Length - 2).Replace("账户", "");
                        startat = match.Index + match.Length;
                    }
                    //业务号
                    regex = new Regex(@"业务号[^,]+");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.BUSINESSCODE = match.Value.Substring(3, match.Value.Length - 3);
                        startat = match.Index + match.Length;
                    }
                    //授信额度
                    regex = new Regex(@"(?<=[^\u4E00-\u9FA5]授信额度[^,\d]*)(\d+[\d,]*\d+|\d)");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.GRANTMONEY = match.Value.XToDecimalOrNull();
                        startat = match.Index + match.Length;
                    }
                    //共享授信额度
                    regex = new Regex(@"(?<=共享授信额度[^,\d]*)(\d+[\d,]*\d+|\d)");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.SHAREMONEY = match.Value.XToDecimalOrNull();
                        startat = match.Index + match.Length;
                    }
                    //担保方式
                    regex = new Regex(@",[^,]*,");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.ASSURETYPE = match.Value.Substring(1, match.Value.Length - 2);
                        startat = match.Index + match.Length - 1;
                    }
                    //账户状态
                    regex = new Regex(@"账户状态为[^,]+");
                    match = regex.Match(text, startat);
                    if (match.Success)
                    {
                        m.ACCOUNTSTATE = match.Value.Substring(5, match.Value.Length - 5).Replace("\"", "").Replace("“", "").Replace("”", "").Replace("‘", "").Replace("’", "");
                    }
                }
                #endregion

                #region 账户状态部分
                regex = new Regex(@"本月应还款");
                match = regex.Match(item);
                if (match.Success)
                {
                    int start = match.Index + match.Length;
                    regex = new Regex(@"账单日");
                    match = regex.Match(item);
                    int end = match.Index;

                    if (start < end)
                    {
                        var text = item.Substring(start, end - start).Replace("\n", "").Trim();

                        var textArr = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (textArr.Length > 0)
                            m.ACCOUNTSTATE = textArr[0];
                        if (textArr.Length > 1)
                            m.USEDMONEY = textArr[1].XToDecimalOrNull();
                        if (textArr.Length > 2)
                            m.AVGMONEY = textArr[2].XToDecimalOrNull();
                        if (textArr.Length > 3)
                            m.MAXMONEY = textArr[3].XToDecimalOrNull();
                        if (textArr.Length > 4)
                            m.BACKMONEY = textArr[4].XToDecimalOrNull();
                    }
                }
                #endregion

                int backIndex, backIndex2;
                regex = new Regex(@".*还款记录");
                match = regex.Match(item);
                backIndex = match.Index;
                backIndex2 = match.Index + match.Length;
                int overdueIndex;
                regex = new Regex(@".*逾期记录");
                match = regex.Match(item);
                overdueIndex = match.Index;

                #region 逾期部分
                regex = new Regex(@"当前逾期金额");
                match = regex.Match(item);
                if (match.Success)
                {
                    int start = match.Index + match.Length;
                    int end = backIndex > 0 ? backIndex : (overdueIndex > 0 ? overdueIndex : item.Length);
                    if (start < end)
                    {
                        var text = item.Substring(start, end - start).Replace("\n", "").Trim();

                        var textArr = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (textArr.Length > 0)
                            m.BILLDATE = textArr[0].XToDateTimeOrNull();
                        if (textArr.Length > 1)
                            m.FACTMONEY = textArr[1].XToDecimalOrNull();
                        if (textArr.Length > 2)
                            m.LASTBACKDATE = textArr[2].XToDateTimeOrNull();
                        if (textArr.Length > 3)
                            m.OVERDUETIMES = textArr[3].XToDecimalOrNull();
                        if (textArr.Length > 4)
                            m.OVERDUEMONEY = textArr[4].XToDecimalOrNull();
                    }
                }
                #endregion

                #region 还款记录部分
                if (backIndex > 0)
                {
                    m.Back = new CREDIT_DAIJIKA_BACK();
                    //还款记录标题
                    regex = new Regex(@".*还款记录");
                    match = regex.Match(item);
                    m.Back.BACKTITLE = match.Value.Replace("\n", "");

                    int end = overdueIndex > 0 ? overdueIndex : item.Length;
                    var text = item.Substring(backIndex2, end - backIndex2).Replace("\n", "").Trim();

                    var textArr = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var type = typeof(CREDIT_DAIJIKA_BACK);
                    for (int j = 0; j < textArr.Length; j++)
                    {
                        if (j >= 24) break;
                        var propertyName = "MONTH" + (j + 1).ToString("00");
                        type.GetProperty(propertyName).SetValue(m.Back, textArr[j]);
                    }
                }
                #endregion

                #region 逾期记录部分
                if (overdueIndex > 0)
                {
                    m.Overdues = new List<CREDIT_DAIJIKA_OVERDUE>();
                    regex = new Regex(@"逾期金额\n");
                    match = regex.Match(item, overdueIndex);
                    var text = item.Substring(match.Index + match.Length).Trim('\n');
                    var textArr = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var row in textArr)
                    {
                        var rowArr = row.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (rowArr.Length == 0 || rowArr[0] == IGNORE_SYMBOL)
                            continue;
                        var overdue = new CREDIT_DAIJIKA_OVERDUE();
                        m.Overdues.Add(overdue);
                        overdue.OVERDUEMONTH = rowArr[0];
                        if (rowArr.Length > 1)
                            overdue.OVERDUETERMS = rowArr[1].XToIntOrNull();
                        if (rowArr.Length > 2)
                            overdue.OVERDUEMONEY = rowArr[2].XToDecimalOrNull();

                        if (rowArr.Length < 4 || rowArr[3] == IGNORE_SYMBOL)
                            continue;
                        var overdue2 = new CREDIT_DAIJIKA_OVERDUE();
                        m.Overdues.Add(overdue2);
                        overdue2.OVERDUEMONTH = rowArr[3];
                        if (rowArr.Length > 4)
                            overdue2.OVERDUETERMS = rowArr[4].XToIntOrNull();
                        if (rowArr.Length > 5)
                            overdue2.OVERDUEMONEY = rowArr[5].XToDecimalOrNull();
                    }
                }
                #endregion
            }
            return list;
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
            var r = RhPDFTextParser.Default.ParseText(text);
            return r;
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

    enum PositionInRow
    {
        Start = 0,
        Middle = 1,
        End = 2,
    }
}
