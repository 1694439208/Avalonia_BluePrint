
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using BluePrint.Core.IJoin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace BluePrint.Core.INode
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
        public Subject<string> _title = new Subject<string>();
        public string title = "";

        public void SetTitle(string data)
        {
            //_title.OnNext(data);
            if (this.FindViewControl<TextBlock>("title1") is TextBlock textBlock1)
            {
                textBlock1.Text = data;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Children.Add(new TextBlock
            {
                Name = "title1",
                Text = title,
                //[!TextBlock.TextProperty] = new Binding("title"),
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
