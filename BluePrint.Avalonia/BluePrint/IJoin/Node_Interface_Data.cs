using Avalonia;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace 蓝图重制版.BluePrint.IJoin
{
    // 自定义一个泛型类 MyData，用于记录数据类型和数据
    public class MyData
    {
        public Type DataType { get; }
        public object Data { get; }

        public MyData(Type dataType, object data)
        {
            DataType = dataType;
            Data = data;
        }
        public T GetValue<T>()
        {
            if (Data is JObject job)
            {
                return job.ToObject<T>();
            }
            return (T)Data;
        }
    }

    public class MyData<T> : MyData
    {
        public MyData(T data) : base(typeof(T), data)
        {
        }
        
    }
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
        public System.Object Value { set; get; }
        /// <summary>
        /// 接口类参数
        /// </summary>
        public Dictionary<string, MyData> ClassValue { set; get; } = new Dictionary<string, MyData>();
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
