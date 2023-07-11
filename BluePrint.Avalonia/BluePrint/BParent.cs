using Avalonia.Controls;
using Avalonia;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint.Join;
using 蓝图重制版.BluePrint.Node;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Primitives;
using BluePrint.Avalonia.BluePrint.Controls;
using Document.Node;
using BluePrint.Avalonia.BluePrint.DataType;
using Avalonia.LogicalTree;
using System.Xml.Linq;

namespace 蓝图重制版.BluePrint
{
    public class BParent : Panel
    {
        /// <summary>
        /// 设置节点上下文
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(List<Type> context) {
            _NodeTypes = context;
        }
        /// <summary>
        /// 在蓝图0，0位置创建一个指定类型的节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Control CreateNode(Type type, double x1 = 0, double y1 = 0)
        {
            Control control = null;
            if (_NodeTypes != null)
            {
                foreach (var item in _NodeTypes.Where(x => x == type).ToList())
                {
                    var Control = System.Activator.CreateInstance(item, new object[] { this });
                    //(Control as Control).Margin = $"{x1},{y1},auto,auto";
                    Canvas.SetLeft((Control as Control), x1);
                    Canvas.SetTop((Control as Control), y1);
                    bluePrint.AddChildren((Control)Control);
                    control = (Control)Control;
                }
            }
            bluePrint.InvalidateVisual();
            bluePrint.InvalidateArrange();
            bluePrint.InvalidateMeasure();
            return control;
        }
        /// <summary>
        /// 创建一个接头
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public IJoinControl CreateJoin(Type type, IJoinControl.NodePosition position, NodeBase node)
        {
            var Control = System.Activator.CreateInstance(type, new object[] { this, position, node });//,new object[] { this, position, node }
            return (IJoinControl)Control;
        }
        /// <summary>
        /// 在蓝图指定位置创建一个指定类型的节点
        /// </summary>
        /// <param name="type">节点类型</param>
        /// <param name="element">鼠标相对元素</param>
        /// <param name="point">元素内部偏移</param>
        public void CreateNode(Type type,Control element,Point point) {
            if (_NodeTypes!=null)
            {
                Point p = element.GetPosition(point);
                foreach (var item in _NodeTypes.Where(x => x == type).ToList())
                {
                    var Control = System.Activator.CreateInstance(item, new object[] { this });
                    //(Control as Control).Margin = $"{p.X},{p.Y},auto,auto";
                    Canvas.SetLeft((Control as Control), p.X);
                    Canvas.SetTop((Control as Control), p.Y);
                    bluePrint.AddChildren((Control)Control);
                }
            }
        }
        public struct BPNodeType
        {
            /// <summary>
            /// 节点类型
            /// </summary>
            public Type node { set; get; }
        }
        public struct BPPutJoin
        {
            /// <summary>
            /// 序列化用的id
            /// </summary>
            public IJoinControl.NodePosition Position { set; get; }
            /// <summary>
            /// 序列化用的id
            /// </summary>
            public int ID { set; get; }
            /// <summary>
            /// 接头类型
            /// </summary>
            public Type type { set; get; }
            /// <summary>
            /// 接头类内参数
            /// </summary>
            public Dictionary<string, object> PropretyValue { set; get; }
            /// <summary>
            /// 接头需要的数据
            /// </summary>
            public Node_Interface_Data Data { set; get; }
        }
        public struct BPNodedata {
            /// <summary>
            /// 序列化用的id
            /// </summary>
            public int ID { set; get; }
            /// <summary>
            /// 位置
            /// </summary>
            public Data_Point Point { set; get; }
            /// <summary>
            /// 节点
            /// </summary>
            public BPNodeType node { set; get; }
            /// <summary>
            /// 节点的输出数据
            /// </summary>
            public List<BPPutJoin> _OutPutJoin {set;get;}
            /// <summary>
            /// 节点的输入数据
            /// </summary>
            public List<BPPutJoin> _IntPutJoin { set; get; }
        }
        public struct BPJoindata
        {
            /// <summary>
            /// 执行线的头节点
            /// </summary>
            public List<int> _Star { set; get; }
            /// <summary>
            /// 执行线的尾节点
            /// </summary>
            public List<int> _End { set; get; }
        }
        public class BPByte
        {
            /// <summary>
            /// 节点列表
            /// </summary>
            public List<BPNodedata> NodeList { set; get; }
            /// <summary>
            /// 执行线列表
            /// </summary>
            public List<BPJoindata> JoinList { set; get; }
        }

