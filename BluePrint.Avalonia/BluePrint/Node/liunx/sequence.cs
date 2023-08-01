using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("顺序执行1", "流程")]
    public class sequence : NodeBase
    {
        public sequence(BParent _bParent):base(_bParent) {
            Title = "序列执行";
            base._IntPutJoin.AddRange(new List<(IJoinControl,Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
            });
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("Flase执行"),
                    Type = typeof(JoinType),
                    Tips = "False",
                }),
                (new AddExecJoin(bParent, IJoinControl.NodePosition.right, this){
                },new Node_Interface_Data{
                    Title = "添加执行",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "添加执行",
                }),
            });
            //if (_OutPutJoin[2].Item1 is AddExecJoin join)
            //{
            //    join.OnJoinEveTemp += (s, e) => {
            //        var temp = e.Message as DataType.JoinEventType;
            //        if (temp.eveType == DataType.EveType.MouseUp)
            //        {
            //            //_OutPutJoin.find
            //            AddOntPut((new ExecJoin(bParent, IJoinControl.NodePosition.right, this), new Node_Interface_Data
            //            {
            //                Title = "执行结束的接头",
            //                Value = new JoinType("执行结束"),
            //                Type = typeof(JoinType),
            //                Tips = "test",
            //            }), s as IJoinControl, IsAddList: true);
            //        }
            //    };
            //}
        }

        public override async Task Execute(object Context, List<object> arguments, Runtime.Evaluate.Result result)
        {
            //输出默认
            await base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return $@"
{Execute.join("\r\n")}
";
        }
    }
}
