
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint.Node;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Avalonia.Controls;
using Avalonia;

namespace 蓝图重制版.BluePrint.IJoin
{
    
    public class GeneratorJoin : IJoinControl
    {
        public GeneratorJoin() : base()
        {
        }
        public GeneratorJoin(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir,Node, Runtime.Token.NodeToken.Call)
        {
            nodePosition = JoinDir;
            _Node = Node;
            bParent = _bParent;
        }
        BParent bParent;


        //public override Control Get_NodeRef() { return base.Get_NodeRef(); }
        public bool IsButton = false;
        public NodePosition nodePosition;
        Control _Node;
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
            if (value.ClassValue!=null&&value.ClassValue.TryGetValue("IsButton", out var val))
            {
                IsButton = (bool)val;
            }
            title = value;
        }
        public override Node_Interface_Data Get()
        {
            return title;
        }
        public Control UINode = new Panel {
            Width = 20f,
        };

        public Node_Interface_Data title;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var b = base.GetJoinRef();
            b.BorderThickness = new Thickness(0, 0, 0, 0);
            b.Width = 16;
            b.Height = 16;
            b.Child = new Panel
            {
                Width = 16,
                Height = 16,
            };

            if (IsButton)
            {
                UINode = new Button
                {
                    Width = 60f,
                    Content = "一键生成",
                };
                (UINode as Button).Click += (s, e) => {
                    //(_Node as Context).Execute();
                    var a = new Runtime.NodeParse(bParent,false);
                    var ast = a.Parser(_Node as NodeBase);
                    var code = Runtime.CodeGenerator.Generator(ast);
                    //Runtime.Evaluate.Eval(ast,null);
                    //var code = Runtime.CodeGenerator.Generator(ast);
                    System.Diagnostics.Debug.WriteLine(code);
                    //ToSZArray
                    //new dialog2(Window.Windows.FirstOrDefault(), code);
                    //CPF.Skia.SkiaPdf.CreatePdf(Root,"蓝图.pdf");
                };
            }
            base.AddControl(UINode, nodePosition);
        }
    }
}
