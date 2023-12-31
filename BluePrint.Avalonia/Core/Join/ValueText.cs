﻿
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using BluePrint.Core.IJoin;

namespace BluePrint.Core.Join
{
    public class ValueText : IJoinControl
    {
        public ValueText() : base()
        {
        }
        public ValueText(BParent _bParent, NodePosition JoinDir, Control Node) :base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
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
            //if (__value == null)
            //{
            //    __value = value;
            //}
            //__value.Value = value.Value;
            __value = value;
            RenderData();
            //if (GetJoinType() == typeof(Data_Bitmap))
            //{
            //    text1.Text = (__value.Value as Data_Bitmap).Title1;
            //}
            //else if (GetJoinType() == typeof(bool))
            //{
            //    text1.Text = __value.Value.ToString();
            //}
            //else if (GetJoinType() == typeof(string))
            //{
            //    text1.Text = __value.Value.ToString();
            //}
            //else if (GetJoinType() == typeof(IEnumerable<string>))
            //{
            //    text1.Text = "列表数据";
            //}
            //else {

            //}
        }
        public override Node_Interface_Data Get()
        {
            return __value;
        }
        public override void RenderData()
        {
            text1.Text = __value?.Tips?.ToString() ?? "";
            ToolTip.SetTip(text1, __value?.Value?.ToString() ?? "");
        }
        public TextBlock text1 = new TextBlock
        {
            Text = "文本a11111111",
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
