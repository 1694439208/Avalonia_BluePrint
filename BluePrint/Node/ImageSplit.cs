
using Avalonia;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.INode
{
    [NodeBaseInfo("图片rgb分割", "功能")]
    public class ImageSplit : NodeBase
    {
        public ImageSplit():base() { 
        
        }
        public ImageSplit(BParent _bParent):base(_bParent) {
            Title = "分割图像";
            ///节点输出参数 设置
            _OutPutJoin = new List<(IJoinControl, Node_Interface_Data)>
            {
                (new ExecJoin(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
                    Title = "执行结束",
                    Type = typeof(JoinType),
                    Value = new JoinType("执行结束"),
                }),
                (new ValueText(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
                    Title = "R",
                    Type = typeof(Data_Bitmap),
                    Value = new Data_Bitmap("R"),
                }),
                (new ValueText(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
                    Title = "G",
                    Type = typeof(Data_Bitmap),
                    Value = new Data_Bitmap("G"),
                }),
                (new ValueText(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
                    Title = "B",
                    Type = typeof(Data_Bitmap),
                    Value = new Data_Bitmap("B"),
                }),
            };

            ///节点输入参数 设置
            _IntPutJoin = new List<(IJoinControl, Node_Interface_Data)>
            {
                (new ExecJoin(bParent,IJoinControl.NodePosition.Left,this),new Node_Interface_Data{
                    Title = "执行开始",
                    Type = typeof(JoinType),
                    Value = new JoinType("执行开始"),
                }),
                (new ImageJoint(bParent, IJoinControl.NodePosition.Left, this)
                {
                    UInNodeSize = new Size(200, 200),
                },new Node_Interface_Data{
                    Title = "执行开始",
                    Type = typeof(Data_Bitmap),
                    Value = new Data_Bitmap("","res://蓝图重制版/Data/test.jpg"),
                }),
            };
        }
        /// <summary>
        /// 将图片转换成黑白色效果
        /// </summary>
        /// <param name="bmp">原图</param>
        public static unsafe Bitmap[] ImageSplit1(Bitmap bmp)
        {
            //确定图像的宽和高
            //double height = bmp.Size.Height;
            //double width = bmp.Size.Width;
            //Bitmap[] ret = new Bitmap[3];
            //for (int i = 0; i < 3; i++)
            //{
            //    ret[i] = new Bitmap(width, height);
            //}

            //using (BitmapLock l = bmp.Lock(),r1 = ret[0].Lock(), g1 = ret[1].Lock(), b1 = ret[2].Lock())
            //{//l.DataPointer就是数据指针，一般不建议直接使用，因为不同平台，不同图形适配器，不同位图格式，数据格式不一样，那你就需要判断不同格式来遍历指针数据处理了
            //    for (int y = 0; y < height; y++)
            //    {
            //        for (int x = 0; x < width; x++)
            //        {
            //            l.GetPixel(x, y, out byte a, out byte r, out byte g, out byte b);
            //            //var p = (byte)Math.Min(255, 0.7 * r + (0.2 * g) + (0.1 * b));
            //            r1.SetPixel(x, y, (byte)a,r, (byte)0, (byte)0);
            //            g1.SetPixel(x, y, (byte)a, (byte)0, g, (byte)0);
            //            b1.SetPixel(x, y, (byte)a, (byte)0, (byte)0, b);
            //            //l.SetPixel(x, y, a, r, g, b);
            //        } // x
            //    } // y
            //}
            return new Bitmap[3] { bmp , bmp , bmp };
        }
        public override void Execute(object Context, List<object> arguments, in Runtime.Evaluate.Result result)
        {

            //各种计算
            Bitmap bitmap = arguments.Get<Data_Bitmap>(0).bitmap;
            var bitmaps = ImageSplit1(bitmap);
            string[] names = { "R", "G", "B" };

            for (int i = 0; i < 3; i++)
            {
                var bit = new Data_Bitmap(names[i]);
                bit.SetBitmap(bitmaps[i]);
                result.SetReturnValue(i, bit);
            }

           

            //计算完毕可以设置接口的值，然后调用渲染,只是为了可视化
            for (int i = 0; i < arguments.Count; i++)
            {
                _IntPutJoin[i + 1].Item1.Set(new Node_Interface_Data { Value = arguments[i] });
                _IntPutJoin[i + 1].Item1.Render();
            }
            //输出默认
            base.Execute(Context,arguments, result);
        }
        public override string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return $@"
{PrevNodes.join("\r\n")}
//因为没实现这个函数，所以也就简单模拟一下
{result[0]} = img;
{result[1]} = img;
{result[2]} = img;

{Execute.join("\r\n")}
";
        }
    }
}
