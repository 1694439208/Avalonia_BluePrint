using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Join;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("判断", "流程")]
    public class Branch : NodeBase
    {

        public Branch(BParent _bParent):base(_bParent) {
            Title = "Branch";
            base._IntPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new TextJoint(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "布尔值",
                    Value = false,
                    Type = typeof(bool),
                    Tips = "test",
                    //IsTypeCheck = false,
                }),
            }) ;
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("True执行"),
                    Type = typeof(JoinType),
                    Tips = "True",
                }),
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("Flase执行"),
                    Type = typeof(JoinType),
                    Tips = "False",
                }),
            });
        }
        public override void Execute(object Context, List<object> arguments, in Runtime.Evaluate.Result result) {
            
            var data = _IntPutJoin[1].Item1.Get();
            if (arguments.Get<bool>(0))
            {
                result.SetExecute(0);
            }
            else {
                result.SetExecute(1);
            }

            //计算完毕可以设置接口的值，然后调用渲染,只是为了可视化
            for (int i = 0; i < arguments.Count; i++)
            {
                _IntPutJoin[i + 1].Item1.Set(new Node_Interface_Data {Value = arguments[i]});
                _IntPutJoin[i + 1].Item1.Render();
            }
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
            return $"{PrevNodes.join("\r\n")}\r\n{ret}fi"; 
        }
    }
}
