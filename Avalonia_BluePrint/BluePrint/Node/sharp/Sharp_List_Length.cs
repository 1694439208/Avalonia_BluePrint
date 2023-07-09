
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("列表取长度", "列表功能")]
    public class Sharp_List_Length : NodeBase
    {
        public Sharp_List_Length(BParent _bParent):base(_bParent) {
            Title = "列表取长度";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){
                    //Watermark = "表达式",
                    //Enabled = false,
                },new Node_Interface_Data{
                    Title = "",
                    Value = "a",
                    Type = typeof(string),
                    Tips = "字符串变量",
                    ClassValue =new Dictionary<string, object>(){
                        {nameof(TextBoxJoint.Enabled),false },
                        {nameof(TextBoxJoint.Watermark),"变量" },
                        //{nameof(TextBoxJoint.Width),130f }
                    }
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "长度|整数型",
                    Type = typeof(List<object>),
                    Tips = "返回列表内容数量",
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
            var a = arguments[0].GetUid(false);
            return $"{a}.Count";
        }
    }
}
