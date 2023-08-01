using System;
using System.Collections.Generic;
using System.Text;
using BluePrint;
using BluePrint.Core.IJoin;
using BluePrint.Core.INode;
using BluePrint.Core.Join;
using BluePrint.Core.Runtime;

namespace BluePrint.Core.Node
{
    [NodeBaseInfo("转JSON", "数据转换")]
    public class Sharp_ToJson : NodeBase
    {
        public Sharp_ToJson(BParent _bParent):base(_bParent) {
            Title = "转JSON";
            base._IntPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "任意变量",
                    Type = typeof(string),
                    IsTypeCheck = false,
                    Tips = "返回表达式",
                }),
            }) ;
            base._OutPutJoin.AddRange(new List<(IJoinControl, Node_Interface_Data)>{
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行下一步",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new LableJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "表达式",
                    Type = typeof(string),
                    Tips = "返回表达式",
                }),
            });
        }

        public override async Task Execute(object Context, List<object> arguments, Evaluate.Result result)
        {
            _IntPutJoin[1].Item1.Set(new Node_Interface_Data { Value = arguments[0] });
            _IntPutJoin[1].Item1.RenderData();
            //输出默认
            await base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            var a = arguments[0].GetUid(false);
            var b = result[0].GetUid(false);

            var ret = $@"{PrevNodes.join("\r\n")}{"\r\n"}    var st = {a};{"\r\n"}System.Runtime.Serialization.Json.DataContractJsonSerializer json = new System.Runtime.Serialization.Json.DataContractJsonSerializer(st.GetType());
    MemoryStream stream = new MemoryStream();
    json.WriteObject(stream, st);
    var {b} = System.Text.Encoding.UTF8.GetString(stream.ToArray());
    stream.Dispose();{Execute[0]}";

            return ret;
        }
    }
}
