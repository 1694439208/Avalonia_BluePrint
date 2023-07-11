
//using Avalonia.Controls;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using 蓝图重制版.BluePrint;
//using 蓝图重制版.BluePrint.DataType;
//using 蓝图重制版.BluePrint.IJoin;
//using 蓝图重制版.BluePrint.INode;
//using 蓝图重制版.BluePrint.Node;
//using 蓝图重制版.BluePrint.Runtime;

//namespace Document.Join
//{
//    public class JoinxlsSelect: IJoinControl
//    {
//        public JoinxlsSelect() : base()
//        {
//        }
//        public JoinxlsSelect(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node,Token.NodeToken.Value)
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
//            if (value.ClassValue != null && value.ClassValue.TryGetValue("Enabled", out var val))
//            {
//                Enabled = (bool)val;
//            }
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
        
//        protected override void OnInitialized()
//        {
//            base.OnInitialized();
//            //为了方便就固定了状态
//            //SetIsConnectState(false);
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
//                                             Extensions = "xls,xlsx",
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
//                                        //var excel = ExcelHelper.ExcelForeach(path);
//                                        //title.Value = excel;
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
