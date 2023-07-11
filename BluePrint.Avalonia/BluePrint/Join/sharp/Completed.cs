
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;
using System.Linq;
using Avalonia.Controls;
using ReactiveUI;
using Newtonsoft.Json.Linq;
using Avalonia.Data;
using Avalonia;

namespace 蓝图重制版.BluePrint.Node
{
    public class Completed : IJoinControl
    {
        public Completed() : base()
        {
            this.DataContext = new ViewModel();
        }
        public Completed(BParent _bParent, NodePosition JoinDir, Control Node) :base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
        {
            nodePosition = JoinDir;
            this.DataContext = new ViewModel();
        }
        public class ViewModel : ReactiveObject
        {
            private int _SelectedIndex = 0;
            public int SelectedIndex
            {
                get => _SelectedIndex;
                set => this.RaiseAndSetIfChanged(ref _SelectedIndex, value);
            }
            public void SelectionChanged()
            {
                //Greeting = "Another greeting from Avalonia";
            }
        }


        public NodePosition nodePosition;
        public override void SetDir(NodePosition value)
        {
            nodePosition = value;
        }
        public override NodePosition GetDir()
        {
            return nodePosition;
        }
        public Node_Interface_Data __value;
        public override void Set(Node_Interface_Data value)
        {
            if (DataContext is ViewModel model)
            {
                model.SelectedIndex = Convert.ToInt32(value.Value);
            }
            __value = value;
            /*
            if (GetJoinType() == typeof(Data_Bitmap))
            {
                text1.Text = (__value.Value as Data_Bitmap).Title;
            }
            else if (GetJoinType() == typeof(bool))
            {
                text1.Text = __value.Value.ToString();
            }
            else if (GetJoinType() == typeof(string))
            {
                text1.Text = __value.Value.ToString();
            }
            else if (GetJoinType() == typeof(IEnumerable<string>))
            {
                text1.Text = "列表数据";
            }
            else {
                text1.Text = __value.Value.ToString();
            }*/
            
        }
        public override Node_Interface_Data Get()
        {
            if (DataContext is ViewModel model)
            {
                __value.Value = model.SelectedIndex;
            }
            
            return __value;
        }
        public static Dictionary<string, (Type,string)> ObjectTypeDic = new Dictionary<string, (Type, string)>() {
            {"文本",(typeof(string),"string") },
            //{"图片",(typeof(System.Drawing.Bitmap),"System.Drawing.Bitmap") },
            {"布尔",(typeof(bool),"bool") },
            {"数字",(typeof(float),"float") },
            {"整数",(typeof(int),"int") },
            {"日期时间",(typeof(DateTime),"DateTime") },
            {"列表",(typeof(List<object>),"List<string>") },
            {"词典",(typeof(Dictionary<object,object>),"Dictionary<string,object>") },
            {"表格",(typeof(DataTable),"DataTable") },
            {"动态对象",(typeof(object),"") },
        };
       
        /*protected override void Initial0izeComponent()
        {
            Children.Add(new Border { 
                MarginLeft = 0,
                MarginTop = "auto",
                Width = 10,
                Height = 10,
                BorderType = BorderType.BorderThickness,
                BorderThickness = new Thickness(1,1, 1, 1),
                BorderFill = "red",
                Padding = "10,10,10,10",
            });
           
            //Background = Color.FromRgb(81, 137, 255);
        }*/
        protected override void OnInitialized()
        {
             ComboBox text1 = new ComboBox
             {
                 Width = 80f,
                 //Items = ObjectTypeDic.Keys.ToItems(),
                 //SelectedIndex = 0,
                 //Foreground = Color.FromRgb(255, 255, 255),
             };
            text1.Bind(ComboBox.SelectedIndexProperty, new Binding("SelectedIndex"));
            text1.SelectionChanged +=(s,e)=> {
                var key = ((ComboBox)s).SelectedItem?.ToString();
                if (key != null)
                {
                    if (ObjectTypeDic.ContainsKey(key))
                    {
                        __value.Type = ObjectTypeDic[key].Item1;
                        SetType(__value);
                    }
                }
            };
            foreach (var item in ObjectTypeDic.Keys)
            {
                text1.Items.Add(item);
            }
            
            base.OnInitialized();
            base.AddControl(text1, nodePosition);
        }
    }
}
