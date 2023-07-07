
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;

namespace 蓝图重制版.BluePrint.Node
{
    public class TextJoint : IJoinControl
    {
        public TextJoint() : base()
        {
        }
        public TextJoint(BParent _bParent, NodePosition JoinDir, Control Node) :base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
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
        public override Type GetJoinType()
        {
            return base.GetJoinType();
        }
        Node_Interface_Data textDate;
        public override void Set(Node_Interface_Data value)
        {
            textDate = value;
            text1.Text = textDate.Title;
        }
        public override Node_Interface_Data Get()
        {
            return textDate;
        }
        public TextBlock text1 = new TextBlock
        {
            Text = "文本a11111111",
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
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
            base.AddControl(text1, nodePosition);
        }
    }
}
