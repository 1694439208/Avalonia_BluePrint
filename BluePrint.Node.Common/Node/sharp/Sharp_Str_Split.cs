﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluePrint.Core.IJoin;
using BluePrint.Core.INode;
using BluePrint.Core.Join;

namespace BluePrint.Core.Node
{
    [NodeBaseInfo("字符串分割", "字符串处理")]
    public class Sharp_Str_Split : NodeBase
    {
        public Sharp_Str_Split(BParent _bParent):base(_bParent) {
            Title = "字符串分割";
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
                    Tips = "用来分割的字符串" + LOL_JSON.TIPS,
                    ClassValue =new Dictionary<string, MyData>(){
                        {nameof(TextBoxJoint.Enabled),new MyData<bool>(false) },
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("用来分隔的字符串") },
                        //{nameof(TextBoxJoint.Width),130f }
                    }
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "列表变量",
                    Type = typeof(List<object>),
                    Tips = "列表变量",
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
            a = a == "" ? "a" : a;
            var b = arguments[1].GetUid(false);
            //return $"{PrevNodes.join("\r\n")}\r\n    {result[0].IDEndsWith.StartsWithGetID()} = {arguments[0].ID.GetID(false)}.Where(a=>a==1).ToList();{Execute[0]}";
            return $"{a}.Split(new string[]{{{LOL_JSON.ToLiteral(b)}}},StringSplitOptions.None)";
        }
    }
}
