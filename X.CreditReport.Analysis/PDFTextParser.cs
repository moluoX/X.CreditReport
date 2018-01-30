using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace X.CreditReport.Analysis
{
    /// <summary>
    /// 注意：把词汇库文件dic.txt和dic_ext.txt放到程序根目录
    ///       添加需要处理的生词请手动添加到dic_ext.txt程序自动检测该文件，
    ///       检测到有新词更新，会自动重新加载词库，无需重启系统
    ///       
    /// 调用方式：
    /// string result = PDFTextParser.Default.ParseText(text);
    /// </summary>
    internal class PDFTextParser
    {
        private PDFTextParser() { }
        private static PDFTextParser _Default;
        public static PDFTextParser Default
        {
            get
            {
                if (_Default == null)
                {
                    _Default = new PDFTextParser();
                    _Default.LoadWordDic();
                }
                return _Default;
            }
        }
        public List<string> PartenList
        {
            get
            {
                return partenList;
            }
        }
        List<string> partenList = new List<string>();
        string dicFile = @"config\dic.txt";
        string dicExtFile = @"config\dic_ext.txt";
        string dicErrFile = @"config\dic_err.txt";
        string trimFile = @"config\regex_trim.txt";
        string deleteFile = @"config\regex_delete.txt";
        string strPatern = @"[\s]*";

        /// <summary>
        /// 处理人行PDF文件内容
        /// </summary>
        /// <param name="sText">内容字符串</param>
        /// <returns></returns>
        public string ParseText(string sText)
        {
            this.CheckDicUpdate();
            string parseText = Match4DateString(sText);
            parseText = procErr(parseText);
            foreach (string filter in partenList)
                parseText = PatternTrim(parseText, filter);

            return parseText;
        }
        DateTime lastUpdateTimeExt = DateTime.Now;
        DateTime lastUpdateTimeErr = DateTime.Now;
        DateTime lastUpdateTimeDel = DateTime.Now;
        DateTime lastUpdateTimeTrim = DateTime.Now;
        private void CheckDicUpdate()
        {

            FileInfo fi = new FileInfo(dicExtFile);
            int compareResult = fi.LastWriteTime.CompareTo(lastUpdateTimeExt);
            if (compareResult > 0)
            {
                lastUpdateTimeExt = fi.LastWriteTime;
                LoadWordDic();
                return;
            }
            fi = new FileInfo(dicErrFile);
            compareResult = fi.LastWriteTime.CompareTo(lastUpdateTimeErr);
            if (compareResult > 0)
            {
                lastUpdateTimeErr = fi.LastWriteTime;
                LoadWordDic();
                return;
            }
            fi = new FileInfo(deleteFile);
            compareResult = fi.LastWriteTime.CompareTo(lastUpdateTimeDel);
            if (compareResult > 0)
            {
                lastUpdateTimeDel = fi.LastWriteTime;
                LoadWordDic();
                return;
            }
            fi = new FileInfo(trimFile);
            compareResult = fi.LastWriteTime.CompareTo(lastUpdateTimeTrim);
            if (compareResult > 0)
            {
                lastUpdateTimeTrim = fi.LastWriteTime;
                LoadWordDic();
                return;
            }
        }
        List<string> trimList = new List<string>();
        List<string> deletePaternList = new List<string>();
        public string Match4DateString(string pdfParseText)
        {
            string text = pdfParseText;
            text = text.Replace("\r", "");
            //text = text.Replace("\n", " ");

            for (int i = 0; i < trimList.Count; i++)
            {
                text = PatternTrim(text, trimList[i]);
            }
            for (int idx = 0; idx < deletePaternList.Count; idx++)
            {
                text = PatternDelete(text, deletePaternList[idx]);
            }

            string patM = "\\d{4}年[\\d ]{1,4}月[\\d ]{1,4}[^\\d ]";
            string ri = "日";
            Regex rM = new Regex(patM, RegexOptions.IgnoreCase);
            Match mM = rM.Match(text);
            while (mM.Success)
            {
                string source = mM.Value;
                string target = source.Substring(0, source.Length - 1) + ri;
                text = text.Replace(source, target);
                mM = mM.NextMatch();
            }
            List<string> regexList = new List<string>();
            regexList.Add("[iN]{3,}");
            regexList.Add("[\\*/]{3,}");
            //regexList.Add("[/]{2,}");
            Dictionary<string, string> PaternResult = new Dictionary<string, string>();

            for (int ii = 0; ii < regexList.Count; ii++)
            {
                patM = regexList[ii];// "[\\*iN/]{3,}";//[N\\*/]+[1]*
                rM = new Regex(patM, RegexOptions.IgnoreCase);
                mM = rM.Match(text);
                while (mM.Success)
                {
                    string source = mM.Value;
                    string tmpChar = " ";
                    string target = source[0] + tmpChar;
                    for (int i = 1; i < source.Length - 1; i++)
                    {
                        target += source[i] + tmpChar;
                    }
                    target += source[source.Length - 1] + "";
                    if (!PaternResult.ContainsKey(source))
                    {
                        PaternResult.Add(source, target);
                    }
                    //text = text.Replace(source, target);
                    mM = mM.NextMatch();
                }
            }
            Dictionary<string, string> PaternResultSort = PaternResult.OrderByDescending(p => p.Key.Length).ToDictionary(p => p.Key, o => o.Value);
            foreach (string key in PaternResultSort.Keys)
            {
                text = text.Replace(key, PaternResultSort[key]);
            }
            return text;
        }
        private string PatternDelete(string pdfParseText, string patern)
        {
            string text = pdfParseText;
            Regex rM = new Regex(patern, RegexOptions.IgnoreCase);
            Match mM = rM.Match(text);
            while (mM.Success)
            {
                text = text.Replace(mM.Value, "");
                mM = mM.NextMatch();
            }
            return text;
        }
        private static List<string> getHanzi(string str)
        {
            string patern = "[\u4E00-\u9FA5]{2,}";
            string text = str;
            Regex rM = new Regex(patern, RegexOptions.IgnoreCase);
            Match mM = rM.Match(text);
            List<string> result = new List<string>();
            while (mM.Success)
            {
                if (mM.Value.Trim() != "")
                {
                    result.Add(mM.Value);
                }
                mM = mM.NextMatch();
            }
            return result;
        }
        public static bool HasHanzi(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        private string procErr(string str)
        {
            string tempStr = str;
            foreach (string errKey in ErrorDic.Keys)
            {
                tempStr = tempStr.Replace(errKey, ErrorDic[errKey]);
            }
            return tempStr;
        }
        private string PatternTrim(string pdfParseText, string patern)
        {
            if (patern == "") return pdfParseText;
            string text = pdfParseText;
            Regex rM = new Regex(patern, RegexOptions.IgnoreCase);
            Match mM = rM.Match(text);
            while (mM.Success)
            {
                string tmpStr = mM.Value.Replace(" ", "").Replace("\n", "");
                text = text.Replace(mM.Value, tmpStr);
                mM = mM.NextMatch();
            }
            return text;
        }
        Dictionary<string, string> ErrorDic = new Dictionary<string, string>();
        private void LoadWordDic()
        {
            partenList.Clear();
            string[] strs = File.ReadAllLines(dicFile, Encoding.UTF8);
            foreach (string key in strs)
            {
                string filtler = "";
                for (int idx = 0; idx < key.Length - 1; idx++)
                    filtler += key[idx] + strPatern;
                filtler += key[key.Length - 1];
                partenList.Add(filtler);
            }
            strs = File.ReadAllLines(dicExtFile, Encoding.UTF8);
            foreach (string key in strs)
            {
                string filtler = "";
                for (int idx = 0; idx < key.Length - 1; idx++)
                    filtler += key[idx] + strPatern;
                filtler += key[key.Length - 1];
                partenList.Add(filtler);
            }
            string[] errors = File.ReadAllLines(dicErrFile, Encoding.UTF8);
            foreach (string ekey in errors)
            {
                if (ekey.Trim() == "" || !ekey.Contains("=")) continue;

                string[] ci = ekey.Split(new char[] { '=' });
                if (ErrorDic.ContainsKey(ci[0])) continue;

                ErrorDic.Add(ci[0], ci[1]);
            }
            string[] deletes = File.ReadAllLines(deleteFile, Encoding.UTF8);
            foreach (string ekey in deletes)
            {
                if (ekey.Trim() == "") continue;

                this.deletePaternList.Add(ekey);
            }
            string[] trims = File.ReadAllLines(trimFile, Encoding.UTF8);
            foreach (string ekey in trims)
            {
                if (ekey.Trim() == "") continue;

                this.trimList.Add(ekey);
            }
        }

        public static List<string> splitValueText(string v)
        {
            List<string> result = new List<string>();
            string str = v;
            str = str.Replace(".", "1");
            str = str.Replace("\"", "1");
            str = str.Replace("'", "1");
            str = str.Replace("{", "1");
            str = str.Replace("(", "1");
            str = str.Replace(")", "1");
            str = str.Replace("-", "1");
            str = str.Replace("?", "1");
            str = str.Replace("*", "1");
            str = str.Replace("/", "1");
            str = str.Replace("\\", "1");
            List<string> strList = getHanzi(str);

            foreach (string tmpStr in strList)
            {
                if (tmpStr.Length >= 2) result.Add(tmpStr);
            }
            return result;
        }
    }
}
