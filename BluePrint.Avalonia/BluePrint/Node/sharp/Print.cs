using Document.Join;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint.Join;
using 蓝图重制版.BluePrint.Node;
using 蓝图重制版.BluePrint.Runtime;

namespace Document.Node
{
    [NodeBaseInfo("注释", "其他")]
    public class Print : NodeBase
    {
        public Print(BParent _bParent):base(_bParent) {
            Title = "注释";
            base._IntPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new JoinTextView(bParent, IJoinControl.NodePosition.Left, this){
                    //日志是可以浏览的，所以不禁用
                },new Node_Interface_Data{
                    Title = "变量类型",
                    Value = "",
                    Type = typeof(object),
                    Tips = "打印的变量",
                    IsTypeCheck = false,
                    ClassValue = new Dictionary<string, MyData>{
                        {"IsEnabledd",new MyData<bool>(false)}
                    },
                }),
                (new test(bParent, IJoinControl.NodePosition.Left, this){
                    //日志是可以浏览的，所以不禁用
                },new Node_Interface_Data{
                    Title = "变量类型",
                    Value = "",
                    Type = typeof(object),
                    Tips = "打印的变量",
                    IsTypeCheck = false,
                })
            }) ;
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                })
            });
        }

        public override void Execute(object Context, List<object> arguments, in Evaluate.Result result)
        {
            _IntPutJoin[1].Item1.Set(new Node_Interface_Data { Value = arguments[0] });
            _IntPutJoin[1].Item1.Render();
            //输出默认
            base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            var a = arguments[0].Join.Get();
            return $@"{PrevNodes.join("\r\n")}    /*{a.Value}*/{Execute.join("\r\n")}";
        }
    }
}
