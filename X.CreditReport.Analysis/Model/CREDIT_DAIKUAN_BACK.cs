/******************************************
* 模块名称：实体 贷款记录对应还款记录明细
* 当前版本：1.0
* 开发人员：Administrator
* 生成时间：2017/12/14
* 版本历史：此代码由 VB/C#.Net实体代码生成工具(EntitysCodeGenerate 4.5) 自动生成。
* 
******************************************/
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace X.CreditReport.Analysis.Model
{
	/// <summary>
	/// 实体 贷款记录对应还款记录明细
	/// </summary>
	[Description("Primary:BACKGUID")]
    [Serializable]
	public class CREDIT_DAIKUAN_BACK
	{
        #region 构造函数
        /// <summary>
        /// 实体 贷款记录对应还款记录明细
        /// </summary>
        public CREDIT_DAIKUAN_BACK(){}
        #endregion

        #region 私有变量
        private string _backguid = null;
        private string _daikuanguid = null;
        private string _backtitle = null;
        private string _month01 = null;
        private string _month02 = null;
        private string _month03 = null;
        private string _month04 = null;
        private string _month05 = null;
        private string _month06 = null;
        private string _month07 = null;
        private string _month08 = null;
        private string _month09 = null;
        private string _month10 = null;
        private string _month11 = null;
        private string _month12 = null;
        private string _month13 = null;
        private string _month14 = null;
        private string _month15 = null;
        private string _month16 = null;
        private string _month17 = null;
        private string _month18 = null;
        private string _month19 = null;
        private string _month20 = null;
        private string _month21 = null;
        private string _month22 = null;
        private string _month23 = null;
        private string _month24 = null;
        private DateTime? _createdate;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键，还款记录GUID(NOT NULL)
        /// </summary>
        public string BACKGUID
        {
            set{ _backguid=value;}
            get{return _backguid;}
        }
        /// <summary>
        /// 贷款记录GUID
        /// </summary>
        public string DAIKUANGUID
        {
            set{ _daikuanguid=value;}
            get{return _daikuanguid;}
        }
        /// <summary>
        /// 还款记录标题
        /// </summary>
        public string BACKTITLE
        {
            set{ _backtitle=value;}
            get{return _backtitle;}
        }
        /// <summary>
        /// MONTH01
        /// </summary>
        public string MONTH01
        {
            set{ _month01=value;}
            get{return _month01;}
        }
        /// <summary>
        /// MONTH02
        /// </summary>
        public string MONTH02
        {
            set{ _month02=value;}
            get{return _month02;}
        }
        /// <summary>
        /// MONTH03
        /// </summary>
        public string MONTH03
        {
            set{ _month03=value;}
            get{return _month03;}
        }
        /// <summary>
        /// MONTH04
        /// </summary>
        public string MONTH04
        {
            set{ _month04=value;}
            get{return _month04;}
        }
        /// <summary>
        /// MONTH05
        /// </summary>
        public string MONTH05
        {
            set{ _month05=value;}
            get{return _month05;}
        }
        /// <summary>
        /// MONTH06
        /// </summary>
        public string MONTH06
        {
            set{ _month06=value;}
            get{return _month06;}
        }
        /// <summary>
        /// MONTH07
        /// </summary>
        public string MONTH07
        {
            set{ _month07=value;}
            get{return _month07;}
        }
        /// <summary>
        /// MONTH08
        /// </summary>
        public string MONTH08
        {
            set{ _month08=value;}
            get{return _month08;}
        }
        /// <summary>
        /// MONTH09
        /// </summary>
        public string MONTH09
        {
            set{ _month09=value;}
            get{return _month09;}
        }
        /// <summary>
        /// MONTH10
        /// </summary>
        public string MONTH10
        {
            set{ _month10=value;}
            get{return _month10;}
        }
        /// <summary>
        /// MONTH11
        /// </summary>
        public string MONTH11
        {
            set{ _month11=value;}
            get{return _month11;}
        }
        /// <summary>
        /// MONTH12
        /// </summary>
        public string MONTH12
        {
            set{ _month12=value;}
            get{return _month12;}
        }
        /// <summary>
        /// MONTH13
        /// </summary>
        public string MONTH13
        {
            set{ _month13=value;}
            get{return _month13;}
        }
        /// <summary>
        /// MONTH14
        /// </summary>
        public string MONTH14
        {
            set{ _month14=value;}
            get{return _month14;}
        }
        /// <summary>
        /// MONTH15
        /// </summary>
        public string MONTH15
        {
            set{ _month15=value;}
            get{return _month15;}
        }
        /// <summary>
        /// MONTH16
        /// </summary>
        public string MONTH16
        {
            set{ _month16=value;}
            get{return _month16;}
        }
        /// <summary>
        /// MONTH17
        /// </summary>
        public string MONTH17
        {
            set{ _month17=value;}
            get{return _month17;}
        }
        /// <summary>
        /// MONTH18
        /// </summary>
        public string MONTH18
        {
            set{ _month18=value;}
            get{return _month18;}
        }
        /// <summary>
        /// MONTH19
        /// </summary>
        public string MONTH19
        {
            set{ _month19=value;}
            get{return _month19;}
        }
        /// <summary>
        /// MONTH20
        /// </summary>
        public string MONTH20
        {
            set{ _month20=value;}
            get{return _month20;}
        }
        /// <summary>
        /// MONTH21
        /// </summary>
        public string MONTH21
        {
            set{ _month21=value;}
            get{return _month21;}
        }
        /// <summary>
        /// MONTH22
        /// </summary>
        public string MONTH22
        {
            set{ _month22=value;}
            get{return _month22;}
        }
        /// <summary>
        /// MONTH23
        /// </summary>
        public string MONTH23
        {
            set{ _month23=value;}
            get{return _month23;}
        }
        /// <summary>
        /// MONTH24
        /// </summary>
        public string MONTH24
        {
            set{ _month24=value;}
            get{return _month24;}
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CREATEDATE
        {
            set{ _createdate=value;}
            get{return _createdate;}
        }
        #endregion
	}
}
