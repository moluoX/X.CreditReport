/******************************************
* 模块名称：实体 征信报告居住信息记录表
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
    /// 实体 征信报告居住信息记录表
    /// </summary>
    [Description("Primary:GUID")]
    [Serializable]
    public class CREDIT_LIVINGINFO
    {
        #region 构造函数
        /// <summary>
        /// 实体 征信报告居住信息记录表
        /// </summary>
        public CREDIT_LIVINGINFO() { }
        #endregion

        #region 私有变量
        private string _guid = null;
        private string _loanguid = null;
        private string _reportguid = null;
        private Int32? _sn;
        private string _address = null;
        private string _status = null;
        private DateTime? _updatedate;
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
        /// 编号
        /// </summary>
        public Int32? SN
        {
            set { _sn = value; }
            get { return _sn; }
        }
        /// <summary>
        /// 居住地址
        /// </summary>
        public string ADDRESS
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 居住状况
        /// </summary>
        public string STATUS
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 信息更新日期
        /// </summary>
        public DateTime? UPDATEDATE
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
        #endregion
    }
}
