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
    [NodeBaseInfo("转表达式", "数据转换")]
    public class Sharp_ToEval : NodeBase
    {
        public Sharp_ToEval(BParent _bParent):base(_bParent) {
            Title = "转表达式";
            base._IntPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "任意变量",
                    Type = typeof(string),
                    IsTypeCheck = false,
                    Tips = "返回表达式",
                }),
            }) ;
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "表达式",
                    Type = typeof(string),
                    Tips = "返回表达式",
                }),
            });
        }

        public override void Execute(object Context, List<object> arguments, in Evaluate.Result result)
        {
            _IntPutJoin[1].Item1.Set(new Node_Interface_Data { Value = arguments[0] });
            _IntPutJoin[1].Item1.RenderData();
            //输出默认
            base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return $@"{ arguments[0].CodeTemplate}";
        }
    }
}
