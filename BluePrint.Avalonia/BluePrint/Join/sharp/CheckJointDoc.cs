using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint.Node;
using 蓝图重制版.BluePrint.Runtime;

namespace Document.Join
{
    
    public class CheckJointDoc : IJoinControl
    {
        public CheckJointDoc() : base()
        {
            this.DataContext = new ViewModel();
        }
        public CheckJointDoc(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node, Token.NodeToken.Value)
        {
            nodePosition = JoinDir;
            this.DataContext = new ViewModel();
        }

        public class ViewModel : ReactiveObject
        {
            private bool _IsChecked = false;
            public bool IsChecked
            {
                get => _IsChecked;
                set => this.RaiseAndSetIfChanged(ref _IsChecked, value);
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
        Node_Interface_Data dataDate;
        public override void Set(Node_Interface_Data value)
        {
            UINode.Content = value.Title;
            dataDate = value;
            RenderData();
        }
        public override Node_Interface_Data Get()
        {
            dataDate.Value = (DataContext as ViewModel)?.IsChecked??false;
            return dataDate;
        }
        public override void RenderData()
        {
            if (GetJoinType() == typeof(bool))
            {
                UINode.IsChecked = (bool)dataDate.Value;
                //UINode.Content = dataDate.Title;
            }
        }
        public CheckBox UINode = new CheckBox
        {
            Content = "布尔值",
            //Foreground = "255,255,255",
        };
        
        protected override void OnInitialized()
        {
            //UINode.Bindings.Add(nameof(CheckBox1.IsChecked), nameof(IsChecked), this, BindingMode.TwoWay);
            //UINode.Checked += UINode_Checked;
            UINode.Bind(CheckBox.IsCheckedProperty,new Binding("IsChecked"));
            base.OnInitialized();
            base.AddControl(UINode, nodePosition);
        }


    }
}
