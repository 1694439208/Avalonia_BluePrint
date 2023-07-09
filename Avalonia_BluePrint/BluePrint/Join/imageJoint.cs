using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;
using Avalonia;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Avalonia_BluePrint.BluePrint.DataType;

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
            base.Set(value);
            if (value.ClassValue != null && value.ClassValue.TryGetValue("UInNodeSize", out var val))
            {
                var size = val.GetValue<Data_Size>();
                UINode.Width = size.Width;
                UINode.Height = size.Height;
            }
            if (GetJoinType() == typeof(Data_Bitmap)){
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
                try
                {
                    UINode.Background = new ImageBrush(new Bitmap(_value.bitmap_path)); ;// $"url({_value.bitmap_path}) no-repeat fill";
                }
                catch (Exception)
                {
                }
                
            }
        }
        public override Node_Interface_Data Get()
        {
            //_value.bitmap = null;
            return base.Get();
        }
        public Panel UINode = new Panel
        {
            Width = 100,
            Height = 100,
            //BorderFill: rgb(220, 220, 220);
            //BorderStroke: 1;
        };
        private Size _UInNodeSize = default;
        public Size UInNodeSize {
            set {
                UINode.Width = value.Width;
                UINode.Height = value.Height;
                _UInNodeSize = value;
            }
            get
            {
                return _UInNodeSize;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            base.AddControl(UINode, nodePosition);
        }
    }
}
