
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;
using static 蓝图重制版.BluePrint.Runtime.Token;

namespace 蓝图重制版.BluePrint.Node
{
    public class IJoinControl : Panel,BP_IJoin
    {
        
        /*/// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="uIElement"></param>
        public void Add(UIElement uIElement) {
            Children.Add(uIElement);
        }
        /// <summary>
        /// 接口的提示控件
        /// </summary>
        public ToastControl toast;*/
        /// <summary>
        /// 指示接口是否可以连线
        /// </summary>
        public bool IsConnect = true;
        /// <summary>
        /// 序列化用的id
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 执行排序下标
        /// </summary>
        public int Index { set; get; }
        /// <summary>
        /// 设置接口状态
        /// </summary>
        /// <param name="isc"></param>
        public void SetIsConnectState(bool isc) {
            IsConnect = isc;
        }
        /// <summary>
        /// 读取接口状态
        /// </summary>
        /// <returns></returns>
        public bool GetIsConnectState()
        {
            return IsConnect;
        }
        /// <summary>
        /// 设置接口方向
        /// </summary>
        /// <param name="position"></param>
        public virtual void SetDir(NodePosition position) { _position = position; }
        /// <summary>
        /// 获取接口方向，默认左边
        /// </summary>
        /// <returns></returns>
        public virtual NodePosition GetDir() { return _position; }
        public enum NodePosition
        {
            Left, right
        };
        /// <summary>
        /// 设计器使用的构造函数
        /// </summary>
        public IJoinControl()//为了设计器用的
        {
        }
        NodeToken _nodeToken = NodeToken.Call;
        public NodeToken GetNodeType() {
            return _nodeToken;
        }
        public void SetNodeType(NodeToken nodeToken)
        {
            _nodeToken = nodeToken;
        }
        public IJoinControl(BParent _bParent, NodePosition position,Control Node) {
            bParent = _bParent;
            _position = position;
            _Node = Node;
        }
        public IJoinControl(BParent _bParent, NodePosition position, Control Node, NodeToken nodeToken)
        {
            bParent = _bParent;
            _position = position;
            _Node = Node;
            _nodeToken = nodeToken;
        }
        /// <summary>
        /// 获取父元素节点引用
        /// </summary>
        /// <returns></returns>
        public virtual Control Get_NodeRef() { return _Node; }
        Control _Node;
        BParent bParent;
        public NodePosition _position;
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            e.Handled = true;
        }
        /// <summary>
        /// 接口数据类型
        /// </summary>
        private Type JoinType;
        /// <summary>
        /// 是否强类型检查
        /// </summary>
        private bool IsTypeCheck;
        /// <summary>
        /// 设置接口数据类型
        /// </summary>
        /// <param name="type"></param>
        public virtual void SetType(Node_Interface_Data type) {
            JoinType = type.Type;
            IsTypeCheck = type.IsTypeCheck;
            if (type.Tips != "")
            {
                ToolTip.SetTip(this, type.Tips);
            }
        }
        /// <summary>
        /// 读取接口数据类型
        /// </summary>
        /// <param name="type"></param>
        public virtual Type GetJoinType()
        {
            return JoinType;
        }
        public virtual bool GetIsTypeCheck()
        {
            return IsTypeCheck;
        }
        Node_Interface_Data _Data;
        /// <summary>
        /// 设置接口数据
        /// </summary>
        /// <param name="value"></param>
        public virtual void Set(Node_Interface_Data value) {
            _Data.Type = _Data.Value?.GetType();
            _Data = value;
        }

        /// <summary>
        /// 读取接口数据
        /// </summary>
        /// <returns></returns>
        public virtual Node_Interface_Data Get() {
            _Data.Type = _Data.Value?.GetType();
            return _Data;
        }

        /// <summary>
        /// 设置接口参数Name
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetNmae(string name) { __Nmae = name; }

