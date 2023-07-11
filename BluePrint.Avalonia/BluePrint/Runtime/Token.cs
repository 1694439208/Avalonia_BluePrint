using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.Runtime
{
    public class Token
    {
        public enum NodeToken {
            /// <summary>
            /// 表达式，用于中止枚举
            /// </summary>
            Expression,
            /// <summary>
            /// 表达式参数，用于中止枚举，表示一个用来嵌入的表达式
            /// </summary>
            ExpressionValue,
            /// <summary>
            /// 执行
            /// </summary>
            Call,
            /// <summary>
            /// 参数
            /// </summary>
            Value,
            /// <summary>
            /// 又想让他执行又给他参数
            /// </summary>
            CallValue,
            /// <summary>
            /// 自定义参数，自己给节点赋值
            /// </summary>
            ObjectValue,
            None
        }
    }
}
