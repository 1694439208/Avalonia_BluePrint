using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    public interface Context
    {
        /// <summary>
        /// 用于节点定义输入接口
        /// </summary>
        List<(IJoinControl IJoin, Node_Interface_Data JoinData)> IntPutJoin { set; get; }
        /// <summary>
        /// 用于节点定义输出接口
        /// </summary>
        List<(IJoinControl IJoin, Node_Interface_Data JoinData)> OutPutJoin { set; get; }
        /// <summary>
        /// 执行节点
        /// </summary>
        void Execute(object Context, List<object> arguments, in Runtime.Evaluate.Result result);
        
        
    }
}