        /// <summary>
        /// 读取接口数据
        /// </summary>
        /// <returns></returns>
        public virtual string GetName() { return __Nmae; }
        /// <summary>
        /// 接口==变量  变量名称
        /// </summary>
        public string __Nmae = "";
        /// <summary>
        /// 渲染数据内容
        /// </summary>
        public virtual void Render() {}
        
        public Control GetParnt()
        {
            return _Node;
        }
        public Point GetPos(bool IsZ)
        {
            //if (IsZ)
            //{
            //    return TransformPoint(new Point(0,ActualSize.Height / 2));
            //}
            //return TransformPoint(new Point(ActualSize.Width, ActualSize.Height / 2));
            return default;
        }
        /// <summary>
        /// 获取接头的坐标
        /// </summary>
        /// <returns></returns>
        public Point GetJoinPos(IJoinControl.NodePosition dir)
        {
            if (dir == NodePosition.Left)
            {
                return new Point(0,this.Bounds.Height/2);// BoundsB_Join.TransformPoint(new Point(0, B_Join.Height.Value / 2));
            }
            else {
                return new Point(this.Bounds.Width, this.Bounds.Height/2); ;// B_Join.TransformPoint(new Point(B_Join.Width.Value, B_Join.Height.Value / 2));
            }
        }
        public Control GetThis()
        {
            return this;
        }

        double Interface_size = 20;
        //FloatField Interface_top = "auto";

        DockPanel B_StackPanel;
        Border B_Join;
        /// <summary>
        /// 读取接头图标元素引用
        /// </summary>
        /// <returns></returns>
        public Border GetJoinRef() {
            return B_Join;
        }
        
