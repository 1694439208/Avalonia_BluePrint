using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("设置Quicker变量值", "Quicker")]
    public class Sharp_SetVar : NodeBase
    {
        public Sharp_SetVar(BParent _bParent):base(_bParent) {
            Title = "设置Quicker变量值";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){ 
                },new Node_Interface_Data{
                    Title = "",
                    Value = "",
                    Type = typeof(string),
                    Tips = "变量名",
                    ClassValue =new Dictionary<string, MyData>(){
                        {nameof(TextBoxJoint.Enabled), new MyData<bool>(false)},
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("变量名")},
                    }
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "返回变量",
                    Value = " 变量",
                    Type = typeof(object),
                    IsTypeCheck=false,
                    Tips = "返回变量",
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行下一步",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
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
            var varName = arguments[0].Join.Get().GetData<string>();
            var data = arguments[1].IsExpression? arguments[1].CodeTemplate : arguments[1].ID.GetID(false);
            return $"{PrevNodes.join("\r\n")}\r\n    context.SetVarValue(\"{varName}\", {data});{Execute[0]}";
            //return $"{PrevNodes.join("\r\n")}\r\n{result[0].ID.GetID()} = {arguments[0].ID.GetID(false)}.Where(a=>a==1).ToList();{Execute[0]}";
        }
    }
}
