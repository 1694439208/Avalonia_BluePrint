
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;

namespace 蓝图重制版.BluePrint.Node
{
    public class DateJoint : IJoinControl
    {
        public DateJoint() : base()
        {
        }
        public DateJoint(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
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
        Node_Interface_Data dataDate;
        public override void Set(Node_Interface_Data value)
        {
            dataDate = value;
        }
        public override Node_Interface_Data Get()
        {
            return dataDate;
        }
        public DatePicker UINode = new DatePicker
        {
            Height = 23f,
            //Classes = "el-textbox",
            Width = 110f,
        };


        protected override void OnInitialized()
        {
            base.OnInitialized();
            base.AddControl(UINode, nodePosition);
        }
    }
}
