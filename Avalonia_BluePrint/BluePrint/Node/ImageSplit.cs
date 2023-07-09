
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia_BluePrint.BluePrint.DataType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
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
                    ClassValue = new Dictionary<string, MyData>{
                        {"UInNodeSize",new MyData<Data_Size>(new Data_Size(200, 200))}
                    },
                    Title = "执行开始",
                    Type = typeof(Data_Bitmap),
                    Value = new Data_Bitmap("","F:\\Users\\Administrator\\source\\repos\\CPF蓝图\\蓝图重制版\\Data\\test.jpg"),
                }),
            };
        }
        public unsafe static Bitmap[] ImageSplit1(Bitmap bitmap)
        {
            // 获取位图的宽度和高度
            int width = bitmap.PixelSize.Width;
            int height = bitmap.PixelSize.Height;

            var stride = width * 4;
            var data = new byte[width * stride];

            var copyTo = new byte[data.Length];
            fixed (byte* pCopyTo = copyTo)
                bitmap.CopyPixels(default, new IntPtr(pCopyTo), data.Length, stride);


            var redPixels = new byte[width * height * 4];
            var greenPixels = new byte[width * height * 4];
            var bluePixels = new byte[width * height * 4];

            // 分隔红、绿、蓝通道
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 计算像素在字节数组中的偏移量
                    int offset = (y * stride) + (x * 4);

                    // 获取红、绿、蓝通道值
                    byte red = copyTo[offset + 2];
                    byte green = copyTo[offset + 1];
                    byte blue = copyTo[offset];

                    // 将通道值写入对应的数组中
                    redPixels[offset + 2] = red;
                    greenPixels[offset + 1] = green;
                    bluePixels[offset] = blue;
                    // 输出通道值
                    //Debug.WriteLine($"Pixel ({x}, {y}): R={red} G={green} B={blue}");
                }
            }

            Bitmap redBitmap;
            Bitmap greenBitmap;
            Bitmap blueBitmap;
            ;
            fixed (byte* r = redPixels)
                redBitmap = new Bitmap(bitmap.Format.Value,Avalonia.Platform.AlphaFormat.Premul, new IntPtr(r), bitmap.PixelSize, bitmap.Dpi, stride);
            fixed (byte* g = greenPixels)
                greenBitmap = new Bitmap(bitmap.Format.Value, Avalonia.Platform.AlphaFormat.Premul, new IntPtr(g), bitmap.PixelSize, bitmap.Dpi, stride);
            fixed (byte* b = bluePixels) 
                blueBitmap = new Bitmap(bitmap.Format.Value, Avalonia.Platform.AlphaFormat.Premul, new IntPtr(b), bitmap.PixelSize, bitmap.Dpi, stride);

            // 返回 Bitmap 数组，包含分离后的红、绿、蓝通道
            return new Bitmap[] { redBitmap, greenBitmap, blueBitmap };
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
