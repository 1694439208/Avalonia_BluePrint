
using Avalonia.Controls;
using Hm_Controls;
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.IJoin;

namespace 蓝图重制版.BluePrint.Node
{
    public class TextBoxJoint : IJoinControl
    {

        public TextBoxJoint() : base()
        {
        }
        public TextBoxJoint(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir, Node, Runtime.Token.NodeToken.Value)
        {
            nodePosition = JoinDir;
        }
        /// <summary>
        /// 水印
        /// </summary>
        public string Watermark {
            set {
                UINode.Watermark = value;
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
        Node_Interface_Data textBoxDate;
        public override void Set(Node_Interface_Data value)
        {
            if (value.ClassValue != null && value.ClassValue.TryGetValue("Watermark", out var val))
            {
                Watermark = val.GetValue<string>();
            }
            if (value.ClassValue != null && value.ClassValue.TryGetValue("Enabled", out var val1))
            {
                Enabled = val1.GetValue<bool>();
            }
            if (value.ClassValue != null && value.ClassValue.TryGetValue("Width", out var width))
            {
                UINode.Width = Convert.ToSingle(width.GetValue<double>());
            }
            //this[nameof(MinWidth)] = (this, nameof(ActualSize), a => (FloatField)((Size)a).Width);
            //this.SetPropretyValue("ad","123");ObjectTypeDic[key].Item2;
            //自动绑定属性
            /*foreach (var item in value.ClassValue)
            {
                if (this is CpfObject)
                {
                    if (this.HasProperty(item.Key))
                    {
                        this.SetValue(item.Value,item.Key);
                    }
                }
            }*/

            textBoxDate = value;
            UINode.Text = textBoxDate.Value.ToString();
;
        }
        public override Node_Interface_Data Get()
        {
            textBoxDate.Value = Convert.ChangeType(UINode.Text, GetJoinType());
            return textBoxDate;
        }
        public double width = 90f;
        public ElTextBox UINode = new ElTextBox
        {
            Width = 90f,
            ClipToBounds = true,
            Text = "test",
        };
        
        protected override void OnInitialized()
        {
            base.OnInitialized();
            //UINode.Width = width;
            base.AddControl(new Panel
            {
                //Classes = "el-textbox",
                //Width = "100%",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                //Height = 27.1f,
                //MarginTop = 10,
                Children = {UINode},
            }, nodePosition);
        }
    }
}
