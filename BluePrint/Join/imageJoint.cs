using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;
using Avalonia;

namespace 蓝图重制版.BluePrint.Node
{
    public class ImageJoint : IJoinControl
    {
        public ImageJoint() : base()
        {
        }
        public ImageJoint(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
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
        public Data_Bitmap _value;
        public override void Set(Node_Interface_Data value)
        {
            if(GetJoinType() == typeof(Data_Bitmap)){
                _value = (Data_Bitmap)value.Value;
            }     
        }
        public override void Render()
        {
            if (_value.bitmap != null)
            {
                UINode.Background = new ImageBrush(_value.bitmap);
            }
            else {
                UINode.Background = new ImageBrush(new Bitmap(_value.bitmap_path)); ;// $"url({_value.bitmap_path}) no-repeat fill";
            }
        }
        public override Node_Interface_Data Get()
        {
            return new Node_Interface_Data {
                Type = typeof(Data_Bitmap),
                Value = _value,
            };
        }
        public Panel UINode = new Panel
        {
            Width = 100,
            Height = 100,
            //BorderFill: rgb(220, 220, 220);
            //BorderStroke: 1;
        };
        public Size UInNodeSize {
            set {
                UINode.Width = value.Width;
                UINode.Height = value.Height;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            base.AddControl(UINode, nodePosition);
        }
    }
}
