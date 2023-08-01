//using CPF;
//using CPF.Controls;
//using CPF.Drawing;
//using CPF.Input;
//using System;
//using System.Collections.Generic;
//using BluePrint.IJoin;
//using BluePrint.Node;
//using System.Linq;

//namespace BluePrint.INode
//{
//    //[NodeClass("字符串拼接")]
//    class NodeContext : Control,Context
//    {
//        public NodeContext(BParent _bParent) {
//            bParent = _bParent;
//        }
//        BParent bParent;
//        List<(IJoinControl, Node_Interface_Data)> _IntPutJoin;
//        List<(IJoinControl, Node_Interface_Data)> Context.IntPutJoin { 
//            get{
//                return _IntPutJoin;
//            }
//            set {
//                value = _IntPutJoin;
//            }
//        }
//        List<(IJoinControl, Node_Interface_Data)> _OutPutJoin;
//        List<(IJoinControl, Node_Interface_Data)> Context.OutPutJoin {
//            get {
//                return _OutPutJoin;
//            }
//            set
//            {
//                value = _OutPutJoin;
//            }
//        }

//        /// <summary>
//        /// 计算节点
//        /// </summary>
//        public void Execute()
//        {
//            //输入节点  _IntPutJoin 可以直接读取所有输入节点数据

//            //各种计算
//            MessageBox.Show("执行节点");
//            if (_OutPutJoin.Count > 0 && _OutPutJoin[0].Item1.GetJoinType() == typeof(JoinType))
//            {
//                var line = bParent.bluePrint.FildOutJoin(_OutPutJoin[0].Item1);
//                foreach (var item in line)
//                {
//                    ((item.GetEndJoin() as IJoinControl).Get_NodeRef() as Context).Execute();
//                }
//            }
//        }

//        protected override void InitializeComponent()
//        {
//            ClipToBounds = true;
//            this.CornerRadius = "3.8";
//            Background = Color.FromRgb(35, 38, 35);
//            BorderFill = "#000";
//            BorderStroke = "1";
            
//            ///节点输出参数 设置
//            _OutPutJoin = new List<(IJoinControl, Node_Interface_Data)>
//            {
//                (new ExecJoin(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{ 
//                    Title = "",
//                    Type = typeof(JoinType),
//                    Value = new JoinType("执行结束"),
//                }),
//                (new TextJoint(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
//                    Title = "      ->",
//                    Type = typeof(string),
//                }),
//                (new TextJoint(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
//                    Title = "时间戳11111",
//                    Type = typeof(string),
//                }),
//                (new DateJoint(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
//                    Title = "0",
//                    Type = typeof(string),
//                }),
//                (new SliderJoint(bParent,IJoinControl.NodePosition.right,this),new Node_Interface_Data{
//                    Title = "1",
//                    Type = typeof(string),
//                }),
//            };

//            ///节点输入参数 设置
//            _IntPutJoin = new List<(IJoinControl, Node_Interface_Data)>
//            {
//                (new ExecJoin(bParent,IJoinControl.NodePosition.Left,this),new Node_Interface_Data{
//                    Title = "",
//                    Type = typeof(JoinType),
//                    Value = new JoinType("执行开始"),
//                }),
//                (new TextJoint(bParent,IJoinControl.NodePosition.Left,this),new Node_Interface_Data{
//                    Title = "->",
//                    Type = typeof(string),
//                }),
//                (new TextJoint(bParent,IJoinControl.NodePosition.Left,this),new Node_Interface_Data{
//                    Title = "时间",
//                    Type = typeof(string),
//                }),
//                (new TextBoxJoint(bParent,IJoinControl.NodePosition.Left,this),new Node_Interface_Data{
//                    Title = "这是普通textbox",
//                    Type = typeof(string),
//                    Value = "这是普通textbox",
//                }),
//                (new ImageJoint(bParent,IJoinControl.NodePosition.Left,this),new Node_Interface_Data{
//                    Title = "",
//                    Type = typeof(DataType.Data_Bitmap),
//                    Value = new DataType.Data_Bitmap("","https://images0.cnblogs.com/blog/337520/201301/27145133-99cbff0ad8124c4f9b85fb3ba79d057f.jpg"),
//                }),

//            };
//            var stack = new DockPanel
//            {
//                Children =
//                {
//                    new StackPanel
//                    {
//                        Orientation = Orientation.Vertical,
//                        Attacheds =
//                        {
//                            {DockPanel.Dock,Dock.Left}
//                        },
//                        MarginTop = TitleHeight + 5,
//                        //MarginRight = 20,
//                    },
//                    new StackPanel
//                    {
//                        Orientation = Orientation.Vertical,
//                        Attacheds =
//                        {
//                            {DockPanel.Dock,Dock.Right}
//                        },
//                        MarginLeft = 20,
//                        MarginTop = TitleHeight + 5,
//                    },
//                }
//            };
//            InPutIJoin = stack.Children[0] as StackPanel;
//            OuPutIJoin = stack.Children[1] as StackPanel;
//            Children.Add(stack);
//            RefreshNodes();
//        }
//        public StackPanel OuPutIJoin;
//        public StackPanel InPutIJoin;

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
//        protected override void OnLayoutUpdated()
//        {
//            base.OnLayoutUpdated();
//            //IsKeyboardFocusWithin
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
//            foreach (var item in _IntPutJoin)
//            {
//                item.Item1.SetType(item.Item2);
//                item.Item1.Set(item.Item2);
//                item.Item1.Attacheds.Add(DockPanel.Dock, Dock.Left);
//                InPutIJoin.Children.Add(item.Item1);
//            }
//            foreach (var item in _OutPutJoin)
//            {
//                item.Item1.SetType(item.Item2);
//                item.Item1.Set(item.Item2);
//                item.Item1.Attacheds.Add(DockPanel.Dock, Dock.Right);
//                OuPutIJoin.Children.Add(item.Item1);
//            }
//        }

//        int TitleHeight = 25;
        
//        [PropertyChanged(nameof(IsKeyboardFocusWithin))]
//        void OnIsKeyboardFocusWithin(object newValue, object oldValue, PropertyMetadataAttribute attribute)
//        {
//            /*if ((bool)newValue)
//            {
//                BorderFill = Color.FromRgb(197, 131, 35);
//                BorderStroke = "1";
//            }
//            else {
//                BorderFill = Color.FromRgb(35, 38, 35);
//                BorderStroke = "1";
//            }
//            */
//        }
//        protected override void OnMouseDown(MouseButtonEventArgs e)
//        {
//            base.OnMouseDown(e);
//            if (e.MouseButton == MouseButton.Left)
//            {
//                if (!IsKeyboardFocusWithin)
//                {
//                    Focus(NavigationMethod.Click);
//                }
                
//                Mouxy = e.Location;
//                isclick = true;
//                (Parent as BluePrint).SelectThisInstance(this);
//                CaptureMouse();
//                //ZIndex = Parent.GetChildren();
//            }
            
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
//            //e.Handled = true;
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

//        public enum NodePosition
//        {
//            Left, right
//        }
//    }
//}
