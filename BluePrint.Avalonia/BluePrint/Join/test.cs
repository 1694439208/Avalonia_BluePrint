
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using BluePrint.Avalonia.BluePrint.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;

namespace 蓝图重制版.BluePrint.Node
{
    public class test : IJoinControl
    {
        public test() : base()
        {
        }
        public test(BParent _bParent, NodePosition JoinDir, Control Node) :base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
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
        }
        public override Node_Interface_Data Get()
        {
            return __value;
        }
        public TagCloud text1 = new TagCloud
        {
            Width = 300,
            Height = 300,
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
