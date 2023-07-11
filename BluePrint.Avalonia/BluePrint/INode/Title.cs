
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint
{
    public class Title : Panel
    {
        //public override void Render(DrawingContext dc)
        //{
        //    /*var rect = new Rect(ActualSize);
        //    var gradientFill = new RadialGradientFill();
        //    gradientFill.Center = new Point(-10, -10);
        //    gradientFill.Radius = 100;

        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(138, 192, 7), 0.5f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(35, 38, 35), 0.9f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(138, 192, 7), 0.5f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(35, 38, 35), 0.9f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(35, 38, 35), 0.9f));
        //    using (var brush = gradientFill.CreateBrush(rect, Root.RenderScaling))
        //    {
        //        dc.FillRectangle(brush, rect);
        //    }*/
        //    base.Render(dc);
        //}
        public string title;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            VisualChildren.Add(new TextBlock
            {
                Text = title,
                //Foreground = "218,223,221",
                FontSize = 15,
                FontFamily = "微软雅黑",
            });
        }
        //protected override void InitializeComponent()
        //{
        //    var gradientFill = new RadialGradientFill();
        //    gradientFill.Center = new Point(-10, -10);
        //    gradientFill.Radius = 100;

        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(138, 192, 7), 0.5f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(35, 38, 35), 0.9f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(138, 192, 7), 0.5f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(35, 38, 35), 0.9f));
        //    gradientFill.GradientStops.Add(new GradientStop(Color.FromRgb(35, 38, 35), 0.9f));

        //    CornerRadius = new CornerRadius(3.8f, 3.8f, 0, 0);
        //    Background = gradientFill;

        //    Children.Add(new TextBlock { 
        //        Text = title,
        //        Foreground = "218,223,221",
        //        FontSize = 15,
        //        FontFamily = "微软雅黑",
        //    });
        //}
    }
}
