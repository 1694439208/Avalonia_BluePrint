using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BluePrint.Core.IJoin;
using BluePrint.Core.INode;
using BluePrint.Core.Join;

namespace BluePrint.Core.Node
{
    [NodeBaseInfo("字符串拼接", "字符串处理")]
    public class Sharp_StrAppend : NodeBase
    {
        public Sharp_StrAppend(BParent _bParent):base(_bParent) {
            Title = "字符串拼接";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new AddTextBoxJoin(bParent, IJoinControl.NodePosition.Left, this){
                    
                },new Node_Interface_Data{
                    Title = "添加拼接",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "添加一个拼接的接口" + LOL_JSON.ListItemTIPS,
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
            //如果当前接口有指针指向那就读取指针生成变量，如果没有那就读取当前接口值处理
            var str = string.Join("+", arguments.Select(a => { return $"{a.GetUidALL(false)}"; }).ToArray());
            return str;
        }
    }
}
