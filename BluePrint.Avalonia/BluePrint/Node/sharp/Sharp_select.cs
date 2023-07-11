using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("修改列表", "列表功能")]
    public class Sharp_select : NodeBase
    {
        public Sharp_select(BParent _bParent):base(_bParent) {
            Title = "修改列表";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "列表变量",
                    Type = typeof(List<object>),
                    Tips = "列表变量",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "修改条件",
                    Type = typeof(string),
                    Tips = "对每一个元素处理",
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行下一步",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "列表变量",
                    Type = typeof(List<object>),
                    Tips = "列表变量",
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
            var ret = Execute.Count switch
            {
                1 => $@"if {arguments[0].ID.GetID()}
then
    {Execute[0]}",
                2 => $@"if {arguments[0].ID.GetID()}
then
    {Execute[0]}
else
    {Execute[1]}",
                _ => ""
            };
            


            var data = arguments[0].Join.Get();
            if (data.Type == typeof(List<object>))
            {
                return $"{PrevNodes.join("\r\n")}\r\n    {result[0].ID.GetID()} = {arguments[0].GetUid(false)}.Select(a=>{arguments[1].CodeTemplate}).ToList();{Execute[0]}";
            }
            else {
                return "变量类型不是列表，请检查quicker变量类型";
            }
            //return $@"{result[0].ID.GetID(false)}=${{{data}}}";
            
        }
    }
}