        public void Deserialize(BPByte data) {
            //数据反序列化到蓝图
            List<KeyValuePair<int, KeyValuePair<int, IJoinControl>>> keyValues = new List<KeyValuePair<int, KeyValuePair<int, IJoinControl>>>();
            List<NodeBase> controlList = new List<NodeBase>();
            var indexa = 1;
            foreach (var item in data.NodeList)
            {
                //CreateNode((Type)tree1.Tag, Canvas.GetLeft(parent.MousepanelPupopPos), Canvas.GetTop(parent.MousepanelPupopPos));
                var control = CreateNode(item.node.node,item.Point.X, item.Point.Y);
                if (control == null)
                {
                    continue;
                }
                var basenode = control as NodeBase;
                controlList.Add(basenode);
                basenode.ID = item.ID;
                //反序列化创建的 把他输入输出都删掉重新创建
                basenode._IntPutJoin.Clear();
                basenode.ClearIntPut();
                basenode._OutPutJoin.Clear();
                basenode.ClearOntPut();
                //然后再重新创建
                foreach (var item1 in item._IntPutJoin)
                {
                    indexa++;
                    var join = CreateJoin(item1.type, item1.Position, basenode);
                    //var data1 = item1.Data.Value.GetType();
                    //Newtonsoft.Json.JObject
                    //data1.Value = (typeof(data1.Type))(data1.Value);
                    join.Set(item1.Data);
                    join.ID = item1.ID;
                    keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(item.ID, new KeyValuePair<int, IJoinControl>(item1.ID, join)));
                    foreach (var pv in item1.PropretyValue)
                    {
                        //join.SetPropretyValue(pv.Key, pv.Value);
                        var property = AvaloniaPropertyRegistry.Instance.FindRegistered(pv.Value.GetType(), pv.Key);
                        join.SetValue(property, pv.Value);
                    }
                    basenode._IntPutJoin.Add((join, item1.Data));
                }
                foreach (var item1 in item._OutPutJoin)
                {
                    indexa++;
                    var join = CreateJoin(item1.type, item1.Position, basenode);
                    join.ID = item1.ID;
                    join.Set(item1.Data);
                    keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(item.ID,new KeyValuePair<int, IJoinControl>(item1.ID, join)));
                    foreach (var pv in item1.PropretyValue)
                    {
                        //join.SetPropretyValue(pv.Key, pv.Value);
                        var property = AvaloniaPropertyRegistry.Instance.FindRegistered(pv.Value.GetType(), pv.Key);
                        join.SetValue(property, pv.Value);
                    }
                    basenode._OutPutJoin.Add((join, item1.Data));
                }
                basenode.RefreshNodes();
                //basenode.RefreshDrawBezier();
            }
            //节点创建完创建执行线
            foreach (var item1 in data.JoinList)
            {
                //执行线只支持一对一
                var bP_Line1 = new BP_Line
                {
                    Width = 1f,
                    Height = 1f,
                    //backound_color = "rgb(255,255,255)",
                };
                Canvas.SetLeft(bP_Line1, 0);
                Canvas.SetTop(bP_Line1, 0);
                //var nodelist = bluePrint.GetChildrenList();
                var star1 = from grade in keyValues
                            where grade.Key == item1._Star[0] && grade.Value.Key == item1._Star[1]
                            select grade.Value.Value;
                var end1 = from grade in keyValues
                           where grade.Key == item1._End[0] && grade.Value.Key == item1._End[1]
                           select grade.Value.Value;

                var star = star1.FirstOrDefault();
                var end = end1.FirstOrDefault();
                if (star != null && end != null)
                {
                    if (star.GetNodeType() == Runtime.Token.NodeToken.CallValue || star.GetNodeType() == Runtime.Token.NodeToken.Call)
                    {
                        //bP_Line1.backound_color = "rgb(255,255,255)";
                        //执行节点设置为白色执行线
                    }
                    bluePrint.AddLineChildren(bP_Line1);
                    bP_Line1.SetJoin(star, end);
                    bP_Line.InvalidateVisual();
                    //创建完成让他先刷新一下
                    bP_Line1.RefreshDrawBezier();

                }
            }
            /*foreach (var item in controlList)
            {
                BeginInvoke(new Action(() => {
                    (item as BP_Line).RefreshDrawBezier();
                }));
            }*/
            /*this.Delay(TimeSpan.FromSeconds(1), () => {
                foreach (var item in controlList)
                {
                    BeginInvoke(new Action(() => {
                        item.RefreshDrawBezier();
                        //item.InvalidateMeasure();
                        //item.InvalidateArrange();
                        //item.Invalidate();
                    }));
                }
                controlList.Clear();
            });*/
            controlList.Clear();
            keyValues.Clear();

