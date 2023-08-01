
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using BluePrint.Core.IJoin;

namespace BluePrint.Core.Join
{
    public class SliderJoint : IJoinControl
    {
        public SliderJoint() : base()
        {
        }
        public SliderJoint(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
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
        public override void Set(Node_Interface_Data value)
        {
            sliderDate = value;
            //UINode.Background = value.ToString();
        }
        public override Node_Interface_Data Get()
        {
            return new Node_Interface_Data
            { 
                Title="",
            };
        }
        public Node_Interface_Data sliderDate;
        public Slider UINode = new Slider
        {
            Maximum = 100f,
            Name = "slider",
            //PresenterFor = this,
            //Classes = "el-slider",
            Width = 123.5f,
        };


        protected override void OnInitialized()
        {
            base.OnInitialized();
            base.AddControl(UINode, nodePosition);
        }
    }
}
