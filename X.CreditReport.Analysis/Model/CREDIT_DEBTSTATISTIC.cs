/******************************************
* 模块名称：实体 征信报告贷款负债统计信息记录表
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
	/// 实体 征信报告贷款负债统计信息记录表
	/// </summary>
	[Description("Primary:GUID")]
    [Serializable]
	public class CREDIT_DEBTSTATISTIC
	{
        #region 构造函数
        /// <summary>
        /// 实体 征信报告贷款负债统计信息记录表
        /// </summary>
        public CREDIT_DEBTSTATISTIC(){}
        #endregion

        #region 私有变量
        private string _guid = null;
        private string _loanguid = null;
        private string _reportguid = null;
        private decimal? _loantotal;
        private decimal? _loanleave;
        private decimal? _loan6monthavg;
        private decimal? _creditcardtotal;
        private decimal? _singlecardmax;
        private decimal? _singlecardmin;
        private decimal? _creditcarduse;
        private decimal? _card6monthavg;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键GUID(NOT NULL)
        /// </summary>
        public string GUID
        {
            set{ _guid=value;}
            get{return _guid;}
        }
        /// <summary>
        /// 对应贷款GUID
        /// </summary>
        public string LOANGUID
        {
            set{ _loanguid=value;}
            get{return _loanguid;}
        }
        /// <summary>
        /// 对应征信报告GUID
        /// </summary>
        public string REPORTGUID
        {
            set{ _reportguid=value;}
            get{return _reportguid;}
        }
        /// <summary>
        /// 贷款合同总额
        /// </summary>
        public decimal? LOANTOTAL
        {
            set{ _loantotal=value;}
            get{return _loantotal;}
        }
        /// <summary>
        /// 贷款余额
        /// </summary>
        public decimal? LOANLEAVE
        {
            set{ _loanleave=value;}
            get{return _loanleave;}
        }
        /// <summary>
        /// 最近6个月贷款平均应还款
        /// </summary>
        public decimal? LOAN6MONTHAVG
        {
            set{ _loan6monthavg=value;}
            get{return _loan6monthavg;}
        }
        /// <summary>
        /// 贷记卡授信总额
        /// </summary>
        public decimal? CREDITCARDTOTAL
        {
            set{ _creditcardtotal=value;}
            get{return _creditcardtotal;}
        }
        /// <summary>
        /// 单卡最高额度
        /// </summary>
        public decimal? SINGLECARDMAX
        {
            set{ _singlecardmax=value;}
            get{return _singlecardmax;}
        }
        /// <summary>
        /// 单卡最低额度
        /// </summary>
        public decimal? SINGLECARDMIN
        {
            set{ _singlecardmin=value;}
            get{return _singlecardmin;}
        }
        /// <summary>
        /// 已使用额度
        /// </summary>
        public decimal? CREDITCARDUSE
        {
            set{ _creditcarduse=value;}
            get{return _creditcarduse;}
        }
        /// <summary>
        /// 最近6个月平均使用额度
        /// </summary>
        public decimal? CARD6MONTHAVG
        {
            set{ _card6monthavg=value;}
            get{return _card6monthavg;}
        }
        #endregion
	}
}