            //this.OnLayoutUpdated();
        }
        public BPByte SerializeBP()
        {
            //蓝图序列化到数据保存
            //bluePrint.Instances.Add(control);
            List<BPNodedata> pdata = new List<BPNodedata>();
            var index_id = 0;
            var ChildList = bluePrint.GetChildrenList();
            foreach (var item5 in ChildList)
            {
                if (item5 is NodeBase item)
                {
                    item.ID = index_id;
                    index_id++;
                    List<BPPutJoin> IntPutJoin = new List<BPPutJoin>();
                    foreach (var item1 in item._IntPutJoin)
                    {
                        IntPutJoin.Add(new BPPutJoin
                        {
                            Position = item1.Item1._position,
                            type = item1.Item1.GetType(),//Type
                            Data = item1.Item1.Get(),
                            ID = index_id,
                            PropretyValue = item1.Item1.Dump()
                        }) ;
                        item1.Item1.ID = index_id;
                        index_id++;
                    }

                    List<BPPutJoin> OutPutJoin = new List<BPPutJoin>();
                    foreach (var item1 in item._OutPutJoin)
                    {
                        OutPutJoin.Add(new BPPutJoin
                        {
                            Position = item1.Item1._position,
                            type = item1.Item1.GetType(),
                            Data = item1.Item1.Get(),
                            ID = index_id,
                            PropretyValue = item1.Item1.Dump()
                        });
                        item1.Item1.ID = index_id;
                        index_id++;
                    }
                    pdata.Add(new BPNodedata
                    {
                       
                        Point = new Data_Point(Canvas.GetLeft(item), Canvas.GetTop(item)),//item.ActualOffset
                        ID = item.ID,
                        node = new BPNodeType
                        {
                            node = item.GetType()
                        },
                        _IntPutJoin = IntPutJoin,
                        _OutPutJoin = OutPutJoin,
                    });
                }
            }
            List<BPJoindata> IntPutJoin1 = new List<BPJoindata>();
            foreach (BP_Line item in bluePrint.GetLinesList())
            {
                //        _Star = null;
                //private IJoinControl _End = null;
                //item._IntPutJoin
                IntPutJoin1.Add(new BPJoindata
                {
                    _Star = new List<int> { (item.GetStar().Get_NodeRef() as NodeBase).ID , item.GetStar().ID},
                    _End = new List<int> { (item.GetEnd().Get_NodeRef() as NodeBase).ID, item.GetEnd().ID },
                });
            }
            var bp = new BPByte
            {
                NodeList = pdata,
                JoinList = IntPutJoin1,
            };
            return bp;
        }

        //public override void Render(DrawingContext dc)
        //{
        //    var rect = this.Bounds;
        //    base.Render(dc);
        //    // new SolidColorBrush(Color.FromRgb(39, 39, 39));
        //    dc.FillRectangle(new SolidColorBrush(Color.FromRgb(255, 255, 255)), rect);

        //    dc.DrawGeometry(null, XPathColor, XPath);
        //    dc.DrawGeometry(null, YPathColor, YPath);

        //}
        //private Brush XPathColor = new SolidColorBrush(Color.FromArgb(255,52, 52, 52 )) ;
        //private Brush YPathColor = new SolidColorBrush(Color.FromArgb(150,0, 0, 0));


        public BluePrint bluePrint;
        //public P
        protected override void OnInitialized()
        {
            //Instances.Add(this);
            //模板定义
            ClipToBounds = true;
            //base.OnInitialized();

            ///Background = Color.FromRgb(39, 39, 39);
            bluePrint = new BluePrint
            {
                //MarginLeft = 0f,
                //MarginTop = 0f,
                //Width = "100%",
                //Height = "100%",
                Tag = 1f,
                //RenderTransformOrigin = new PointField(0, 0),
            };
            Canvas.SetLeft(bluePrint,0);
            Canvas.SetTop(bluePrint, 0);
            //panel = new Panel
            //{
            //    MarginLeft = 0f,
            //    MarginTop = 0f,
            //    Children = { bluePrint }
            //};ControlbluePrint
            Children.Add(new GridLinesControl { });
            
            Children.Add(bluePrint);
            //添加拖动节点
            MouseJoin = new MouseJoin(this, IJoinControl.NodePosition.Left, this) {
                //MarginLeft = 0f,
                //MarginTop = 0f,
                Width = 1f,
                Height = 1f,
                IsVisible = false
                //Visibility = Visibility.Hidden
            };
            Canvas.SetLeft(MouseJoin, 0);
            Canvas.SetTop(MouseJoin, 0);
            bluePrint.AddChildren1(MouseJoin);
            //
            //添加默认拖动显示线条
            bP_Line = new BP_Line
            {
                //MarginLeft = 0f,
                //MarginTop = 0f,
                //backound_color = Color.Parse("#89C4F8"),
                LineWidth = 5,
            };
            Canvas.SetLeft(bP_Line, 0);
            Canvas.SetTop(bP_Line, 0);
            MousepanelPupopPos = new Panel
            {
                Width = 1,
                Height = 1,
                IsVisible = false,
            };
            VisualChildren.Add(MousepanelPupopPos);
            //bP_Line.Size = new SizeField(1,1);AddLineChildren
            //ClearState();AddChildren
            //之所以不用AddLineChildren 是因为此线条为默认，不参与计算判断
            bluePrint.AddChildren1(bP_Line);
            //ClearState();

        }

        float scale = 1;
        float scale_value = 0.8f;

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var p = e.GetCurrentPoint(bluePrint);

            bluePrint.RenderTransformOrigin = new RelativePoint(new Point(p.Position.X / bluePrint.Bounds.Width, p.Position.Y / bluePrint.Bounds.Height), RelativeUnit.Relative);

            if (e.Delta.Y < 0)
            {
                scale *= scale_value;
            }
            else
            {
                scale /= scale_value;
            }

            bluePrint.RenderTransform = new ScaleTransform(scale, scale);

            base.OnPointerWheelChanged(e);
        }

        Point? mousePos;
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            ClearState();
            base.OnPointerPressed(e);
            //var key = Root.InputManager.KeyboardDevice.Modifiers;

            ////Console.WriteLine($"key:{key}");
            ////cpf接收不到alt的消息暂时用ctrl代替，等待修复
            //if (e.MouseButton == MouseButton.Right &&
            //    (key == InputModifiers.Control|| key == (InputModifiers.Control | InputModifiers.RightMouseButton)))
            //{
            //    mousePos = e.Location / scale;
            //    CaptureMouse();
            //}
            MousePoint = e.GetPosition(this);//.Location;
            Canvas.SetLeft(MousepanelPupopPos, MousePoint.X);
            Canvas.SetTop(MousepanelPupopPos, MousePoint.Y);
        }
        
        /// <summary>
        /// 鼠标位置为了弹窗IList, ICollection
        /// </summary>
        public Panel MousepanelPupopPos;

        private List<Type> _NodeTypes = new List<Type>();
        public List<Type> NodeContextTypes
        {
            set {
                _NodeTypes = value;
            }
            get {
                return _NodeTypes;
            }
        }
        public Flyout? popup;
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            
            base.OnPointerReleased(e);
            //Console.ForegroundColor = ConsoleColor.Red;
            if (e.InitialPressMouseButton == MouseButton.Right)
            {
                //Console.WriteLine($"if (e.MouseButton == MouseButton.Right&& key == InputModifiers.None)");
                if (popup == null)
                {
                    //Console.WriteLine($"popup == null");
                    //var SearchElTextBox = new TextBox
                    //{
                    //    Name = "SearchElTextBox",
                    //    Watermark = "搜索",
                    //};

                    //var p2 = new Panel
                    //{
                    //    Children =
                    //    {
                    //        SearchElTextBox,
                    //        new TextBlock
                    //        {
                    //            Name = "closetext",
                    //            //Classes = "el-icon,el-icon-close",
                    //            //MarginRight =0f,
                    //            Width = 20,
                    //            Foreground = new SolidColorBrush(Color.FromArgb(255,0,0,0)),
                    //        },
                    //    },
                    //};

                    //var textBox1 = p2.LogicalChildren().Where(x => x. == name)
                    popup = new Flyout
                    {
                        Placement = PlacementMode.Pointer,
                        Content = new SearchMenuItem(NodeContextTypes, this),
                    };
                    //this.popup = popup;
                }
                popup.ShowAt(this,true);
                //var aa = popup.Focus(NavigationMethod.Click);
            }
        }
        public Hm_Controls.ElTextBox earchStr = null;
        /// <summary>
        /// 清空当前拖放状态
        /// </summary>
        public void ClearState() {
            ParentJoin = null; 
            IsMouseJoin = false;
            bP_Line.SetJoin(null, null);
            //bP_Line.Width = 1f;
            //bP_Line.Height = 1f;
            //bP_Line.Margin = "0,0,auto,auto";
            Canvas.SetLeft(bP_Line, 0);
            Canvas.SetTop(bP_Line, 0);
            //MouseJoin.Margin = "0,0,auto,auto";
            Canvas.SetLeft(MouseJoin, 0);
            Canvas.SetTop(MouseJoin, 0);
            //MouseJoin.MarginTop = -1;
            bP_Line.IsVisible = false;//.Visibility = Visibility.Hidden;
            MouseJoin.IsVisible = false;//Visibility.Hidden;
        }
        /// <summary>
        /// 用于鼠标拖动的接口，本身只是为了拖动 
        /// </summary>
        public IJoinControl MouseJoin;// = new MouseJoin(this,IJoinControl.NodePosition.Left);
        /// <summary>
        /// 用于模拟拖动的线，不参与流程
        /// </summary>
        public BP_Line bP_Line;
        /// <summary>
        /// 上一个选中的接口
        /// </summary>
        public IJoinControl ParentJoin = null;
        /// <summary>
        /// 设置鼠标状态
        /// </summary>
        /// <param name="State"></param>
        public void SetMouseState(IJoinControl State, PointerReleasedEventArgs e1) {
            //IsMouseJoin = false;
            //bP_Line.InvalidateVisual();
            //bP_Line.RefreshDrawBezier();
            if (ParentJoin == null)
            {
                bP_Line.Width = 0;
                bP_Line.Height = 0;
                bP_Line.RefreshDrawBezier();
                ParentJoin = State;
                //Point p = State.GetPosition(State.GetJoinPos(IJoinControl.NodePosition.Left));
                //MouseJoin.MarginLeft = p.X;
                //MouseJoin.MarginTop = p.Y;
                //bP_Line.MarginLeft = p.X;
                //bP_Line.MarginTop = p.Y;
                //Debug.WriteLine($"State.p:{p}");
                //Debug.WriteLine($"e.Location1:{bP_Line.Margin}");
                //Debug.WriteLine($"-----------------------------------------------");
                //bP_Line.SetJoin(null, null);
                if (ParentJoin.GetDir() == IJoinControl.NodePosition.Left)
                {
                    MouseJoin.SetDir(IJoinControl.NodePosition.right);
                    bP_Line.SetJoin(MouseJoin, ParentJoin);
                }
                if (ParentJoin.GetDir() == IJoinControl.NodePosition.right)
                {
                    MouseJoin.SetDir(IJoinControl.NodePosition.Left);
                    bP_Line.SetJoin(ParentJoin, MouseJoin);
                }
                
                //bP_Line.PositionReckon();
                IsMouseJoin = true;
                //MouseJoin.Visibility = Visibility.Visible;
                /*this.Delay(TimeSpan.FromSeconds(1), () => {
                    bP_Line.Visibility = Visibility.Visible;
                });*/
                bP_Line.IsVisible = true;// Visibility.Visible;

            }
            else {
                Point p = State.GetPosition(State.GetJoinPos(IJoinControl.NodePosition.Left));
                //输入输出一样或者父元素一样全部不可连接
                if (ParentJoin.GetDir() == State.GetDir() ||
                    ParentJoin.GetParnt() == State.GetParnt())
                {
                    ClearState();
                    UIElementTool.Toast(bluePrint, "接口不匹配||不能驲自己",p);
                }
                else {
                    //现在就可以初始化线条用于连接
                    //再判断一下两个节点是否已经有线条了

                    IJoinControl a,b;
                    
                    if (ParentJoin.GetDir() == IJoinControl.NodePosition.Left)
                    {
                        a = State;
                        b = ParentJoin;
                    }
                    else {
                        a = ParentJoin;
                        b = State;
                        //bP_Line1.SetJoin(ParentJoin, State);
                    }
                    
                    if (a.GetJoinType() == b.GetJoinType())
                    {
                        //var add = a.GetJoinType();
                        /*if (star.GetNodeType() == Runtime.Token.NodeToken.CallValue || star.GetNodeType() == Runtime.Token.NodeToken.Call)
                        {
                            bP_Line1.backound_color = "rgb(255,255,255)";
                            //执行节点设置为白色执行线
                        }*/
                        if (a.GetJoinType() == typeof(JoinType)|| 
                            a.GetNodeType() == Runtime.Token.NodeToken.CallValue || 
                            a.GetNodeType() == Runtime.Token.NodeToken.Call)
                        {
                            if (!bluePrint.FildIsJoinRef(a) && !bluePrint.FildIsJoinRef(b))
                            {
                                //执行线只支持一对一
                                var bP_Line1 = new BP_Line
                                {
                                    //MarginLeft = 0f,
                                    //MarginTop = 0f,
                                    Width = 1f,
                                    Height = 1f,
                                    //backound_color = "rgb(255,255,255)",
                                };
                                bP_Line1.SetJoin(a, b);
                                
                                bluePrint.AddLineChildren(bP_Line1);
                                
                                //创建完成让他先刷新一下
                                
                                Canvas.SetLeft(bP_Line1, 0);
                                Canvas.SetTop(bP_Line1, 0);
                                bP_Line1.RefreshDrawBezier();
                                bP_Line.InvalidateVisual();
                            }
                            else {
                                UIElementTool.Toast(bluePrint, "流程只支持一对一", p);
                            }
                        }
                        else {
                            //剩下的是普通线条
                            if (!bluePrint.FildLine(a, b))
                            {
                                var bP_Line1 = new BP_Line
                                {
                                    //MarginLeft = 0f,
                                    //MarginTop = 0f,
                                    Width = 1f,
                                    Height = 1f
                                };
                                
                                Canvas.SetLeft(bP_Line1, 0);
                                Canvas.SetTop(bP_Line1, 0);
                                bP_Line1.RefreshDrawBezier();
                                bP_Line.InvalidateVisual();
                                bluePrint.AddLineChildren(bP_Line1);
                                bP_Line1.SetJoin(a, b);
                                ///连上之后，输入禁用操作
                                b.SetEnabled(false);
                                //创建完成让他先刷新一下
                                bP_Line1.RefreshDrawBezier();
                            }
                            else
                            {
                                //已经连过了
                                UIElementTool.Toast(bluePrint, "已经连过了", p);
                            }
                        }
                        
                    }
                    else {
                        //判断接口数据类型
                        if (!a.GetIsTypeCheck() || !b.GetIsTypeCheck())
                        {
                            //接口数据类型不匹配
                            UIElementTool.Toast(bluePrint, "已消除类型检查", p,0.35f);
                            //不进行强类型检测
                            var csharp的goto有限制所以只能复制一份 = "";
                            //剩下的是普通线条
                            if (!bluePrint.FildLine(a, b))
                            {
                                var bP_Line1 = new BP_Line
                                {
                                    //MarginLeft = 0f,
                                    //MarginTop = 0f,
                                    Width = 1f,
                                    Height = 1f
                                };
                                Canvas.SetLeft(bP_Line1, 0);
                                Canvas.SetTop(bP_Line1, 0);
                                bluePrint.AddLineChildren(bP_Line1);
                                bP_Line1.SetJoin(a, b);
                                ///连上之后，输入禁用操作
                                b.SetEnabled(false);
                                //创建完成让他先刷新一下
                                bP_Line1.RefreshDrawBezier();
                            }
                            else
                            {
                                //已经连过了
                                UIElementTool.Toast(bluePrint, "已经连过了", p);
                            }
                        }
                        else {
                            //接口数据类型不匹配
                            var log = $"a:{a.GetJoinType()},b:{b.GetJoinType()}";
                            //test.Log(log);
                            UIElementTool.Toast(bluePrint, "接口数据类型不匹配", p);
                        }
                    }
                    
                    ClearState(); 
                }
                ParentJoin = null;
            }
            //Invalidate();
        }
        /// <summary>
        /// 是否已经点击接口拖动
        /// </summary>
        public bool IsMouseJoin = false;
        /// <summary>
        /// 鼠标当前位置
        /// </summary>
        public Point MousePoint;


        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            //var MousePoint1 = e.GetCurrentPoint(this);
            MousePoint = e.GetCurrentPoint(this).Position;//.Location;
                                                          // 计算缩放后的椭圆中心坐标
            var tf = this.TransformToVisual(bluePrint);
            // 计算缩放后的椭圆中心坐标
            if (tf is Matrix matrix)
            {
                Point scaledCenter = matrix.Transform(MousePoint);
                foreach (var item in bluePrint.GetFocusNodes())
                {
                    if (item is NodeBase node)
                    {
                        node.SetOffset(scaledCenter);
                    }
                }
            }

            if (IsMouseJoin)
            {
                bP_Line.InvalidateVisual();
                bP_Line.RefreshDrawBezier();

                /*var p1 = e.MouseDevice.GetPosition(bluePrint);
                MouseJoin.MarginLeft = p1.X;
                MouseJoin.MarginTop = p1.Y;*/
                
                //Debug.WriteLine($"e.Location11:{p}--{e.Location}");
            }
            //var p1 = e.GetPosition(this);
            //MouseJoin.MarginLeft = p1.X;
            //MouseJoin.MarginTop = p1.Y;
            //Debug.WriteLine($"e.Location11:{MousePoint}");
            Canvas.SetLeft(MouseJoin, MousePoint.X);
            Canvas.SetTop(MouseJoin, MousePoint.Y);



            //if (mousePos.HasValue && e.RightButton == MouseButtonState.Pressed)
            //{
            //    //bluePrint.MarginLeft
            //    var aa = e.Location.X - bluePrint.MarginLeft.Value / scale;
            //    var p = e.Location / scale;
            //    var a = p - mousePos.Value;
            //    Matrix matrix = Matrix.Identity;
            //    if (bluePrint.RenderTransform is MatrixTransform transform)
            //    {
            //        matrix = transform.Value;
            //    }
                
            //    matrix.TranslatePrepend(a.X, a.Y);
            //    System.Diagnostics.Debug.WriteLine(a);
            //    bluePrint.RenderTransform = new MatrixTransform(matrix);
            //    mousePos = p;
            //}
            //Parent.Invalidate();
        }

    }
}
