/******************************************
* 模块名称：实体 信贷交易信息明细-贷记卡
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
    /// 实体 信贷交易信息明细-贷记卡
    /// </summary>
    [Description("Primary:DAIJIKAGUID")]
    [Serializable]
    public class CREDIT_DAIJIKA
    {
        #region 构造函数
        /// <summary>
        /// 实体 信贷交易信息明细-贷记卡
        /// </summary>
        public CREDIT_DAIJIKA() { }
        #endregion

        #region 私有变量
        private string _daijikaguid = null;
        private string _loanguid = null;
        private string _reportguid = null;
        private string _businesscode = null;
        private string _organizename = null;
        private string _currency = null;
        private DateTime? _grantdate;
        private decimal? _grantmoney;
        private string _assuretype = null;
        private string _accountstate = null;
        private decimal? _sharemoney;
        private decimal? _usedmoney;
        private decimal? _avgmoney;
        private decimal? _maxmoney;
        private DateTime? _billdate;
        private decimal? _backmoney;
        private decimal? _factmoney;
        private DateTime? _lastbackdate;
        private decimal? _overduetimes;
        private decimal? _overduemoney;
        private Int32? _ordernum;
        private DateTime? _createdate;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键，贷记卡GUID(NOT NULL)
        /// </summary>
        public string DAIJIKAGUID
        {
            set { _daijikaguid = value; }
            get { return _daijikaguid; }
        }
        /// <summary>
        /// 贷款GUID --兼容为VARCHAR2
        /// </summary>
        public string LOANGUID
        {
            set { _loanguid = value; }
            get { return _loanguid; }
        }
        /// <summary>
        /// 征信报告GUID
        /// </summary>
        public string REPORTGUID
        {
            set { _reportguid = value; }
            get { return _reportguid; }
        }
        /// <summary>
        /// 业务号
        /// </summary>
        public string BUSINESSCODE
        {
            set { _businesscode = value; }
            get { return _businesscode; }
        }
        /// <summary>
        /// 发卡机构
        /// </summary>
        public string ORGANIZENAME
        {
            set { _organizename = value; }
            get { return _organizename; }
        }
        /// <summary>
        /// 币种
        /// </summary>
        public string CURRENCY
        {
            set { _currency = value; }
            get { return _currency; }
        }
        /// <summary>
        /// 发卡日期
        /// </summary>
        public DateTime? GRANTDATE
        {
            set { _grantdate = value; }
            get { return _grantdate; }
        }
        /// <summary>
        /// 授信额度
        /// </summary>
        public decimal? GRANTMONEY
        {
            set { _grantmoney = value; }
            get { return _grantmoney; }
        }
        /// <summary>
        /// 担保方式
        /// </summary>
        public string ASSURETYPE
        {
            set { _assuretype = value; }
            get { return _assuretype; }
        }
        /// <summary>
        /// 账户状态
        /// </summary>
        public string ACCOUNTSTATE
        {
            set { _accountstate = value; }
            get { return _accountstate; }
        }
        /// <summary>
        /// 共享额度
        /// </summary>
        public decimal? SHAREMONEY
        {
            set { _sharemoney = value; }
            get { return _sharemoney; }
        }
        /// <summary>
        /// 已用额度
        /// </summary>
        public decimal? USEDMONEY
        {
            set { _usedmoney = value; }
            get { return _usedmoney; }
        }
        /// <summary>
        /// 最近6个月平均使用额度
        /// </summary>
        public decimal? AVGMONEY
        {
            set { _avgmoney = value; }
            get { return _avgmoney; }
        }
        /// <summary>
        /// 最大使用额度
        /// </summary>
        public decimal? MAXMONEY
        {
            set { _maxmoney = value; }
            get { return _maxmoney; }
        }
        /// <summary>
        /// 账单日
        /// </summary>
        public DateTime? BILLDATE
        {
            set { _billdate = value; }
            get { return _billdate; }
        }
        /// <summary>
        /// 本月应还款
        /// </summary>
        public decimal? BACKMONEY
        {
            set { _backmoney = value; }
            get { return _backmoney; }
        }
        /// <summary>
        /// 本月实还款
        /// </summary>
        public decimal? FACTMONEY
        {
            set { _factmoney = value; }
            get { return _factmoney; }
        }
        /// <summary>
        /// 最近一次还款日期
        /// </summary>
        public DateTime? LASTBACKDATE
        {
            set { _lastbackdate = value; }
            get { return _lastbackdate; }
        }
        /// <summary>
        /// 当前逾期期数
        /// </summary>
        public decimal? OVERDUETIMES
        {
            set { _overduetimes = value; }
            get { return _overduetimes; }
        }
        /// <summary>
        /// 当前逾期金额
        /// </summary>
        public decimal? OVERDUEMONEY
        {
            set { _overduemoney = value; }
            get { return _overduemoney; }
        }
        /// <summary>
        /// 排序序号
        /// </summary>
        public Int32? ORDERNUM
        {
            set { _ordernum = value; }
            get { return _ordernum; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CREATEDATE
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        #endregion
    }
}
