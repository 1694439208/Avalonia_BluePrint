﻿
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint.Node;

namespace 蓝图重制版.BluePrint.IJoin
{
    public class AddCustomJoinEventArgs : RoutedEventArgs
    {
        public DataType.JoinEventType Message { get; set; }

        public AddCustomJoinEventArgs(RoutedEvent routedEvent, DataType.JoinEventType message) : base(routedEvent)
        {
            Message = message;
        }
    }
    public class AddCustomJoin: IJoinControl
    {
        public AddCustomJoin() : base()
        {
        }
        public AddCustomJoin(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.None)
        {
            nodePosition = JoinDir;
            _Node = Node;
            bParent = _bParent;
        }
        //public override Control Get_NodeRef() { return base.Get_NodeRef(); }
        public NodePosition nodePosition;
        Control _Node;
        BParent bParent;
        public override void SetDir(NodePosition value)
        {
            nodePosition = value;
        }
        public override NodePosition GetDir()
        {
            return nodePosition;
        }
        public override void Set(Node_Interface_Data value)
        {
            title = value;
        }
        public override Node_Interface_Data Get()
        {
            return title;
        }
        public Control UINode = new Panel
        {
            Width = 20f,
        };

        public Node_Interface_Data title;

        public static readonly RoutedEvent<RoutedEventArgs> TapEvent =
            RoutedEvent.Register<AddExecJoin, RoutedEventArgs>(nameof(OnJoinEveTemp), RoutingStrategies.Bubble);

        // Provide CLR accessors for the event
        public event EventHandler<AddExecJoinEventArgs> OnJoinEveTemp
        {
            add => AddHandler(TapEvent, value);
            remove => RemoveHandler(TapEvent, value);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            //为了方便就固定了状态
            SetIsConnectState(false);
            //var svg = new SVG
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
            //    Commands =
            //    {
            //        {
            //            nameof(SVG.MouseUp),
            //            (s,e)=>{
            //                this.RaiseEvent(new DataType.JoinEventType(){
            //                    eveType=DataType.EveType.MouseUp,
            //                    Value = e,
            //                },nameof(OnJoinEveTemp));
            //            }
            //        },
            //    },
            //    IsHitTestVisible = true,
            //    ToolTip = title?.Value,
            //    IsAntiAlias = true,
            //    Fill = "#FFFFFF",
            //    Size = "16,16",
            //    Stretch = Stretch.Uniform,
            //    Source = "<svg ><path d=\"m0,29.08312l29.08312,0l0,-29.08312l29.83376,0l0,29.08312l29.08312,0l0,29.83376l-29.08312,0l0,29.08312l-29.83376,0l0,-29.08312l-29.08312,0l0,-29.83376z\" p-id=\"1199\"></path></svg>"
            //};
            var svg = new Panel
            {
                //Triggers =
                //{
                //    {
                //        nameof(SVG.IsMouseOver),
                //        Relation.Me,
                //        null,
                //        (nameof(SVG.Fill),"#aaa")
                //    }
                //},
                IsHitTestVisible = true,
                Width = 16,
                Height = 16,
                Background = Brushes.DarkBlue,
                //ToolTip = title?.Value,
                //IsAntiAlias = true,
                //Fill = "#FFFFFF",
                //Size = "16,16",
                //Stretch = Stretch.Uniform,
                //Source = "<svg ><path d=\"m0,29.08312l29.08312,0l0,-29.08312l29.83376,0l0,29.08312l29.08312,0l0,29.83376l-29.08312,0l0,29.08312l-29.83376,0l0,-29.08312l-29.08312,0l0,-29.83376z\" p-id=\"1199\"></path></svg>"
            };
            svg.PointerReleased += (s, e) =>
            {
                // 触发自定义事件
                RaiseEvent(new AddExecJoinEventArgs(TapEvent, new DataType.JoinEventType()
                {
                    eveType = DataType.EveType.MouseUp,
                    Value = e,
                }));
                //this.RaiseEvent(, nameof(OnJoinEveTemp));
            };
            //svg.RaiseEvent(1, nameof(SVG.MouseUp));

            var b = base.GetJoinRef();
            b.BorderThickness = new Thickness(1);
            b.Width = 20;
            b.Height = 20;
            b.Child = svg;

            /*if (OnJoinEveTemp != null)
            {
                OnJoinEveTemp(this, EveType.MouseUp, e);
            }*/
            //b.Child.RaiseEvent(e, nameof(DoubleClick));
            UINode = new TextBlock
            {
                MaxWidth = 60f,
                Text = title.Title,
                Foreground = Brushes.Black,// "255,255,255",
                TextAlignment = Avalonia.Media.TextAlignment.Center,// CPF.Drawing.TextAlignment.Center,
            };
            base.AddControl(UINode, nodePosition);

            OnJoinEveTemp += (s, e) =>
            {
                var temp = e.Message as DataType.JoinEventType;
                if (temp.eveType == DataType.EveType.MouseUp)
                {
                    //_OutPutJoin.find
                    if (_Node is NodeBase nodeBase)
                    {
                        if (GetDir() == NodePosition.Left)
                        {
                            nodeBase.AddIntPut((new TextJoint(bParent, IJoinControl.NodePosition.Left, _Node), new Node_Interface_Data
                            {
                                Title = "输出",
                                Tips = "输出",
                                Value = "",
                                Type = typeof(string),
                                IsTypeCheck = false,
                            }), s as IJoinControl, IsAddList: true);
                        }
                        if (GetDir() == NodePosition.right)
                        {
                            nodeBase.AddOntPut((new TextJoint(bParent, IJoinControl.NodePosition.right, _Node), new Node_Interface_Data
                            {
                                Title = "输出",
                                Tips = "输出",
                                Value = "",
                                Type = typeof(string),
                                IsTypeCheck = false,
                            }), s as IJoinControl, IsAddList: true);
                        }
                    }
                }
            };
        }
    }
}
