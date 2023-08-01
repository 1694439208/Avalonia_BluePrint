using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.IJoin
{
    //仅限用于类，且可以多次使用属性
    [AttributeUsage(AttributeTargets.Class,Inherited = false)]
    //[AttributeUsage(AttributeTargets.All)]
    public class NodeBaseInfoAttribute : Attribute
    {
        private string _NodeName;
        private string _NodeGroup;
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName
        {
            get { return _NodeName; }
            set { _NodeName = value; }
        }
        public string NodeGroup
        {
            get { return _NodeGroup; }
            set { _NodeGroup = value; }
        }
        //构造函数，设置属性参数
        public NodeBaseInfoAttribute(string Name,string Group)
        {
            _NodeName = Name;
            _NodeGroup = Group;
        }
    }
}
