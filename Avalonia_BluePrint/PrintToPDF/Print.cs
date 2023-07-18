using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Skia;
using Avalonia.Skia.Helpers;
using Avalonia.VisualTree;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Avalonia.PrintToPDF
{
    internal class Print
    {
        public static string ToFile(string fileName, params Visual[] visuals) => ToFile(fileName, visuals.AsEnumerable());
        public static string ToFile(string fileName, IEnumerable<Visual> visuals)
        {
            try
            {
                using var doc = SKDocument.CreatePdf(fileName);
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
                        return "无法找到：ImmediateRenderer";
                    }
                    //var type = assembly.GetType("Avalonia.Rendering+ImmediateRenderer");
                    var method = type.GetMethod("Render", new System.Type[] { typeof(Visual), typeof(DrawingContext) });
                    //创建一个绘图对象
                    var platformdrawingcontext = Media_assembly.GetTypes().Where(a => a.Name == "PlatformDrawingContext").FirstOrDefault();

                    var Control = System.Activator.CreateInstance(platformdrawingcontext, new object[] { context,true });

                    // 调用ImmediateRenderer.Render方法
                    method?.Invoke(null, new object[] { visual, Control });
                    doc.EndPage();
                    return "生成pdf成功";
                }
                doc.Close();
                return "生成pdf完成";
            }
            catch (System.Exception ex)
            {
                return $"生成失败：{ex.Message}";
            }
        }
    }
}
