/******************************************
* 模块名称：实体 征信导入名单表
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
	/// 实体 征信导入名单表
	/// </summary>
	[Description("Primary:GUID")]
    [Serializable]
	public class CREDIT_IMPORT_INFO
	{
        #region 构造函数
        /// <summary>
        /// 实体 征信导入名单表
        /// </summary>
        public CREDIT_IMPORT_INFO(){}
        #endregion

        #region 私有变量
        private string _guid = null;
        private string _customername = null;
        private string _idcard = null;
        private string _departmentguid = null;
        private string _departmentname = null;
        private string _createby = null;
        private DateTime _createdate = DateTime.MinValue;
        private string _active = null;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 主键(NOT NULL)
        /// </summary>
        public string GUID
        {
            set{ _guid=value;}
            get{return _guid;}
        }
        /// <summary>
        /// 客户姓名(NOT NULL)
        /// </summary>
        public string CUSTOMERNAME
        {
            set{ _customername=value;}
            get{return _customername;}
        }
        /// <summary>
        /// 客户身份证
        /// </summary>
        public string IDCARD
        {
            set{ _idcard=value;}
            get{return _idcard;}
        }
        /// <summary>
        /// 部门guid
        /// </summary>
        public string DEPARTMENTGUID
        {
            set{ _departmentguid=value;}
            get{return _departmentguid;}
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DEPARTMENTNAME
        {
            set{ _departmentname=value;}
            get{return _departmentname;}
        }
        /// <summary>
        /// 创建人,关联pubcodeinfo
        /// </summary>
        public string CREATEBY
        {
            set{ _createby=value;}
            get{return _createby;}
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CREATEDATE
        {
            set{ _createdate=value;}
            get{return _createdate;}
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        public string ACTIVE
        {
            set{ _active=value;}
            get{return _active;}
        }
        #endregion
	}
}
