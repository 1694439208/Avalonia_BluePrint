
//using Avalonia.Controls;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BluePrint;
//using BluePrint.DataType;
//using BluePrint.IJoin;
//using BluePrint.INode;
//using BluePrint.Node;
//using BluePrint.Runtime;

//namespace Document.Join
//{
//    public class JoinFileSelect: IJoinControl
//    {
//        public JoinFileSelect() : base()
//        {
//        }
//        public JoinFileSelect(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node,Token.NodeToken.Value)
//        {
//            nodePosition = JoinDir;
//            _Node = (NodeBase)Node;
//            bParent = _bParent;
//        }
//        BParent bParent;
//        public NodePosition nodePosition;
//        NodeBase _Node;
//        public override void SetDir(NodePosition value)
//        {
//            nodePosition = value;
//        }
//        public override NodePosition GetDir()
//        {
//            return nodePosition;
//        }
//        public override void Set(Node_Interface_Data value)
//        {
//            title = value;
//        }
//        public override Node_Interface_Data Get()
//        {
//            return title;
//        }
//        public Control UINode = new Panel
//        {
//            Width = 20f,
//        };

//        public Node_Interface_Data title;

//        public event JoinEvent OnJoinEveTemp
//        {
//            add { AddHandler(value); }
//            remove { RemoveHandler(value); }
//        }
        
//        protected override void InitializeComponent()
//        {
//            base.InitializeComponent();
//            //为了方便就固定了状态
//            //SetIsConnectState(false);

//            var svg = new SVG
//            {
//                Triggers =
//                {
//                    {
//                        nameof(SVG.IsMouseOver),
//                        Relation.Me,
//                        null,
//                        (nameof(SVG.Fill),"#aaa")
//                    }
//                },
//                Commands =
//                {
//                    {
//                        nameof(SVG.MouseUp),
//                        (s,e)=>{
//                            this.RaiseEvent(new JoinEventType(){
//                                eveType=EveType.MouseUp,
//                                Value = e,
//                            },nameof(OnJoinEveTemp));
//                        }
//                    },
//                },
//                IsHitTestVisible = true,
//                ToolTip = title?.Value,
//                IsAntiAlias = true,
//                Fill = "#FFFFFF",
//                Size = "16,16",
//                Stretch = Stretch.Uniform,
//                Source = "<svg ><path d=\"m0,29.08312l29.08312,0l0,-29.08312l29.83376,0l0,29.08312l29.08312,0l0,29.83376l-29.08312,0l0,29.08312l-29.83376,0l0,-29.08312l-29.08312,0l0,-29.83376z\" p-id=\"1199\"></path></svg>"
//            };
//            //svg.RaiseEvent(1, nameof(SVG.MouseUp));

//            /*var b = base.GetJoinRef();
//            b.BorderThickness = new Thickness(0, 0, 0, 0);
//            b.Width = 16;
//            b.Height = 16;
//            b.Child = svg;*/

//            UINode = new StackPanel { 
//                Children = {
//                    new Label{
//                        Foreground = "255,255,255",
//                        MinWidth = 100,
//                        Bindings = {
//                            {nameof(Label.Text),nameof(name),this },
//                            {nameof(Label.ToolTip),nameof(path),this }
//                        }
//                    },
//                    new Button
//                    {
//                        Width = 60f,
//                        Content = "选择文件",
//                        //Foreground = "255,255,255",
//                        //TextAlignment = CPF.Drawing.TextAlignment.Center,
//                        Commands =
//                        {
//                            {nameof(Button.Click),async (s,e)=>{
//                                OpenFileDialog openFileDialog = new OpenFileDialog()
//                                {
//                                    //格式筛选器 (选项名 | 格式 ; 选项名 | 格式 , 格式)
//                                    Filters = new List<FileDialogFilter>{
//                                        new FileDialogFilter
//                                        {
//                                             Extensions = "*",
//                                             Name = "全部类型"
//                                        }
//                                    },
//                                    AllowMultiple = false,
//                                };
//                                var flie_list = await openFileDialog.ShowAsync(Window.Windows.FirstOrDefault());
//                                this.Invoke(()=>{
//                                    path = flie_list.FirstOrDefault();
//                                    if (path!=null){
//                                        name = Path.GetFileName(path);
//                                        title.Value = File.ReadAllBytes(path);
//                                        //RenderLine(this);
//                                        _Node.RefreshDrawBezier();
//                                    }
//                                });
                                
//                                //this.Parent.Invalidate();
//                            }}
//                        }
//                    }
//                },
//            };
//            base.AddControl(UINode, nodePosition);
//        }
//        protected override void OnLayoutUpdated()
//        {
//            base.OnLayoutUpdated();
//            BeginInvoke(new Action(() => {
//                _Node.RefreshDrawBezier();
//            }));
//        }
//        public string path
//        {
//            get { return (string)GetValue(); }
//            set { SetValue(value); }
//        }
//        public string name
//        {
//            get { return (string)GetValue(); }
//            set { SetValue(value); }
//        }
//    }
//}
