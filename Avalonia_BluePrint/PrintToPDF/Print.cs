using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Skia;
using Avalonia.Skia.Helpers;
using Avalonia.VisualTree;
using BluePrint.Avalonia.BluePrint.Tool;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Avalonia.PrintToPDF
{
    internal class Print
    {
        public static Stream ToPDFStream(params Visual[] visuals) => ToPDFStream(visuals.AsEnumerable());
        public static Stream ToPDFStream(IEnumerable<Visual> visuals)
        {
            var ret = new MemoryStream();
            using var doc = SKDocument.CreatePdf(ret);
            foreach (var visual in visuals)
            {
                var bounds = visual.Bounds;
                var page = doc.BeginPage((float)bounds.Width, (float)bounds.Height);
                using var context = DrawingContextHelper.WrapSkiaCanvas(page, SkiaPlatform.DefaultDpi);
                //DrawingContextImpl.FromDrawingContextImpl(contextImpl);
                // 获取ImmediateRenderer.Render方法
                var assembly = typeof(Avalonia.Rendering.IHitTester).Assembly;
                var Media_assembly = typeof(Avalonia.Media.DrawingContext).Assembly;
                var type = assembly.GetTypes().Where(a => a.Name == "ImmediateRenderer").FirstOrDefault();
                if (type == null)
                {
                    throw new System.Exception("无法找到：ImmediateRenderer");
                }
                //var type = assembly.GetType("Avalonia.Rendering+ImmediateRenderer");
                var method = type.GetMethod("Render", new System.Type[] { typeof(Visual), typeof(DrawingContext) });
                //创建一个绘图对象
                var platformdrawingcontext = Media_assembly.GetTypes().Where(a => a.Name == "PlatformDrawingContext").FirstOrDefault();

                var Control = System.Activator.CreateInstance(platformdrawingcontext, new object[] { context, true });

                // 调用ImmediateRenderer.Render方法
                method?.Invoke(null, new object[] { visual, Control });
                doc.EndPage();
            }
            doc.Close();
            ret.Position = 0;
            return ret;
        }

        public static Stream ToPNGStream(Control visuals)
        {
            // 创建一个渲染目标位图
            var render_Bounds = visuals.GetTransformedBounds();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(new PixelSize((int)render_Bounds?.Bounds.Width,
                (int)render_Bounds?.Bounds.Height));

            // 将控件渲染到位图中
            renderTargetBitmap.Render(visuals);

            // 创建一个新的位图对象
            //Bitmap bitmap = new Bitmap(renderTargetBitmap.PlatformImpl, renderTargetBitmap.Size);

            // 将渲染目标位图复制到新的位图对象中
            //.PlatformImpl.Copy(bitmap.PlatformImpl, 0, 0);
            var ret = new MemoryStream();
            renderTargetBitmap.Save(ret, 1);
            ret.Position = 0;
            return ret;
        }
    }
}
