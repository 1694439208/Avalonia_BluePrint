using Avalonia.Controls;
using System.Collections.Generic;
using System;
using ��ͼ���ư�.BluePrint.INode;
using ��ͼ���ư�.BluePrint;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;

namespace Avalonia_BluePrint.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DockPanel stackPanel = new DockPanel()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            };

            var bp = new BParent
            {
                Margin = new Avalonia.Thickness(0, 0, 0, 0),
                //HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,// HorizontalAlignment.Stretch
            };
            //bp.Background = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(bp, 0);
            Canvas.SetTop(bp, 0);


            //NodeTypes.Add(typeof(_StartNode));
            //NodeTypes.Add(typeof(Branch));
            //���ýڵ�������
            bp.SetContext(new List<Type>{
                    typeof(_StartNode),
                    typeof(Branch),
                    typeof(ImageShow),
                    typeof(ImageSplit),
                    typeof(sequence),
                    typeof(ThanGreater),
                    typeof(ScriptType),
                    typeof(CreateVar),
                    typeof(GetVar),
                    typeof(Print),
                    typeof(ThanEqual),
                    typeof(ThanLess),
                    typeof(ThanStrEqual),
                });
            bp.Initialized += (s, e) => {
                var node = new _StartNode(bp)
                {
                };
                bp.bluePrint.AddChildren(node);
                var node2 = new sequence(bp)
                {
                };
                bp.bluePrint.AddChildren(node2);
                Canvas.SetLeft(node2, 0);
                Canvas.SetTop(node2, 0);
                Canvas.SetLeft(node, 0);
                Canvas.SetTop(node, 0);
                int x = 200;
                int y = 30;
                for (int i = 0; i < 100; i++)
                {
                    x += 100;
                    if (i % 15 == 0)
                    {
                        y += 100;
                        x = 200;
                    }
                    var node1 = new Branch(bp);
                    bp.bluePrint.AddChildren(node1);
                    Canvas.SetLeft(node1, x);
                    Canvas.SetTop(node1, y);
                    var line = new BP_Line
                    {
                        Width = 1f,
                        Height = 1f
                    };
                    //bp.bluePrint.AddChildren(line);
                    bp.bluePrint.AddLineChildren(line);
                    line.SetJoin(node._OutPutJoin[0].Item1, node1._IntPutJoin[0].Item1);
                    //line.RefreshDrawBezier();
                }

            };
            stackPanel.Children.Add(bp);

            this.Content = stackPanel;
        }

        //public static WindowNotificationManager? _manager;
        //protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        //{
        //    base.OnApplyTemplate(e);
        //    _manager = new WindowNotificationManager(this) { MaxItems = 3 };
        //}
    }
}