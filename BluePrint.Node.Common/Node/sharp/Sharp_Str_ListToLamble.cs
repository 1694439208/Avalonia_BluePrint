
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluePrint.Core.IJoin;
using BluePrint.Core.INode;
using BluePrint.Core.Join;

namespace BluePrint.Core.Node
{
    [NodeBaseInfo("列表转表达式", "表达式(条件)")]
    public class Sharp_Str_ListToLamble : NodeBase
    {
        public Sharp_Str_ListToLamble(BParent _bParent):base(_bParent) {
            Title = "列表转表达式";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "列表变量",
                    Type = typeof(List<object>),
                    Tips = "要转换的列表变量",
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "表达式",
                    Type = typeof(string),
                    Tips = "表达式",
                }),
            });
        }

        public override async Task Execute(object Context, List<object> arguments, Runtime.Evaluate.Result result)
        {
            //输出默认
            await base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            var a = arguments[0].GetUid(false);
            //return $"{PrevNodes.join("\r\n")}\r\n    {result[0].IDEndsWith.StartsWithGetID()} = {arguments[0].ID.GetID(false)}.Where(a=>a==1).ToList();{Execute[0]}";
            return $"{a}";
        }
    }
}
