
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 蓝图重制版.BluePrint;
using 蓝图重制版.BluePrint.DataType;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint.Node;
using 蓝图重制版.BluePrint.Runtime;

namespace Document.Join
{
    public class JoinTextView : IJoinControl
    {
        public JoinTextView() : base()
        {
        }
        public JoinTextView(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node,Token.NodeToken.Value)
        {
            nodePosition = JoinDir;
            _Node = (NodeBase)Node;
            bParent = _bParent;
        }
        BParent bParent;
        public NodePosition nodePosition;
        NodeBase _Node;
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
            if (value.ClassValue != null && value.ClassValue.TryGetValue("IsEnabledd", out var val))
            {
                IsEnabledd = val.GetValue<bool>();
            }
            switch (value.Value)
            {
                case byte[]:
                    data = Encoding.Default.GetString((byte[])value?.Value);
                    break;
                default:
                    data = value?.Value?.ToString();
                    break;
            }
            title = value;
        }
        public override Node_Interface_Data Get()
        {
            title.Value = data;
            return title;
        }
        public Control UINode = new Panel
        {
            Width = 20f,
        };

        public Node_Interface_Data title;

        
        protected override void OnInitialized()
        {
            base.OnInitialized();
            //为了方便就固定了状态
            //SetIsConnectState(false);


            UINode = new StackPanel { 
                Children = {
                    new TextBox{
                        //Background="255,255,255",
                        Width = 300,
                        MinHeight = 90,
                        MaxHeight = 300,
                        //WordWarp = true,
                        //MaxWidth = 160,
                        ////IsReadOnly = true,
                        //Bindings = {
                        //    { nameof(TextBox.Text),nameof(data),this,BindingMode.TwoWay}
                        //},
                    }
                },
            };
            base.AddControl(UINode, nodePosition);
        }
        public string data;
    }
}
