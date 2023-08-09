using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System.Xml.Linq;
using Avalonia.Media.Immutable;
using Avalonia.VisualTree;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Layout;
using Avalonia.Rendering.Composition;
using BluePrint.Core.IJoin;
using BluePrint.Core.Join;
using BluePrint.Core.INode.Model;

namespace BluePrint.Core
{
    
    public class BP_Line : Control, INode.Model.IBP_Render
    {
        BluePrint? bluePrint { set; get; }
        public BP_Line(BluePrint _bluePrint) {
            bluePrint = _bluePrint;
        }
        /// <summary>
        /// 线条方向 ，用于执行输入输出 动画
        /// </summary>
        public enum LineDirection
        {
            Input,
            Output
        }

        public void SetColor(Color color)
        {
            backound_color = color;
        }
        public IPen strokePen { set; get; } =  new ImmutablePen(Brushes.DarkBlue,
            3d, null, PenLineCap.Round, PenLineJoin.Round);


        public IPen temp_strokePen => new ImmutablePen(Brushes.DarkBlue,
            3d, null, PenLineCap.Round, PenLineJoin.Round);
        /// <summary>
        /// Gets or sets the width of the element.
        /// </summary>
        public int offset = 0;
        //static readonly StyledProperty<double> offsetProperty =
        //    AvaloniaProperty.Register<Layoutable, double>(nameof(offset), 0);
        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            if (geometry != null)
            {
                dc.DrawGeometry(null, strokePen, geometry);
                // 创建一个GeometryStroke对象，并将其设置为Path对象的Stroke属性
                //geometry.FillRule = FillRule.EvenOdd;
                // 创建一个自定义的画刷
                //var brush = new DrawingBrush
                //{
                //    Drawing = new GeometryDrawing
                //    {
                //        Pen = new Pen(Brushes.Black, 20),
                //        Geometry = new RectangleGeometry(new Rect(0, 0, 10, 10))
                //    }
                //};

                //// 使用画刷描边矩形
                //dc.DrawGeometry(null, new Pen(Brushes.Red, 5), geometry);



                // 使用画刷填充矩形
                //dc.DrawGeometry(brush, null, geometry);
                // 设置抗锯齿模式
                //dc.AntialiasMode = AntialiasMode.AntiAlias;
                //dc.DrawPath(backound_color, new Stroke(_LineWidth), geometry);



                // Get the bounds of the control
                //Rect bounds = new Rect(new Point(0, 0), Bounds.Size);
                // Fill the background with the control's background color
                //dc.FillRectangle(Background, Bounds);


                //skPaint.PathEffect = SKPathEffect.CreateDash(new float[] { 10, 5 }, index);
                //new SolidColorPaint(SKColors.LightSlateGray)
                //{
                //    StrokeThickness = 2,
                //    PathEffect = new DashEffect(new float[] { 3, 3 })
                //}


                //dc.DrawRectangle(strokePen, bounds);
                //Debug.Print(geometry.Figures.Count.ToString());
                //Dispatcher.UIThread.Post(() => { Clip = geometry; });

            }
        }
        
