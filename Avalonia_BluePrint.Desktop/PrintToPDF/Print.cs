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
        public static void ToFile(string fileName, params Visual[] visuals) => ToFile(fileName, visuals.AsEnumerable());
        public static void ToFile(string fileName, IEnumerable<Visual> visuals)
        {
            using(var doc = SKDocument.CreatePdf(fileName))
            {
                foreach (var visual in visuals)
                {

                    var bounds = visual.Bounds;
                    var page = doc.BeginPage((float)bounds.Width, (float)bounds.Height);
                    using (var context = DrawingContextHelper.WrapSkiaCanvas(page, SkiaPlatform.DefaultDpi))
                    {
                        // 获取ImmediateRenderer.Render方法
                        var assembly = typeof(Avalonia.Rendering.IHitTester).Assembly;
                        var type = assembly.GetType("Avalonia.Rendering+ImmediateRenderer");
                        var method = type.GetMethod("Render", BindingFlags.NonPublic | BindingFlags.Static);

                        // 调用ImmediateRenderer.Render方法
                        method.Invoke(null, new object[] { visual, context });

                        doc.EndPage();
                    }
                }
                doc.Close();
            }
        }
    }
}
