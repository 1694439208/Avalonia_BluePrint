using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Join;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("打印", "LiunxScript")]
    public class Print : NodeBase
    {
        public Print(BParent _bParent):base(_bParent) {
            Title = "打印";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new ValueText(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "变量类型",
                    Value = "",
                    Type = typeof(object),
                    Tips = "打印的变量",
                    IsTypeCheck = false,
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                })
            });
        }

        public override async Task Execute(object Context, List<object> arguments, Runtime.Evaluate.Result result)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                _IntPutJoin[i + 1].Item1.Set(new Node_Interface_Data { Value = arguments[i] });
                _IntPutJoin[i + 1].Item1.RenderData();
            }
            //输出默认
            await base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return $@"{PrevNodes.join("\r\n")}
echo ${{{arguments[0].ID.GetID(false)}}}
{Execute.join("\r\n")}";
        }
    }
}
