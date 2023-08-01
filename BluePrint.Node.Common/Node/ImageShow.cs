
using Avalonia;
using BluePrint.Core.DataType;
using BluePrint.Core.IJoin;
using BluePrint.Core.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace BluePrint.Core.INode
{
    [NodeBaseInfo("打印图片", "功能")]
    public class ImageShow : NodeBase
    {
        public ImageShow() : base(){ }
        public ImageShow(BParent _bParent):base(_bParent) {
            Title = "显示图像";
            ///节点输出参数 设置
            _OutPutJoin = new List<(IJoinControl, Node_Interface_Data)>
            {
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
            };

            ///节点输入参数 设置
            _IntPutJoin = new List<(IJoinControl, Node_Interface_Data)>
            {
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new ImageJoint(bParent, IJoinControl.NodePosition.Left, this)
                {
                    UInNodeSize = new Size(200, 200),
                },new Node_Interface_Data{
                    Title = "image",
                    Value = new Data_Bitmap(""),
                    Type = typeof(Data_Bitmap),
                    Tips = "",
                    ClassValue = new Dictionary<string, MyData>{
                        {"UInNodeSize",new MyData<Data_Size>(new Data_Size(200, 200))}
                    },
                }),
            };
        }

        public override async Task Execute(object Context, List<object> arguments, Runtime.Evaluate.Result result)
        {
            //计算完毕可以设置接口的值，然后调用渲染,只是为了可视化
            for (int i = 0; i < arguments.Count; i++)
            {
                _IntPutJoin[i+1].Item1.Set(new Node_Interface_Data { Value = arguments[i] });
                _IntPutJoin[i+1].Item1.RenderData();
            }
            //输出默认
            await base.Execute(Context,arguments, result);
        }

        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            var ret = $@"{PrevNodes.join("\r\n")}打印({arguments[0].ID.GetID()});{Execute.join("\r\n")}";
            return ret;
        }
    }
}
