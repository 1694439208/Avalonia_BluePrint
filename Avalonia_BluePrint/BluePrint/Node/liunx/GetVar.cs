using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("获取变量值", "LiunxScript")]
    public class GetVar : NodeBase
    {
        public GetVar(BParent _bParent):base(_bParent) {
            Title = "获取变量值";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){ 
                    Watermark = "变量名",
                    Enabled = false,
                },new Node_Interface_Data{
                    Title = "",
                    Value = "",
                    Type = typeof(string),
                    Tips = "变量名",
                })
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ValueText(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "变量值",
                    Value = "",
                    Type = typeof(string),
                    Tips = "变量值",
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
            var data = arguments[0].Join.Get().GetData<string>();
            return $@"{result[0].ID.GetID(false)}=${{{data}}}";
        }
    }
}
