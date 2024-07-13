
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using BluePrint.Core.IJoin;

namespace BluePrint.Core.Join
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
        public string Watermark
        {
            set
            {
                UINode.Watermark = value;
            }
        }

        public bool Multiline { get; set; }
        public bool PasswordMode { get; set; }


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
            base.Set(value);
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
            if (value.ClassValue != null && value.ClassValue.TryGetValue("Height", out var height))
            {
                UINode.Height = Convert.ToSingle(height.GetValue<double>());
            }
            if (value.ClassValue != null && value.ClassValue.TryGetValue("Multiline", out var multiline))
            {
                if (multiline.GetValue<bool>())
                {
                    UINode.AcceptsReturn = true;
                    UINode.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
                }
                else
                    UINode.TextWrapping = Avalonia.Media.TextWrapping.NoWrap;
            }
            if (value.ClassValue != null && value.ClassValue.TryGetValue("PasswordMode", out var passwordMode))
            {
                if (passwordMode.GetValue<bool>())
                    UINode.PasswordChar = '*';
                else
                    UINode.PasswordChar = new char();
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
            UINode.Text = value.Value.ToString();
            ;
        }
        public override Node_Interface_Data Get()
        {
            base.Get().Value = Convert.ChangeType(UINode.Text, GetJoinType());
            return base.Get();
        }
        public double width = 90f;
        public TextBox UINode = new TextBox
        {
            Width = 90f,
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
                Children = { UINode },
            }, nodePosition);
        }
    }
}
