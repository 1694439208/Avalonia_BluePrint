using BluePrint.Core.IJoin;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.INode
{
    public enum NodeState
    {
        Error,None,OK,Running
    }
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
        Task Execute(object Context, List<object> arguments, Runtime.Evaluate.Result result);
        
        
    }
}
