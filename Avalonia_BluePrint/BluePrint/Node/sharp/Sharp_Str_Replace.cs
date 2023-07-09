﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("字符串替换", "字符串处理")]
    public class Sharp_Str_Replace : NodeBase
    {
        public Sharp_Str_Replace(BParent _bParent):base(_bParent) {
            Title = "字符串替换";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "字符串变量",
                    Type = typeof(string),
                    Tips = "需要字符串变量类型" + LOL_JSON.ListItemTIPS,
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){
                },new Node_Interface_Data{
                    Title = "",
                    Value = "",
                    Type = typeof(string),
                    Tips = "用来替换的字符串" + LOL_JSON.TIPS,
                    ClassValue =new Dictionary<string, object>(){
                        {nameof(TextBoxJoint.Enabled),false },
                        {nameof(TextBoxJoint.Watermark),"str" },
                        //{nameof(TextBoxJoint.Width),130f }
                    }
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this){
                },new Node_Interface_Data{
                    Title = "",
                    Value = "",
                    Type = typeof(string),
                    Tips = "用于替换的字符串" + LOL_JSON.TIPS,
                    ClassValue =new Dictionary<string, object>(){
                        {nameof(TextBoxJoint.Enabled),false },
                        {nameof(TextBoxJoint.Watermark),"替换str的字符串" },
                        //{nameof(TextBoxJoint.Width),130f }
                    }
                })
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "字符串变量",
                    Type = typeof(string),
                    Tips = "返回字符串",
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
            a = a == "" ? "a" : a;
            var b = arguments[1].GetUid(false);
            var c = arguments[2].GetUid(false);
            //return $"{PrevNodes.join("\r\n")}\r\n    {result[0].IDEndsWith.StartsWithGetID()} = {arguments[0].ID.GetID(false)}.Where(a=>a==1).ToList();{Execute[0]}";
            return $"{a}.Replace({LOL_JSON.ToLiteral(b)},{LOL_JSON.ToLiteral(c)})";
        }
    }
}
