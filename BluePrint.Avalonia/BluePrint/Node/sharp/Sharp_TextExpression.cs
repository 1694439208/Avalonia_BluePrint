using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("自定义条件", "表达式(条件)")]
    public class Sharp_TextExpression : NodeBase
    {
        public Sharp_TextExpression(BParent _bParent):base(_bParent) {
            Title = "自定义条件";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){
                    Watermark = "表达式",
                    //Enabled = false,
                },new Node_Interface_Data{
                    Title = "",
                    Value = "a",
                    Type = typeof(string),
                    Tips = "可以自己写代码表达式条件",
                    ClassValue =new Dictionary<string, MyData>(){
                        {nameof(TextBoxJoint.Enabled),new MyData<bool>(false) },
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("表达式" )},
                        {nameof(TextBoxJoint.Width),new MyData<double>(130 ) }
                        
                    }
                })
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "",
                    Type = typeof(string),
                    Tips = "",
                }),
            });
        }

        public override async Task Execute(object Context, List<object> arguments, Runtime.Evaluate.Result result)
        {
            //输出默认
            result.SetReturnValue(0, arguments.LastOrDefault()??"");
            await base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            var a = arguments[0].GetUid(false);
            //return $"{PrevNodes.join("\r\n")}\r\n    {result[0].ID.GetID()} = {arguments[0].ID.GetID(false)}.Where(a=>a==1).ToList();{Execute[0]}";

            return $"{a}";
        }
    }
}
