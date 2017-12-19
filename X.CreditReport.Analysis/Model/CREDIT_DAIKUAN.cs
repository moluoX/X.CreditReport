/******************************************
* 模块名称：实体 信贷交易明细_贷款
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
	/// 实体 信贷交易明细_贷款
	/// </summary>
	[Description("Primary:DAIKUANGUID")]
    [Serializable]
	public class CREDIT_DAIKUAN
	{
        #region 构造函数
        /// <summary>
        /// 实体 信贷交易明细_贷款
        /// </summary>
        public CREDIT_DAIKUAN(){}
        #endregion

        #region 私有变量
        private string _daikuanguid = null;
        private string _loanguid = null;
        private string _reportguid = null;
        private string _daikuantype = null;
        private string _businesscode = null;
        private string _organizename = null;
        private string _assuretype = null;
        private string _currency = null;
        private string _accountstate = null;
        private string _backcycle = null;
        private string _backterms = null;
        private DateTime? _begindate;
        private DateTime? _enddate;
        private decimal? _contractmoney;
        private DateTime? _endday;
        private string _endmonth = null;
        private string _fivelevel = null;
        private decimal? _balancemoney;
        private Int32? _remainterms;
        private decimal? _currentmoney;
        private string _backday = null;
        private decimal? _factmoney;
        private string _lastbackdate = null;
        private Int32? _overdueterms;
        private decimal? _overduemoney;
        private decimal? _overdue31;
        private decimal? _overdue61;
        private decimal? _overdue91;
        private decimal? _overdue181;
        private Int32? _ordernum;
        private DateTime? _createdate;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键，贷款记录GUID(NOT NULL)
        /// </summary>
        public string DAIKUANGUID
        {
            set{ _daikuanguid=value;}
            get{return _daikuanguid;}
        }
        /// <summary>
        /// 贷款GUID  _  兼容为nchar
        /// </summary>
        public string LOANGUID
        {
            set{ _loanguid=value;}
            get{return _loanguid;}
        }
        /// <summary>
        /// 征信报告GUID
        /// </summary>
        public string REPORTGUID
        {
            set{ _reportguid=value;}
            get{return _reportguid;}
        }
        /// <summary>
        /// 贷款种类
        /// </summary>
        public string DAIKUANTYPE
        {
            set{ _daikuantype=value;}
            get{return _daikuantype;}
        }
        /// <summary>
        /// 业务号
        /// </summary>
        public string BUSINESSCODE
        {
            set{ _businesscode=value;}
            get{return _businesscode;}
        }
        /// <summary>
        /// 贷款机构名称
        /// </summary>
        public string ORGANIZENAME
        {
            set{ _organizename=value;}
            get{return _organizename;}
        }
        /// <summary>
        /// 担保方式
        /// </summary>
        public string ASSURETYPE
        {
            set{ _assuretype=value;}
            get{return _assuretype;}
        }
        /// <summary>
        /// 币种
        /// </summary>
        public string CURRENCY
        {
            set{ _currency=value;}
            get{return _currency;}
        }
        /// <summary>
        /// 账户状态
        /// </summary>
        public string ACCOUNTSTATE
        {
            set{ _accountstate=value;}
            get{return _accountstate;}
        }
        /// <summary>
        /// 还款频率
        /// </summary>
        public string BACKCYCLE
        {
            set{ _backcycle=value;}
            get{return _backcycle;}
        }
        /// <summary>
        /// 还款期数
        /// </summary>
        public string BACKTERMS
        {
            set{ _backterms=value;}
            get{return _backterms;}
        }
        /// <summary>
        /// 贷款发放日
        /// </summary>
        public DateTime? BEGINDATE
        {
            set{ _begindate=value;}
            get{return _begindate;}
        }
        /// <summary>
        /// 贷款到期日
        /// </summary>
        public DateTime? ENDDATE
        {
            set{ _enddate=value;}
            get{return _enddate;}
        }
        /// <summary>
        /// 贷款合同金额
        /// </summary>
        public decimal? CONTRACTMONEY
        {
            set{ _contractmoney=value;}
            get{return _contractmoney;}
        }
        /// <summary>
        /// 状态截止日
        /// </summary>
        public DateTime? ENDDAY
        {
            set{ _endday=value;}
            get{return _endday;}
        }
        /// <summary>
        /// 状态截止月/转出月
        /// </summary>
        public string ENDMONTH
        {
            set{ _endmonth=value;}
            get{return _endmonth;}
        }
        /// <summary>
        /// 五级分类
        /// </summary>
        public string FIVELEVEL
        {
            set{ _fivelevel=value;}
            get{return _fivelevel;}
        }
        /// <summary>
        /// 本金余额
        /// </summary>
        public decimal? BALANCEMONEY
        {
            set{ _balancemoney=value;}
            get{return _balancemoney;}
        }
        /// <summary>
        /// 剩余还款期数
        /// </summary>
        public Int32? REMAINTERMS
        {
            set{ _remainterms=value;}
            get{return _remainterms;}
        }
        /// <summary>
        /// 本月应还款
        /// </summary>
        public decimal? CURRENTMONEY
        {
            set{ _currentmoney=value;}
            get{return _currentmoney;}
        }
        /// <summary>
        /// 应还款日
        /// </summary>
        public string BACKDAY
        {
            set{ _backday=value;}
            get{return _backday;}
        }
        /// <summary>
        /// 本月实还款
        /// </summary>
        public decimal? FACTMONEY
        {
            set{ _factmoney=value;}
            get{return _factmoney;}
        }
        /// <summary>
        /// 最近一次还款日
        /// </summary>
        public string LASTBACKDATE
        {
            set{ _lastbackdate=value;}
            get{return _lastbackdate;}
        }
        /// <summary>
        /// 当前逾期期数
        /// </summary>
        public Int32? OVERDUETERMS
        {
            set{ _overdueterms=value;}
            get{return _overdueterms;}
        }
        /// <summary>
        /// 当前逾期金额
        /// </summary>
        public decimal? OVERDUEMONEY
        {
            set{ _overduemoney=value;}
            get{return _overduemoney;}
        }
        /// <summary>
        /// 逾期31~60天未还本金
        /// </summary>
        public decimal? OVERDUE31
        {
            set{ _overdue31=value;}
            get{return _overdue31;}
        }
        /// <summary>
        /// 逾期61~90天未还本金
        /// </summary>
        public decimal? OVERDUE61
        {
            set{ _overdue61=value;}
            get{return _overdue61;}
        }
        /// <summary>
        /// 逾期91~180天未还本金
        /// </summary>
        public decimal? OVERDUE91
        {
            set{ _overdue91=value;}
            get{return _overdue91;}
        }
        /// <summary>
        /// 逾期180天以上未还本金
        /// </summary>
        public decimal? OVERDUE181
        {
            set{ _overdue181=value;}
            get{return _overdue181;}
        }
        /// <summary>
        /// 排序序号
        /// </summary>
        public Int32? ORDERNUM
        {
            set{ _ordernum=value;}
            get{return _ordernum;}
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
