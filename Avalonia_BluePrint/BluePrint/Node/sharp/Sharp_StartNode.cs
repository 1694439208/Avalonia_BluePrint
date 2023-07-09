using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("生成代码", "Quicker")]
    public class Sharp_StartNode : NodeBase
    {
        public Sharp_StartNode(BParent _bParent) :base(_bParent)
        {
            Title = "生成代码";
            ///节点输出参数 设置
            _OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>
            {
                (new GeneratorJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                    ClassValue = new Dictionary<string, MyData>{
                        {"IsButton", new MyData<bool>(true)}
                    },
                }),
            }) ;

            ///节点输入参数 设置
            /*_IntPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>
            {
            });*/
        }
        
        /// <summary>
        /// 计算节点
        /// </summary>
        public override void Execute(object Context, List<object> arguments, in Runtime.Evaluate.Result result)
        {
            base.Execute(Context,arguments, result);
        }
        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return $@"using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
// Quicker将会调用的函数
public static void Exec(Quicker.Public.IStepContext context)
{{
{Execute.join("\r\n")}
}}
";
        }
    }
}
