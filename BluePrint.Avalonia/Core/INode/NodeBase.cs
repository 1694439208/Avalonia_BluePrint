using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.Media;
using System.Diagnostics;
using Avalonia.Markup.Xaml;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Avalonia.Data;
using System.Reflection;
using Avalonia.Controls.Primitives;
using BluePrint.Core.IJoin;

namespace BluePrint.Core.INode
{
    //[NodeBaseInfo("控件基类","基类")]
    public class NodeBase : Panel,Context, Model.IBP_Render
    {
        bool Model.IBP_Render.Render { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        public NodeStyle Style { get; set; } = new NodeStyle();
        private BoxShadows? _defaultBoxShadow;

        /// <summary>
        /// 设计器专用
        /// </summary>
        public NodeBase()
        {
        }
        public NodeBase(BParent _bParent) {
            bParent = _bParent;
        }
        public BParent bParent;
        public List<(IJoinControl, Node_Interface_Data)> _IntPutJoin = new List<(IJoinControl, Node_Interface_Data)>();
        List<(IJoinControl, Node_Interface_Data)> Context.IntPutJoin { 
            get{
                return _IntPutJoin;
            }
            set {
                value = _IntPutJoin;
            }
        }
        public List<(IJoinControl, Node_Interface_Data)> _OutPutJoin = new List<(IJoinControl, Node_Interface_Data)>();
        List<(IJoinControl, Node_Interface_Data)> Context.OutPutJoin {
            get {
                return _OutPutJoin;
            }
            set
            {
                value = _OutPutJoin;
            }
        }

        /// <summary>
        /// 序列化用的id
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 计算节点
        /// </summary>
        /// <param name="Context">执行上下文</param>
        /// <param name="arguments">参数</param>
        /// <param name="result">返回</param>
        public virtual async Task Execute(object Context,List<object> arguments, Runtime.Evaluate.Result result) {
            PlayAnimation();
            //默认动作 ，输出所有连接
            for (int i = 0; i < result.GetNextNodeSize(); i++)
            {
                result.SetExecute(i);
            }
        }
        /// <summary>
        /// 节点状态
        /// </summary>
        NodeState NodeState { set; get; } = NodeState.None;
        /// <summary>
        /// 设置当前节点状态
        /// </summary>
        /// <param name="nodeState"></param>
        /// <param name="title"></param>
        public void SetState(NodeState nodeState,string? title=null) {
            NodeState = nodeState;
            if (!string.IsNullOrEmpty(title))
            {
                ToolTip.SetTip(this, new TextBlock
                {
                    Text = title,
                    Padding = new Thickness(5),
                });
                ToolTip.SetShowDelay(this, 0);
                ToolTip.SetPlacement(this, PlacementMode.Top);
                ToolTip.SetVerticalOffset(this, -6);
            }
            else
            {
                ToolTip.SetTip(this, null);
            }
            if (_defaultBoxShadow == null)
                _defaultBoxShadow = border.BoxShadow;
            switch (nodeState)
            {
                case NodeState.Error:
                    if (Style.ErrorStateColor == null)
                    {
                        border.BoxShadow = _defaultBoxShadow.Value;
                        break;
                    }
                    border.BoxShadow = new BoxShadows(new BoxShadow
                    {
                        Spread = 6,
                        Color = Style.ErrorStateColor.Value,
                        Blur = 3,
                    });

                    break;
                case NodeState.OK:
                    if (Style.OKStateColor == null)
                    {
                        border.BoxShadow = _defaultBoxShadow.Value;
                        break;
                    }
                    border.BoxShadow = new BoxShadows(new BoxShadow
                    {
                        Spread = 6,
                        Color = Style.OKStateColor.Value,
                        Blur = 3,
                    });
                    break;
                case NodeState.Running:
                    if (Style.RunningStateColor == null)
                    {
                        border.BoxShadow = _defaultBoxShadow.Value;
                        break;
                    }
                    border.BoxShadow = new BoxShadows(new BoxShadow
                    {
                        Spread = 6,
                        Color = Style.RunningStateColor.Value,
                        Blur = 3,
                    });
                    break;
                case NodeState.None:
                    border.BoxShadow = _defaultBoxShadow.Value;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 设置动画播放
        /// </summary>
        public virtual void PlayAnimation() {
            foreach (var item in _IntPutJoin.Select(a => bParent.bluePrint.FildIutJoin(a.Item1)).SelectMany(a => a))
            {
                item?.Start_Animation(BP_Line.LineDirection.Output);
            }
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <param name="Execute">下一个流程的代码数组</param>
        /// <param name="PrevNodes">上一个流程的代码数组</param>
        /// <param name="arguments">参数变量名数组</param>
        /// <param name="result">返回的变量数组</param>
        /// <returns></returns>
        public virtual string CodeTemplate(List<string> Execute, List<string> PrevNodes, List<ParameterAST> arguments, List<ParameterAST> result)
        {
            return "";
        }
        /*
         *             
return $@"
if({arguments[0]} > {arguments[1]}){{
    {Execute[0]}
}}else{{
    {Execute[1]}
}}
{result[0]} = {arguments[0]} > {arguments[1]}
";
         * /// <summary>
        /// 计算节点
        /// </summary>
        public virtual void Execute()
        {
            //输入节点  _IntPutJoin 可以直接读取所有输入节点数据
            foreach (var item in _IntPutJoin)
            {
                item.Item1.Render();
            }
            //执行下一个连接节点
            if (_OutPutJoin.Count > 0 && _OutPutJoin[0].Item1.GetJoinType() == typeof(JoinType))
            {
                var line = bParent.bluePrint.FildOutJoin(_OutPutJoin[0].Item1);
                foreach (var item in line)
                {
                    var join = (item.GetEndJoin() as IJoinControl);
                    (join.Get_NodeRef() as Context).Execute();
                }
            }
        }*/
        IObservable<double> Move_X { get; set; }
        IObservable<double> Move_Y { get; set; }

        Subject<double> _moveY = new Subject<double>();
        Subject<double> _moveX = new Subject<double>();


        IObservable<bool?> ischeck { get; set; }
        protected override void OnInitialized()
        {
            //AvaloniaXamlLoader.Load(this);
            Focusable = true;
            //ClipToBounds = true;
            //CornerRadius = "3.8";
            Background = new SolidColorBrush(Color.FromRgb(60, 60, 255));// new SolidColorBrush(Color.FromRgb(35, 38, 35));
            //BorderFill = "#000";
            //BorderStroke = "1";
            //添加标题
            
            //Move_X = _moveX.AsObservable().Scan((acc, val) => {
            //    return acc + val;
            //}); ;
            //Move_Y = _moveY.AsObservable().Scan((acc, val) => acc + val); ;
            //Bind(Canvas.LeftProperty, Move_X);
            //Bind(Canvas.TopProperty, Move_Y);

            var stack = new Grid {};
            // Define the Columns.
            var colDef1 = new ColumnDefinition();
            colDef1.Width = new GridLength(50, GridUnitType.Star);
            var colDef2 = new ColumnDefinition();
            colDef2.Width = new GridLength(50, GridUnitType.Star);

            stack.ColumnDefinitions.Add(colDef1);
            stack.ColumnDefinitions.Add(colDef2);

            InPutIJoin = new StackPanel
            {
                //Orientation = Avalonia.Layout.Orientation.Vertical,// Orientation.Vertical,
                Background = new SolidColorBrush(Colors.Chocolate),
                //Margin = new Thickness(0,0,5,0)
            };
            stack.Children.Add(InPutIJoin);
            Grid.SetColumn(InPutIJoin, 0);
            OuPutIJoin = new StackPanel
            {
                Background = new SolidColorBrush(Colors.Bisque),
                //Margin = new Thickness(5, 0, 0, 0),
                
                //Orientation = Avalonia.Layout.Orientation.Vertical,
                //Width = 200,
                //HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                //VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            };
            stack.Children.Add(OuPutIJoin);
            Grid.SetColumn(OuPutIJoin, 1);
            //Blueprint_Canvas.SetTop(OuPutIJoin, TitleHeight + 5);
            //Blueprint_Canvas.SetLeft(OuPutIJoin, 20);

            border = new Border();
            var title_control = new StackPanel
            {
                Background = new SolidColorBrush(Color.Parse("#B8FFBF19")),// Brushes.Aqua,
                Children =
                {
                    new DockPanel{
                        Background = new SolidColorBrush(Color.Parse("#B8FFBF19")),// Brushes.Aqua,
                        Height = TitleHeight,
                        Children = {
                            new Title
                            {
                                Name = "title",
                                Background = new SolidColorBrush(Color.Parse("#B8FFBF19")),// Brushes.Aqua,
                                title = Title,
                            },
                            new CheckBox
                            {
                                Width = TitleHeight,
                                Height = TitleHeight,
                                IsThreeState = true,
                                Name = "zhedie",
                                Padding = new Thickness(0),
                                //[!CheckBox.IsCheckedProperty] = new Binding("ischeck")
                            }
                        }
                    },
                    stack
                }
            };
            DockPanel.SetDock(title_control.FindViewControl<Title>("title"), Dock.Left);

            var checkbox = title_control.FindViewControl<CheckBox>("zhedie");
            DockPanel.SetDock(checkbox, Dock.Right);

            checkbox.IsCheckedChanged += (s, e) => {
                if (s is CheckBox checkBox)
                {
                    //Debug.Print($"checkBox:{checkBox.IsChecked}");
                    switch (checkBox.IsChecked)
                    {
                        
                        case true:
                            //小折叠
                            for (int i = 0; i < _IntPutJoin.Count; i++)
                            {
                                var item = _IntPutJoin[i];
                                if (!bParent.bluePrint.FildIsJoinRef(item.Item1))
                                {
                                    item.Item1.IsVisible = false;
                                }
                            }
                            for (int i = 0; i < _OutPutJoin.Count; i++)
                            {
                                var item = _OutPutJoin[i];
                                if (!bParent.bluePrint.FildIsJoinRef(item.Item1))
                                {
                                    item.Item1.IsVisible = false;
                                }
                            }
                            break;
                        case false:
                            foreach (var item in _IntPutJoin)
                            {
                                item.Item1.Margin = new Thickness(0);
                                item.Item1.IsVisible = true;
                            }
                            foreach (var item in _OutPutJoin)
                            {
                                item.Item1.Margin = new Thickness(0);
                                item.Item1.IsVisible = true;
                            }
                            break;
                        default:
                            //大折叠 只保留第一个
                            for (int i = 0; i < _IntPutJoin.Count; i++)
                            {
                                if (i != 0)
                                {
                                    var h = _IntPutJoin[i].Item1.Bounds.Height;
                                    _IntPutJoin[i].Item1.Margin = new Thickness(0, -h, 0, 0);
                                    //_IntPutJoin[i].Item1.IsVisible = false;
                                }
                            }
                            for (int i = 0; i < _OutPutJoin.Count; i++)
                            {
                                if (i!=0)
                                {
                                    var h = _OutPutJoin[i].Item1.Bounds.Height;
                                    _OutPutJoin[i].Item1.Margin = new Thickness(0, -h, 0, 0);
                                    //_OutPutJoin[i].Item1.IsVisible = false;
                                }
                            }
                            break;
                    }
                }
            };

            //checkbox.Bind(CheckBox.IsCheckedProperty, ischeck);

            border.Child = title_control;
            border.BoxShadow = new BoxShadows(new BoxShadow
            {
                Color = Colors.Black,
                Blur = 10,
            });
            border.Name = "test";
            Children.Add(border);
            
            RefreshNodes();
            // 触发自定义事件
            RaiseEvent(new RoutedEventArgs(TapEvent));
            //
        }

        public static readonly RoutedEvent<RoutedEventArgs> TapEvent =
            RoutedEvent.Register<NodeBase, RoutedEventArgs>(nameof(OnNodeInitEveTemp), RoutingStrategies.Bubble);

        // Provide CLR accessors for the event
        public event EventHandler<RoutedEventArgs> OnNodeInitEveTemp
        {
            add => AddHandler(TapEvent, value);
            remove => RemoveHandler(TapEvent, value);
        }
        public StackPanel OuPutIJoin;
        public StackPanel InPutIJoin;
        Border border;
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            if (NodeState != NodeState.None)
            {
                return;
            }
            base.OnGotFocus(e);

            var myButton = this.FindControl<Border>("test");
            if (border is Border)
            {
                border.BoxShadow = new BoxShadows(new BoxShadow
                {
                    Color = Colors.Black,
                    Blur = 15,
                });
                //border.BorderBrush = new SolidColorBrush(Color.FromRgb(197, 131, 35));
                //border.BorderThickness = new Thickness(1);
            }

        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (NodeState != NodeState.None)
            {
                return;
            }
            if (border is Border)
            {
                border.BoxShadow = new BoxShadows(new BoxShadow
                {
                    Color = Color.FromRgb(197, 131, 35),
                    Blur = 10,
                });
                //border.BorderBrush = null;// new SolidColorBrush(Color.FromRgb(35, 38, 35));
                //border.BorderThickness = new Thickness(1);
            }
            //base.OnLostFocus(e);
            //BorderFill = Color.FromRgb(35, 38, 35);
            //BorderStroke = "1";
        }
        protected override Control? GetTemplateFocusTarget()
        {
            return base.GetTemplateFocusTarget();
        }

        //protected override void OnLayoutUpdated()
        //{
        //    base.OnLayoutUpdated();
        //    RefreshDrawBezier();
        //    //IsKeyboardFocusWithin
        //}
        public string Title = "";
        public void RefreshNodes()
        {
            
            foreach (var item in _IntPutJoin)
            {
                /*item.Item1.SetType(item.Item2);
                item.Item1.Set(item.Item2);
                item.Item1.Render();
                item.Item1.Attacheds.Add(DockPanel.Dock, Dock.Left);
                InPutIJoin.Children.Add(item.Item1);*/
                AddIntPut(item);
            }
            foreach (var item in _OutPutJoin)
            {
                /*item.Item1.SetType(item.Item2);
                item.Item1.Set(item.Item2);
                item.Item1.Render();
                item.Item1.Attacheds.Add(DockPanel.Dock, Dock.Right);
                OuPutIJoin.Children.Add(item.Item1);*/
                AddOntPut(item);
            }
            InvalidateArrange();
        }
        /// <summary>
        /// 添加输入节点
        /// </summary>
        /// <param name="JoinControl">要插入的元素</param>
        /// <param name="join">指定元素位置插入</param>
        /// <param name="pos">true 之前，false 之后</param>
        /// <param name="IsAddList">是否添加到接口数据列表用于管理</param>
        public void AddIntPut((IJoinControl, Node_Interface_Data) JoinControl, IJoinControl join = null,bool pos = true,bool IsAddList = false) {
            JoinControl.Item1.Index = intput_index++;
            JoinControl.Item1.SetType(JoinControl.Item2);
            JoinControl.Item1.Set(JoinControl.Item2);
            JoinControl.Item1.RenderData();
            //JoinControl.Item1.Attacheds.Add(DockPanel.Dock, Dock.Left);
            DockPanel.SetDock(JoinControl.Item1, Dock.Left);
            if (join != null)
            {
                var index = InPutIJoin.Children.IndexOf(join);
                if (index==-1)
                {
                    throw new Exception("简单来说动态插入的接头要和插入他的元素同一个方向，也就是谁点击插入就是谁，接口列表没有此接口，可能已经删除？");
                }
                if (!pos)
                {
                    index += 1;
                }
                if (IsAddList)
                {
                    _IntPutJoin.Insert(index, JoinControl);
                }
                InPutIJoin.Children.Insert(index, JoinControl.Item1);

            }
            else {
                InPutIJoin.Children.Add(JoinControl.Item1);
            }  
        }
        public void ClearIntPut()
        {
            foreach (var item in InPutIJoin.Children.Where(a => a is IJoinControl).ToArray())
            {
                InPutIJoin.Children.Remove(item);
            }
        }
        public void ClearOntPut()
        {
            foreach (var item in OuPutIJoin.Children.Where(a => a is IJoinControl).ToArray())
            {
                OuPutIJoin.Children.Remove(item);
            }
        }
        int intput_index = 0;
        int output_index = 0;
        /// <summary>
        /// 添加输入节点
        /// </summary>
        /// <param name="JoinControl">要插入的元素</param>
        /// <param name="join">指定元素位置插入</param>
        /// <param name="pos">true 之前，false 之后</param>
        /// <param name="IsAddList">是否添加到接口数据列表用于管理</param>
        public void AddOntPut((IJoinControl, Node_Interface_Data) JoinControl, IJoinControl join = null, bool pos = true, bool IsAddList = false)
        {
            JoinControl.Item1.Index = output_index++;
            JoinControl.Item1.SetType(JoinControl.Item2);
            JoinControl.Item1.Set(JoinControl.Item2);
            JoinControl.Item1.RenderData();
            //JoinControl.Item1.Attacheds.Add(DockPanel.Dock, Dock.Right);
            DockPanel.SetDock(JoinControl.Item1, Dock.Right);
            if (join != null)
            {
                var index = OuPutIJoin.Children.IndexOf(join);
                if (index == -1)
                {
                    throw new Exception("接口列表没有此接口，可能已经删除？");
                }
                if (!pos)
                {
                    index += 1;
                }
                if (IsAddList)
                {
                    _OutPutJoin.Insert(index, JoinControl);
                }
                OuPutIJoin.Children.Insert(index, JoinControl.Item1);

            }
            else
            {
                if (IsAddList)
                {
                    _OutPutJoin.Add(JoinControl);
                }
                OuPutIJoin.Children.Add(JoinControl.Item1);
            }
        }
        int TitleHeight = 25;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            e.Handled = true;
            //节点被按下 储存到 焦点处理
            bParent.bluePrint.AddFocusNode(this);
            //PointerPressedHandler 
            base.OnPointerPressed(e);
            var point = e.GetCurrentPoint(this);
            if (point.Properties.IsLeftButtonPressed)
            {
                if (!IsKeyboardFocusWithin)
                {
                    Focus(NavigationMethod.Pointer);
                }

                Mouxy = point.Position;
                isclick = true;
                (Parent as BluePrint).SelectThisInstance(this);
                //CaptureMouse();
                e.Pointer.Capture(this);
            }
            
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            e.Handled = true;//吃掉鼠标消息不让他冒泡
            bParent.bluePrint.RemoveFocusNode(this);
            base.OnPointerReleased(e);
            
            var point = e.GetCurrentPoint(this);
            if (e.InitialPressMouseButton == MouseButton.Left)
            {
                isclick = false;
                e.Pointer.Capture(null);
                //ReleaseMouseCapture();
            }
        }
        private Point Mouxy;
        private bool isclick = false;

        protected override Size ArrangeOverride(Size finalSize)
        {
            var ret = base.ArrangeOverride(finalSize);
            //RefreshDrawBezier();
            //InvalidateVisual();
            //节点第一次被初始化，先刷新连线
            foreach (var item in _IntPutJoin)
            {
                foreach (var line in bParent.bluePrint.FildIutJoin(item.Item1))
                {
                    line.RefreshDrawBezier();
                }
            }
            foreach (var item in _OutPutJoin)
            {
                foreach (var line in bParent.bluePrint.FildOutJoin(item.Item1))
                {
                    line.RefreshDrawBezier();
                }
            }
            return ret;
        }
        public void SetOffset(Point point) {
            Blueprint_Canvas.SetLeft(this, point.X - Mouxy.X);
            Blueprint_Canvas.SetTop(this, point.Y - Mouxy.Y);
        }
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            var point = e.GetCurrentPoint(this);
            //e.Handled = true;
            if (e == null)
            {
                RefreshDrawBezier();
                //为了刷新溢出了的线条
                //Parent.Parent.Invalidate();

                Blueprint_Canvas.SetLeft(this, Blueprint_Canvas.GetLeft(this) + 1);
                Blueprint_Canvas.SetTop(this, Blueprint_Canvas.GetTop(this) + 1);
                return;
            }
            if (point.Properties.IsLeftButtonPressed && isclick)
            {
                //RefreshDrawBezier();
                //为了刷新溢出了的线条
                //Parent.Parent.Invalidate();
                //var a = point.Position - Mouxy;

                //MarginLeft += a.X;
                //MarginTop += a.Y;
                //var aa = Blueprint_Canvas.GetLeft(this);
                //Debug.Print($"move:{a}");
                //_moveX.OnNext(a.X);
                //Blueprint_Canvas.SetLeft(this, Blueprint_Canvas.GetLeft(this) + a.X);
                //Blueprint_Canvas.SetTop(this, Blueprint_Canvas.GetTop(this) + a.Y);
                // TransformPoint

            }

        }
        /// <summary>
        /// 通知此节点接口线条引用刷新
        /// </summary>
        public void RefreshDrawBezier() {
            foreach (var item in _IntPutJoin)
            {            
                var lines = bParent.bluePrint.FildIutJoin(item.Item1);
                foreach (var line in lines)
                {
                    line.RefreshDrawBezier();
                    line.InvalidateVisual();
                }
            }
            foreach (var item in _OutPutJoin)
            {
                var lines = bParent.bluePrint.FildOutJoin(item.Item1);
                foreach (var line in lines)
                {
                    line.RefreshDrawBezier();
                    line.InvalidateVisual();
                }
            }
        }

        

        public enum NodePosition
        {
            Left, right
        }
    }
}
