using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Join;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("大于 >", "数值逻辑运算")]
    public class ThanGreater : NodeBase
    {
        public ThanGreater(BParent _bParent):base(_bParent) {
            Title = "大于 >";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "整数类型",
                    Value = 0,
                    Type = typeof(int),
                    Tips = "整数",
                    IsTypeCheck = false,
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "整数类型",
                    Value = 0,
                    Type = typeof(int),
                    Tips = "整数",
                    IsTypeCheck = false,
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ValueText(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "",
                    Value = false,
                    Type = typeof(bool),
                    Tips = "逻辑判断",
                    IsTypeCheck = false,
                }),
            });
        }
        public override void Execute(object Context, List<object> arguments, in Runtime.Evaluate.Result result) {

            result.SetReturnValue(0, arguments.Get<int>(0) > arguments.Get<int>(1));

            //计算完毕可以设置接口的值，然后调用渲染,只是为了可视化
            for (int i = 0; i < arguments.Count; i++)
            {
                _IntPutJoin[i].Item1.Set(new Node_Interface_Data { Value = arguments[i] });
                _IntPutJoin[i].Item1.RenderData();
            }
            foreach (var item in result.GetReturns())
            {
                _OutPutJoin[item.Key].Item1.Set(new Node_Interface_Data { Value =item.Value });
                _OutPutJoin[item.Key].Item1.RenderData();
            }
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return $@"
{PrevNodes.join("\r\n")}
{result[0].ID.GetID(false)}=[{arguments[0].GetData(a => a.Value)} -gt {arguments[1].GetData(a => a.Value)}]";
        }
    }
}
