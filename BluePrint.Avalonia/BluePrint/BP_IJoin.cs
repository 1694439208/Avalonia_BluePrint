
using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
//using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint
{
    public interface BP_IJoin
    {
        Point GetPos(bool IsZ);
        /// <summary>
        /// 取父元素
        /// </summary>
        /// <returns></returns>
        Control GetParnt();
        /// <summary>
        /// 取自身实例
        /// </summary>
        /// <returns></returns>
        Control GetThis();

        /// <summary>
        /// 取序列化属性键值
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> Dump();
        /// <summary>
        /// 设置反序列化属性键值
        /// </summary>
        /// <param name="data"></param>
        void Load(Dictionary<string, object> data);
    }
}