        /// <summary>
        /// 刷新路径
        /// </summary>
        public void RefreshDrawBezier() {
            //InvalidateVisual();
            //InvalidateArrange();
            //InvalidateMeasure();
            var size = PositionReckon();
            //Debug.Print($"RefreshDrawBezier:{size}");
            switch (drant)
            {
                case 4:
                    geometry = DrawBezier(new Point(0, size.Height), new Point(size.Width, 0));
                    break;
                case 3:
                    geometry = DrawBezier(new Point(size.Width, size.Height), new Point(0, 0));
                    break;
                case 2:
                    geometry = DrawBezier(new Point(size.Width, 0), new Point(0, size.Height));
                    break;
                case 1:
                    geometry = DrawBezier(new Point(0, 0),new Point(size.Width, size.Height));
                    break;
                default:
                    break;
            }
        }
        public void Start_Animation(LineDirection lineDirection)
        {
            bluePrint?.AddAnimationLine(this, lineDirection);
        }
        public void End_Animation()
        {
            bluePrint?.RemoveAnimationLine(this);
        }
        public void close_Animation()
        {

        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            //ClipToBounds = true;
            //Width = 10;
        }
        public int GetQuadrant(Point origin, Point point)
        {
            double dx = point.X - origin.X;
            double dy = point.Y - origin.Y;

            if (dx > 0 && dy > 0)
            {
                return 1;
            }
            else if (dx < 0 && dy > 0)
            {
                return 2;
            }
            else if (dx < 0 && dy < 0)
            {
                return 3;
            }
            else if (dx > 0 && dy < 0)
            {
                return 4;
            }
            else
            {
                return 0; // Point is at the origin
            }
        }
        //public override void ApplyTemplate()
        //{
        //    base.ApplyTemplate();
        //    //RefreshDrawBezier();
        //    //InvalidateVisual();
        //}
        /// <summary>
        /// 计算自身尺寸，位置
        /// </summary>
        public Size PositionReckon()
        {
            var size = new Size(0, 0);
            if (_Star != null && _End != null)
            {
                // IJoinControl.NodePosition
                //var offset = UIElementTool.GetParentPosition(_Star,new Point(0,0), _Star.GetVisualParent());

                var _StarPosition = _Star.TranslatePoint(_Star.GetJoinPos(_Star.GetDir()), _Star.Get_NodeRef());
                var _EndPosition = _End.TranslatePoint(_End.GetJoinPos(_End.GetDir()), _End.Get_NodeRef());

                Point Spos = _Star.Get_NodeRef().GetPosition(_StarPosition??new Point(0, 0));//_Star.GetPosition(_Star.Get_NodeRef().GetJoinPos(_Star.GetDir()));
                Point SEnd = _End.Get_NodeRef().GetPosition(_EndPosition??new Point(0, 0));// _End.GetPosition(_End.GetJoinPos(_End.GetDir()));

                //var Spos = _Star.TranslatePoint(_Star.GetJoinPos(_Star.GetDir()), _Star.bParent.bluePrint) ?? default;
                //var SEnd = _End.TranslatePoint(_End.GetJoinPos(_End.GetDir()), _End.bParent.bluePrint) ?? default;

                if (_Star is MouseJoin)
                {
                    Spos = _Star.GetPosition(default);
                    if (Spos == new Point(0,0))
                    {
                        //Debug.WriteLine($"pos:Spos:{Spos}");
                        return size;
                    }
                    //SEnd = _End.Get_NodeRef().GetPosition(_End.GetJoinPos(_End.GetDir()));

                }
                if (_End is MouseJoin)
                {
                    SEnd = _End.GetPosition(default);
                    if (SEnd == new Point(0, 0))
                    {
                        //Debug.WriteLine($"pos:Spos:{Spos}");
                        return size;
                    }
                    //Debug.WriteLine($"pos:SEnd:{SEnd}");
                }
                if (Spos.X == -1|| SEnd.X == -1)
                {
                    return size;
                }
                drant = GetQuadrant(Spos, SEnd);
                //Debug.WriteLine($"drant:{drant}");
                switch (drant)
                {
                    case 4:
                        Width = SEnd.X - Spos.X;
                        Height = Spos.Y - SEnd.Y;
                        Blueprint_Canvas.SetLeft(this, Spos.X);
                        Blueprint_Canvas.SetTop(this, SEnd.Y);
                        break;
                    case 3:
                        Width = Spos.X - SEnd.X;
                        Height = Spos.Y - SEnd.Y;
                        Blueprint_Canvas.SetLeft(this, SEnd.X);
                        Blueprint_Canvas.SetTop(this, SEnd.Y);
                        break;
                    case 2:
                        Width = Spos.X - SEnd.X;
                        Height = SEnd.Y - Spos.Y;
                        Blueprint_Canvas.SetLeft(this, SEnd.X);
                        Blueprint_Canvas.SetTop(this, Spos.Y);
                        break;
                    case 1:
                        Width = SEnd.X - Spos.X;
                        Height = SEnd.Y - Spos.Y;
                        Blueprint_Canvas.SetLeft(this, Spos.X);
                        Blueprint_Canvas.SetTop(this, Spos.Y);
                        break;
                    default:
                        break;
                }
            }
            return new Size(Width, Height);
        }
        private IJoinControl _Star = null;
        private IJoinControl _End = null;
        public IJoinControl GetStar() {
            return _Star;
        }
        public IJoinControl GetEnd()
        {
            return _End;
        }
        private PathGeometry geometry;
        public void SetJoin(IJoinControl a, IJoinControl b)
        {
            _Star = a;
            _End = b;
            //return this;//
            PositionReckon();
            //Invalidate();
        }
        public void SetJoin(Control a, Control b)
        {
            _Star = a as IJoinControl;
            _End = b as IJoinControl;
            PositionReckon();
            //Invalidate();
            // return this;//Control
        }

