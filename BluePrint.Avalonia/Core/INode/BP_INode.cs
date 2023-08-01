//using CPF;
//using CPF.Controls;
//using CPF.Drawing;
//using CPF.Input;
//using CPF.Shapes;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using BluePrint.INode;
//using BluePrint.Node;

//namespace BluePrint
//{
//    public class BP_INode1 : Control
//    {
//        /*protected override void OnRender(DrawingContext dc)
//        {
//            base.OnRender(dc);
//        }*/
//        private List<BP_Line> OuPutJoin = new List<BP_Line>();
//        private List<BP_Line> InPutJoin = new List<BP_Line>();
//        /*
//         intPutJoin = new Dictionary<string, BP_IJoin>
//            {
//                {"->",new TextJoint()},
//                {"时间",new TextJoint()},

//            };
//            OutPutJoin = new Dictionary<string, BP_IJoin>
//            {
//                {"->",new TextJoint()},
//                {"时间戳",new TextJoint()},
//            };
//         */



//        protected override void InitializeComponent()
//        {
//            ClipToBounds = true;
//            this.CornerRadius = "3.8";
//            Background = Color.FromRgb(35, 38, 35);
//            BorderFill = "#000";
//            BorderStroke = "1";
//            RefreshNodes();
//        }
//        public List<BP_IJoin> OuPutIJoin = new List<BP_IJoin>();
//        public List<BP_IJoin> InPutIJoin = new List<BP_IJoin>();

//        int interval = 20;//接口的间隔y坐标
//        protected override void OnGotFocus(GotFocusEventArgs e)
//        {
//            //base.OnGotFocus(e);
//            BorderFill = Color.FromRgb(197, 131, 35);
//            BorderStroke = "1";
//        }
//        protected override void OnLostFocus(RoutedEventArgs e)
//        {
//            //base.OnLostFocus(e);
//            BorderFill = Color.FromRgb(35, 38, 35);
//            BorderStroke = "1";
//        }
//        public void RefreshNodes()
//        {
//            Children.Add(new Title
//            {
//                IsAntiAlias = true,
//                Width = "100%",
//                Height = TitleHeight,
//                MarginLeft = 0,
//                MarginTop = 0,
//            });
//            var a = new TextJoint()
//            {
//                //ClipToBounds =true,
//                MarginLeft = 0f,
//                Width = 35,
//            };
//            AddIJoin(NodePosition.Left, a);
//            for (int j = 0; j < 5; j++)
//            {
//                var a1 = new TextJoint
//                {
//                    MarginRight = ActualOffset.X + ActualSize.Width,
//                    Width = 35,
//                };
//                AddIJoin(NodePosition.right, a1);
//            }
//            //

//        }

//        int TitleHeight = 25;
//        public void AddIJoin(NodePosition nodePosition, Control bP_IJoin)
//        {
//            Children.Add(bP_IJoin);//左边
//            if (nodePosition == NodePosition.Left)
//            {
//                InPutIJoin.Add(bP_IJoin as BP_IJoin);
//            }
//            else
//            {
//                OuPutIJoin.Add(bP_IJoin as BP_IJoin);
//            }


//            var size = GetHeights();
//            Width = size.Width;
//            Height = size.Height + TitleHeight;
//            float Outheight = TitleHeight;
//            float Inheight = TitleHeight;
//            int i = 0;
//            while (true)
//            {
//                if (InPutIJoin.Count > i)
//                {
//                    var col = InPutIJoin[i].GetThis();
//                    col.MarginLeft = 1;
//                    col.MarginTop = Inheight + (i == 0 ? 16 : interval);
//                    Inheight += (i == 0 ? 16 : interval) + col.ActualSize.Height;
//                }
//                if (OuPutIJoin.Count > i)
//                {
//                    var col = OuPutIJoin[i].GetThis();
//                    col.MarginRight = ActualSize.Width;
//                    col.MarginTop = Outheight + (i == 0 ? 16 : interval);
//                    Outheight += (i == 0 ? 16 : interval) + col.ActualSize.Height;
//                }
//                if (OuPutIJoin.Count <= i && InPutIJoin.Count <= i)
//                {
//                    return;
//                }
//                i++;
//            }
//        }
//        public Size GetHeights()
//        {
//            int i = 0;
//            float ret1 = 0;
//            float ret2 = 0;

