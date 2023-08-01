
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;
using BluePrint.Core.IJoin;

namespace BluePrint.Core.Join
{
    public class Checkjoin : IJoinControl
    {
        public Checkjoin() : base()
        {
        }
        public Checkjoin(BParent _bParent, NodePosition JoinDir, Control Node) :base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
        {
            nodePosition = JoinDir;
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
            __value = value;
            text1.IsChecked = (bool)__value?.Value;
            text1.Content = __value.Value.ToString();
        }
        public override Node_Interface_Data Get()
        {
            return __value;
        }
        public CheckBox text1 = new CheckBox
        {
            Content = "",
            //Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
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
            base.OnInitialized();
            text1.Click += (s,e) => {
                __value.Value = text1.IsChecked??false;
                text1.Content = __value.Value.ToString();
            };
            //VisualChildren.Add(new Panel
            //{
            //    Width = 10,
            //    Height = 10,
            //    //BorderThickness = new Thickness(1, 1, 1, 1),
            //    //BorderBrush = Brushes.Red,
            //    //Padding = new Thickness(10)
            //});
            base.AddControl(text1, nodePosition);
        }
    }
}