        public BP_IJoin GetStarJoin() {
            return _Star;
        }
        public BP_IJoin GetEndJoin()
        {
            return _End;
        }
        public void SetStarJoin(IJoinControl value)
        {
            _Star = value;
        }
        public void SetEndJoin(IJoinControl value)
        {
            _End = value;
        }
        public int drant = 0;
        /// <summary>
        /// 线段颜色
        /// </summary>
        //[UIPropertyMetadata(null, UIPropertyOptions.AffectsRender)]//属性变化之后自动刷新
        private Color backound_color = Color.Parse("rgb(159,159,159)");// "rgb(0,169,244)";
        private IBrush Background = new SolidColorBrush(Colors.White);
        /// <summary>
        /// 线条宽度
        /// </summary>
        private float _LineWidth = 2;
        public float LineWidth {
            get {
                return _LineWidth;
            }
            set {
                _LineWidth = value;
            }
        }

        bool IBP_Render.Render { get; set; }

        //protected override void OnMouseEnter(MouseEventArgs e)
        protected override void OnPointerEntered(PointerEventArgs e)
        {
            backound_color = Color.FromRgb((byte)(backound_color.R - 30), backound_color.G, backound_color.B);
            InvalidateVisual();
            base.OnPointerEntered(e);
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            backound_color = Color.FromRgb((byte)(backound_color.R + 30), backound_color.G, backound_color.B);
            InvalidateVisual();
            base.OnPointerReleased(e);
        }
        /// <summary>
        /// 计算两点之间长度
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double Distance(Point p1, Point p2)
        {
            //C# code
            double width = p2.X - p1.X;
            double height = p2.Y - p1.Y;
            double result = (width * width) + (height * height);
            return (double)Math.Sqrt(result);//根号
        }
        /// <summary>
        /// 绘制线条
        /// </summary>
        /// <param name="kCanvas">画布</param>
        /// <param name="cubicTopoint">起始点坐标</param>
        /// <param name="Mousepoint">结束点坐标</param>
        public PathGeometry DrawBezier(Point cubicTopoint,
            Point Mousepoint)
        {
            List<Point> sKPoints = new List<Point>();

            sKPoints.Add(new Point(Mousepoint.X, Mousepoint.Y));
            sKPoints.Add(new Point(cubicTopoint.X, cubicTopoint.Y));

            sKPoints.Add(new Point(50, cubicTopoint.Y));//控制点
            sKPoints.Add(new Point(50, cubicTopoint.Y));//控制点
            //paint1.AddPoly(sKPoints.ToArray());


            double wid = Mousepoint.X - cubicTopoint.X;


            double yiban = Distance(cubicTopoint, Mousepoint) / 3;
            if (Mousepoint.Y < cubicTopoint.Y)
            {
                double hei = cubicTopoint.Y - Mousepoint.Y;
                sKPoints[2] = new Point(cubicTopoint.X + yiban, cubicTopoint.Y);
                sKPoints[3] = new Point(Mousepoint.X - yiban, Mousepoint.Y);
            }
            else
            {
                double hei = Mousepoint.Y - cubicTopoint.Y;
                sKPoints[2] = new Point(cubicTopoint.X + yiban, cubicTopoint.Y);
                sKPoints[3] = new Point(Mousepoint.X - yiban, cubicTopoint.Y + hei);
            }

            //path1.MoveTo(sKPoints[1]);
            //path1.CubicTo(sKPoints[2], sKPoints[3], sKPoints[0]);
            //path1.QuadTo(sKPoints[2], sKPoints[0]);
            //kCanvas.DrawPath(path1, paint1);

            var p = new PathGeometry();


            var figure = new PathFigure();
            //figure.StartPoint = startPoint;// sKPoints[1];

            // 创建一个 PathGeometryContext 对象
            var context = p.Open();
            context.BeginFigure(sKPoints[1], true);
            // 添加三次贝塞尔曲线
            context.CubicBezierTo(sKPoints[2], sKPoints[3], sKPoints[0]);

            // Add the second cubic bezier curve
            //Point midpoint1 = new Point(
            //    (sKPoints[2].X + 2 * sKPoints[1].X + sKPoints[3].X) / 4,
            //    (sKPoints[2].Y + 2 * sKPoints[1].Y + sKPoints[3].Y) / 4
            //);
            //Point controlPoint2 = new Point(midpoint1.X, midpoint1.Y - 5);
            //Point startPoint2 = sKPoints[2];
            //context.CubicBezierTo(controlPoint2, startPoint2, sKPoints[1]);


            // Close the path
            // 关闭 PathGeometryContext 对象
            //context.Close();

            // 将 PathFigure 添加到 PathGeometry 中
            p.Figures?.Add(figure);

            return p;
        }
    }
}