//            float retw1 = 0;
//            float retw2 = 0;

//            while (true)
//            {
//                if (InPutIJoin.Count > i)
//                {
//                    var col = InPutIJoin[i].GetThis();
//                    ret1 += interval + col.ActualSize.Height;
//                    retw1 = col.Width.Value > retw1 ? col.Width.Value : retw1;
//                }
//                if (OuPutIJoin.Count > i)
//                {
//                    var col = OuPutIJoin[i].GetThis();
//                    ret2 += interval + col.ActualSize.Height;
//                    retw2 = col.Width.Value > retw2 ? col.Width.Value : retw2;
//                }
//                if (OuPutIJoin.Count <= i && InPutIJoin.Count <= i)
//                {
//                    return new CPF.Drawing.Size(retw1 + retw2 + interval, (ret1 > ret2 ? ret1 : ret2) + interval);
//                }
//                i++;
//            }
//        }
//        protected override void OnMouseDown(MouseButtonEventArgs e)
//        {
//            if (e.MouseButton == MouseButton.Left)
//            {
//                Focus();
//                Mouxy = e.Location;
//                isclick = true;
//                (Parent as BluePrint).SelectThisInstance(this);
//                CaptureMouse();
//                //ZIndex = Parent.GetChildren();
//            }
//            base.OnMouseDown(e);
//        }
//        protected override void OnMouseUp(MouseButtonEventArgs e)
//        {
//            base.OnMouseUp(e);
//            if (e.LeftButton == MouseButtonState.Released)
//            {
//                isclick = false;
//                ReleaseMouseCapture();
//            }
//        }
//        private Point Mouxy;
//        private bool isclick = false;
//        protected override void OnMouseMove(MouseEventArgs e)
//        {
//            if (e.LeftButton == MouseButtonState.Pressed && isclick)
//            {
//                //为了刷新溢出了的线条
//                Parent.Parent.Invalidate();
//                var a = e.Location - Mouxy;
//                MarginLeft += a.X;
//                MarginTop += a.Y;
//                // TransformPoint
//            }
//            base.OnMouseMove(e);
//        }
//        ///// <summary>
//        ///// 背景填充
//        ///// </summary>
//        //[UIPropertyMetadata(null, UIPropertyOptions.AffectsRender)]//属性变化之后自动刷新
//        //public ViewFill Background
//        //{
//        //    get { return (ViewFill)GetValue(); }
//        //    set { SetValue(value); }
//        //}
//        public enum NodePosition
//        {
//            Left, right
//        }



//        ////public Dictionary<NodePosition, KeyValuePair<BP_Line, BP_Line>> Join = new Dictionary<NodePosition, KeyValuePair<BP_Line, BP_Line>>();

//        //// public Dictionary<BP_Line, KeyValuePair<NodePosition, BP_Line>>




//        //private float Outheight = 0;
//        //private float Inheight = 0;

//        //private int wid = 0;
//        //private int offset = 5;





//        //    switch (nodePosition)
//        //    {
//        //        case NodePosition.Left:
//        //            OuPutJoin.Add(bP_Line);
//        //            bP_Line.setXY(new Point {
//        //                X = 0,
//        //                Y = Inheight += offset
//        //            });
//        //            Children.Add(bP_Line);
//        //            bP_Line.LineStar = new Point(bP_Line.LineStar.X + ActualOffset.X, ActualOffset.Y + bP_Line.LineStar.Y);
//        //            break;
//        //        case NodePosition.right:
//        //            InPutJoin.Add(bP_Line);
//        //            bP_Line.setXY(new Point
//        //            {
//        //                X = Width.Value - bP_Line.Width.Value,
//        //                Y = Outheight += offset
//        //            });
//        //            Children.Add(bP_Line);
//        //            bP_Line.LineStar = new Point(Width.Value + bP_Line.LineStar.X, Height.Value + bP_Line.LineStar.Y);
//        //            break;
//        //        default:
//        //            break;
//        //    }
//        //}
//    }
//}
