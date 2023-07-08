using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.IJoin
{
    public class Node_Interface_Data
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 提示文本
        /// </summary>
        public string Tips { set; get; }
        /// <summary>
        /// 是否强类型检查 
        /// </summary>
        public bool IsTypeCheck = true;
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type Type { set; get; }
        /// <summary>
        /// 数据
        /// </summary>
        public Object Value { set; get; }
        /// <summary>
        /// 接口类参数
        /// </summary>
        public Dictionary<string,object> ClassValue { set; get; }
        /// <summary>
        /// 获取指定类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>() {
            if (Value==null)
            {
                return default;
            }
            return (T)Value;
        }
    }
}
