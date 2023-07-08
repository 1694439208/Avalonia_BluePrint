
using System;
using System.Collections.Generic;
using System.Text;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint.Node;

using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Media;

namespace 蓝图重制版.BluePrint.IJoin
{
    
    public class ExecJoin : IJoinControl
    {
        public ExecJoin() : base()
        {
        }
        public ExecJoin(BParent _bParent, NodePosition JoinDir, Control Node) : base(_bParent, JoinDir,Node, Runtime.Token.NodeToken.Call)
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
            //Width = 5,
        };

        public Node_Interface_Data title;
        
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var b = GetJoinRef();
            b.BorderThickness = new Thickness(1);
            b.Width = 20;
            b.Height = 20;
            //b.Child = new SVG
            //{
            //    Triggers =
            //    {
            //        {
            //            nameof(SVG.IsMouseOver),
            //            Relation.Me,
            //            null,
            //            (nameof(SVG.Fill),"#aaa")
            //        }
            //    },
            //    ToolTip = title.Value,
            //    IsAntiAlias = true,
            //    Fill = "#FFFFFF",
            //    Size = "16,16",
            //    Stretch = Stretch.Uniform,
            //    Source = "<svg ><path d=\"m0,0l133.09092,0l110.90908,129.85546l-110.90908,129.85545l-133.09092,0l0,-259.71091z\" p-id=\"1199\"></path></svg>"
            //};
            b.Child = new Panel
            {
                Background = Brushes.RoyalBlue,
                Width = 16,
                Height = 16,
            };
            ToolTip.SetTip(b.Child, title.Value);
            if (IsButton)
            {
                UINode = new Button
                {
                    Width = 80f,
                    Content = "动态执行"
                };
                (UINode as Button).Click += (s, e) =>
                {
                    //(_Node as Context).Execute();
                    var a = new Runtime.NodeParse(bParent);
                    var ast = a.Parser(_Node as NodeBase);
                    Runtime.Evaluate.Eval(ast, null);
                    //var code = Runtime.CodeGenerator.Generator(ast);

                    //System.Diagnostics.Debug.WriteLine(code);
                    //ToSZArray

                    //CPF.Skia.SkiaPdf.CreatePdf(Root,"蓝图.pdf");
                };


            }
            base.AddControl(UINode, nodePosition);
        }
    }
}
