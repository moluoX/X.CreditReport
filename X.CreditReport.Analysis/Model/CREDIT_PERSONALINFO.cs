/******************************************
* 模块名称：实体 征信报告个人基本信息记录表
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
    /// 实体 征信报告个人基本信息记录表
    /// </summary>
    [Description("Primary:GUID")]
    [Serializable]
    public class CREDIT_PERSONALINFO
    {
        #region 构造函数
        /// <summary>
        /// 实体 征信报告个人基本信息记录表
        /// </summary>
        public CREDIT_PERSONALINFO() { }
        #endregion

        #region 私有变量
        private string _guid = null;
        private string _loanguid = null;
        private string _reportguid = null;
        private string _custname = null;
        private string _idcard = null;
        private string _checkcause = null;
        private string _gender = null;
        private DateTime? _birthday;
        private string _marrystatus = null;
        private string _cellphone = null;
        private string _companytel = null;
        private string _hometel = null;
        private string _education = null;
        private string _degree = null;
        private string _postaladdr = null;
        private string _householdaddr = null;
        private DateTime? _reportdate;
        private string _isintocheck = null;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键GUID(NOT NULL)
        /// </summary>
        public string GUID
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 对应贷款GUID
        /// </summary>
        public string LOANGUID
        {
            set { _loanguid = value; }
            get { return _loanguid; }
        }
        /// <summary>
        /// 对应征信报告GUID
        /// </summary>
        public string REPORTGUID
        {
            set { _reportguid = value; }
            get { return _reportguid; }
        }
        /// <summary>
        /// 被查询者姓名
        /// </summary>
        public string CUSTNAME
        {
            set { _custname = value; }
            get { return _custname; }
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCARD
        {
            set { _idcard = value; }
            get { return _idcard; }
        }
        /// <summary>
        /// 查询原因
        /// </summary>
        public string CHECKCAUSE
        {
            set { _checkcause = value; }
            get { return _checkcause; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string GENDER
        {
            set { _gender = value; }
            get { return _gender; }
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BIRTHDAY
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MARRYSTATUS
        {
            set { _marrystatus = value; }
            get { return _marrystatus; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string CELLPHONE
        {
            set { _cellphone = value; }
            get { return _cellphone; }
        }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string COMPANYTEL
        {
            set { _companytel = value; }
            get { return _companytel; }
        }
        /// <summary>
        /// 住宅电话
        /// </summary>
        public string HOMETEL
        {
            set { _hometel = value; }
            get { return _hometel; }
        }
        /// <summary>
        /// 学历
        /// </summary>
        public string EDUCATION
        {
            set { _education = value; }
            get { return _education; }
        }
        /// <summary>
        /// 学位
        /// </summary>
        public string DEGREE
        {
            set { _degree = value; }
            get { return _degree; }
        }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string POSTALADDR
        {
            set { _postaladdr = value; }
            get { return _postaladdr; }
        }
        /// <summary>
        /// 户籍地址
        /// </summary>
        public string HOUSEHOLDADDR
        {
            set { _householdaddr = value; }
            get { return _householdaddr; }
        }
        /// <summary>
        /// 报告日期
        /// </summary>
        public DateTime? REPORTDATE
        {
            set { _reportdate = value; }
            get { return _reportdate; }
        }
        /// <summary>
        /// 是否已更新到核查表
        /// </summary>
        public string ISINTOCHECK
        {
            set { _isintocheck = value; }
            get { return _isintocheck; }
        }
        #endregion
    }
}
