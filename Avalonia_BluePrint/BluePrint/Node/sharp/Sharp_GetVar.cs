using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("取Quicker变量值", "Quicker")]
    public class Sharp_GetVar : NodeBase
    {
        public Sharp_GetVar(BParent _bParent):base(_bParent) {
            Title = "取Quicker变量值";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){ 
                },new Node_Interface_Data{
                    Title = "",
                    Value = "",
                    Type = typeof(string),
                    Tips = "变量名",
                    ClassValue =new Dictionary<string, MyData>(){
                        {nameof(TextBoxJoint.Enabled), new MyData<bool>(false)},
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("表达式")},
                    }
                })
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new Completed(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "变量值",
                    Type = typeof(object),
                    Value = 0,
                    Tips = "变量值",//IEnumerable<string>
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
            var data1 = result[0].Join.Get();
            //return $"{result[0].ID.GetID()} = context.GetVarValue(\"{data}\");";
            var value = Completed.ObjectTypeDic.Values.ToList()[Convert.ToInt32(data1.Value)];
            return $"(({value.Item2})context.GetVarValue(\"{data}\"))";
        }
    }
}
