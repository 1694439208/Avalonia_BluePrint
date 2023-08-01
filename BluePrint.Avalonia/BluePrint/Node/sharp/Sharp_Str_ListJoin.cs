
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("列表拼接到字符串", "列表功能")]
    public class Sharp_Str_ListJoin : NodeBase
    {
        public Sharp_Str_ListJoin(BParent _bParent):base(_bParent) {
            Title = "列表拼接到字符串";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "列表变量",
                    Type = typeof(List<object>),
                    Tips = "要拼接的列表变量"+LOL_JSON.ListItemTIPS,
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){
                },new Node_Interface_Data{
                    Title = "",
                    Value = "",
                    Type = typeof(string),
                    Tips = "每个元素中的分隔符" + LOL_JSON.TIPS,
                    ClassValue =new Dictionary<string, MyData>(){
                        {nameof(TextBoxJoint.Enabled),new MyData<bool>(false) },
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("分隔符") },
                        //{nameof(TextBoxJoint.Width),130f }
                    }
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "字符串变量",
                    Type = typeof(string),
                    Tips = "返回字符串",
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
            var b = arguments[1].GetUid(false);
            //return $"{PrevNodes.join("\r\n")}\r\n    {result[0].IDEndsWith.StartsWithGetID()} = {arguments[0].ID.GetID(false)}.Where(a=>a==1).ToList();{Execute[0]}";
            return $"string.Join({b.ToLiteral()},{a})";
        }
    }
}