        protected override void OnInitialized()
        {
            ContextMenu = new ContextMenu()
            {
                //Width = 100,
            };

            var deljoin = new MenuItem
            {
                //Classes = "ContextMenu1",
                Header = "断开连接",
            };
            deljoin.Click += (s, e) => {
                if (_position == NodePosition.Left)
                {
                    var lines = this.bParent.bluePrint.FildIutJoin(this);
                    foreach (var item in lines)
                    {
                        this.bParent.bluePrint.RemoveLine(item);

                    }
                }
                else
                {
                    var lines = this.bParent.bluePrint.FildOutJoin(this);
                    foreach (var item in lines)
                    {
                        this.bParent.bluePrint.RemoveLine(item);

                    }
                }
            };
            ContextMenu.Items.Add(deljoin);
            // 创建一个 MenuItem 对象，并将其添加到 ContextMenu 中
            var menuItem = new MenuItem();
            menuItem.Header = "操作";

            var delline = new MenuItem
            {
                Header = "删除当前节点",
            };
            delline.Click += (s, e) => {
                this.bParent.bluePrint.RemoveNode(_Node);
                this.bParent.ClearState();
            };
            menuItem.Items.Add(delline);
            ContextMenu.Items.Add(menuItem);

            Canvas.SetTop(this,3);
            //Classes = { "IJoinControl"};
            B_Join = new Border
            {
                Width = Interface_size,
                Height = Interface_size,
                //BorderType = BorderType.BorderThickness,
                BorderThickness = new Thickness(1),
                Background = Brushes.RosyBrown,
                //BorderFill = "red",
                //Size = "16,16",
                //Background = Brushes.Red,
                //BorderBrush = Brushes.Red,
                //Padding = "10,10,10,10",
                //Child = new Image
                //{
                //    Triggers =
                //    {
                //        {
                //            nameof(SVG.IsMouseOver),
                //            Relation.Me,
                //            null,
                //            (nameof(SVG.Fill),"#aaa")
                //        }
                //    },
                //        IsAntiAlias = true,
                //        Fill = "136,214,56",
                //        Size = "16,16",
                //        Stretch = Stretch.Uniform,
                //        Source = "<svg><path d=\"m1,104l0,0c0,-56.88532 46.11468,-103 103,-103l0,0c27.31729,0 53.51575,10.85175 72.83198,30.16799c19.31625,19.31625 30.16802,45.5147 30.16802,72.832l0,0c0,56.88532 -46.11468,103 -103,103l0,0c-56.88532,0 -103,-46.11468 -103,-103zm51.5,0l0,0c0,28.44265 23.05735,51.5 51.5,51.5c28.44267,0 51.5,-23.05734 51.5,-51.5c0,-28.44265 -23.05734,-51.5 -51.5,-51.5l0,0c-28.44265,0 -51.5,23.05735 -51.5,51.5z\" p-id=\"1199\"></path></svg>"
                //},
            };
            B_Join.PointerPressed += (s,e) => {
                (s as Border).BorderBrush = new SolidColorBrush(Color.Parse("#d4d4d4")) ;
            };
            B_Join.PointerReleased += (s, e) => {
                (s as Border).BorderBrush = Brushes.Red;
                //(s as Border).ReleaseMouseCapture();
                //(s as Border).CaptureMouse();
                if (e.InitialPressMouseButton == MouseButton.Left)
                {
                    if (GetIsConnectState())//如果接口禁止连线那就不发送事件 用于禁止连线
                    {
                        bParent.SetMouseState(this, e);
                    }
                    Debug.WriteLine("抬起鼠标");
                }
                if (e.InitialPressMouseButton == MouseButton.Right)
                {
                    if (ContextMenu!=null)
                    {
                        ContextMenu.Open();
                    }
                }
            };
            B_StackPanel = new DockPanel
            {
                //Orientation = Orientation.Horizontal,
                Children = { B_Join },
            };
            Children.Add(B_StackPanel);


            if (_position == NodePosition.Left)
            {
                //B_Join.Attacheds.Add(DockPanel.Dock, Dock.Left);
                DockPanel.SetDock(B_Join, Dock.Left);
                Canvas.SetLeft(B_Join, 0);
                //B_Join.MarginLeft = 0;
            }
            else {
                DockPanel.SetDock(B_Join, Dock.Right);
                //B_Join.Attacheds.Add(DockPanel.Dock, Dock.Right);
                //B_Join.MarginRight = 0;
            }
        }
        /// <summary>
        /// 设置控件连线之后可视true代表连接之后继续显示但禁用，反之false
        /// </summary>
        /// <returns></returns>
        public bool Enabled = true;
        /// <summary>
        /// 设置是否允许控件禁用
        /// </summary>
        /// <returns></returns>
        public bool IsEnabledd = true;
        /// <summary>
        /// 设置控件禁用
        /// </summary>
        /// <param name="ise">false禁用 true反之</param>
        public void SetEnabled(bool ise) {
            if (IsEnabledd)
            {
                Body.IsEnabled = ise;
                if (Enabled)
                {
                    return;
                }
            }
            /*if (ise)
            {
                Body.Visibility = Visibility.Visible;
            }
            else {
                //Body.Visibility = Visibility.e;
            }*/
            
        }
        private Control Body;
        public void AddControl(Control control,NodePosition position) {
            if (control!=null)
            {
                Body = control;
                B_StackPanel.Children.Add(control);
                //B_StackPanel.InvalidateVisual();
                //B_StackPanel.InvalidateArrange();
                
                //control.InvalidateVisual();
            }
            if (position == NodePosition.Left)
            {
                //MarginLeft = 0;
                Canvas.SetLeft(this, 0);
            }
            else {
                //MarginRight = 0;
                Canvas.SetRight(this, 0);
            }
            //InvalidateVisual();
            //Children.Add(control);
        }

        public virtual Dictionary<string, object> Dump()
        {
            return new Dictionary<string, object>();
        }

        public virtual void Load(Dictionary<string, object> data)
        {
            if (data!=null)
            {
                foreach (var item in data)
                {
                    //this[item.Key]= item.Value;
                    var property = AvaloniaPropertyRegistry.Instance.FindRegistered(item.Value.GetType(), item.Key);
                    SetValue(property, item.Value);
                }
            }
        }

        public delegate void JoinEvent(EveType eveType, object eventArgs);
        
        
    }
}
