using BluePrint.Core.IJoin;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.Runtime
{
    public class NodeAst
    {
        /// <summary>
        /// 当前接口指针是否指向自己
        /// </summary>
        public bool Isthis { set; get; }
        /// <summary>
        /// 指向了vm Context的指针，用于模拟堆栈
        /// </summary>
        public int NodeJoinId { set; get; }
        /// <summary>
        /// 节点类型
        /// </summary>
        public Token.NodeToken NodeToken { set; get; }
        /// <summary>
        /// 接口对象，用于获取接口数据
        /// </summary>
        public IJoinControl Join { set; get; }
        /// <summary>
        /// 下一个流程线的执行节点列表
        /// </summary>
        public List<NodeAst> NextNodes = new List<NodeAst>();
        /// <summary>
        /// 上一个表达式
        /// </summary>
        public List<NodeAst> PrevNodes = new List<NodeAst>();
        
        /// <summary>
        /// 节点输入参数
        /// </summary>
        public List<NodeAst> Arguments = new List<NodeAst>();
        /// <summary>
        /// 节点输出的参数
        /// </summary>
        public List<NodeAst> Results = new List<NodeAst>();
        /// <summary>
        /// 当前节点指针
        /// </summary>
        public INode.NodeBase NodeBase { set; get; }
        /// <summary>
        /// 自定义数据
        /// </summary>
        public object Value { set; get; }
    }
}
