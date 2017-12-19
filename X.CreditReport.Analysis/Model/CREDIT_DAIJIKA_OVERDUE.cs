/******************************************
* 模块名称：实体 贷记卡记录逾期记录明细
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
	/// 实体 贷记卡记录逾期记录明细
	/// </summary>
	[Description("Primary:OVERDUEGUID")]
    [Serializable]
	public class CREDIT_DAIJIKA_OVERDUE
	{
        #region 构造函数
        /// <summary>
        /// 实体 贷记卡记录逾期记录明细
        /// </summary>
        public CREDIT_DAIJIKA_OVERDUE(){}
        #endregion

        #region 私有变量
        private string _overdueguid = null;
        private string _daijikaguid = null;
        private string _overduemonth = null;
        private Int32? _overdueterms;
        private decimal? _overduemoney;
        private DateTime? _createdate;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键，逾期记录GUID(NOT NULL)
        /// </summary>
        public string OVERDUEGUID
        {
            set{ _overdueguid=value;}
            get{return _overdueguid;}
        }
        /// <summary>
        /// 贷记卡记录GUID
        /// </summary>
        public string DAIJIKAGUID
        {
            set{ _daijikaguid=value;}
            get{return _daijikaguid;}
        }
        /// <summary>
        /// 逾期月份
        /// </summary>
        public string OVERDUEMONTH
        {
            set{ _overduemonth=value;}
            get{return _overduemonth;}
        }
        /// <summary>
        /// 逾期持续月数
        /// </summary>
        public Int32? OVERDUETERMS
        {
            set{ _overdueterms=value;}
            get{return _overdueterms;}
        }
        /// <summary>
        /// 逾期金额
        /// </summary>
        public decimal? OVERDUEMONEY
        {
            set{ _overduemoney=value;}
            get{return _overduemoney;}
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
