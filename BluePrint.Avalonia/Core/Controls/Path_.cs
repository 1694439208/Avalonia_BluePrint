using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrint.Core.Controls
{
    public class Path_ : UserControl
    {
        public static readonly StyledProperty<PathGeometry> GeometryProperty =
            AvaloniaProperty.Register<Path_, PathGeometry>(nameof(Geometry));

        public PathGeometry Geometry
        {
            get => GetValue(GeometryProperty);
            set => SetValue(GeometryProperty, value);
        }
        public static readonly StyledProperty<double> StrokeThicknessProperty =
            AvaloniaProperty.Register<Path_, double>(nameof(StrokeThickness));
        public double StrokeThickness
        {
            get => GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }


        public override void Render(DrawingContext context)
        {
            base.Render(context);
            //context.FillRectangle(new SolidColorBrush(Color.FromRgb(255, 255, 255)), this.Bounds);
            Geometry = CreatePathGeometry(Bounds.Width, Bounds.Height);
            if (Geometry != null)
            {
                var pen = new Pen(Brushes.Black, StrokeThickness)
                {
                    LineCap = PenLineCap.Round,
                    LineJoin = PenLineJoin.Round
                };

                context.DrawGeometry(null, pen, Geometry);
            }
        }
        private PathGeometry CreatePathGeometry(double width, double height)
        {
            width -= StrokeThickness*2;
            height -= StrokeThickness*2;
            // 创建 PathFigure 并直接计算坐标点
            var pathFigure = new PathFigure
            {
                StartPoint = new Point(0 + StrokeThickness, 0 + StrokeThickness),
                Segments = new PathSegments
                {
                    new LineSegment { Point = new Point(width/3*2 + StrokeThickness, 0 + StrokeThickness) },
                    new LineSegment { Point = new Point(width + StrokeThickness, height / 2 + StrokeThickness) },
                    new LineSegment { Point = new Point(width/3*2 + StrokeThickness, height + StrokeThickness) },
                    new LineSegment { Point = new Point(0 + StrokeThickness, height + StrokeThickness) }
                },
                IsClosed = true
            };

            // 创建 PathGeometry
            var pathGeometry = new PathGeometry
            {
                Figures = new PathFigures { pathFigure }
            };

            return pathGeometry;
        }
    }
}
