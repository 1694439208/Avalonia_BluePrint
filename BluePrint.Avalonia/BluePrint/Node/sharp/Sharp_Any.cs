﻿using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("是否存在数据", "列表功能")]
    public class Sharp_Any : NodeBase
    {
        public Sharp_Any(BParent _bParent):base(_bParent) {
            Title = "是否存在数据";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "列表变量",
                    Type = typeof(List<object>),
                    Tips = "列表变量",
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行下一步",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "布尔变量",
                    Type = typeof(bool),
                    Tips = "数据量>=1会返回True",
                }),
            });
        }

        public override void Execute(object Context, List<object> arguments, in Runtime.Evaluate.Result result)
        {
            //输出默认
            base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            var data = arguments[0].Join.Get();
            if (data.Type == typeof(List<object>))
            {
                return $"{PrevNodes.join("\r\n")}\r\n    {result[0].ID.GetID()} = {arguments[0].GetUid(false)}.Any();{Execute[0]}";
            }
            else {
                return "变量类型不是列表，请检查quicker变量类型";
            }
            //return $@"{result[0].ID.GetID(false)}=${{{data}}}";
            
        }
    }
}