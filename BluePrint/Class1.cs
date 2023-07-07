using CPF;
using CPF.Animation;
using CPF.Charts;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using CPF.Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 蓝图重制版.BluePrint
{
    [CPF.Design.DesignerLoadStyle("res://蓝图重制版.BluePrint/Stylesheet1.css")]//用于设计的时候加载样式
    public class Class1 : Control
    {
        //模板定义
        protected override void InitializeComponent()
        {
            var B_Join = new Border
            {
                Width = 10,
                Height = 10,
                BorderType = BorderType.BorderThickness,
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderFill = "red",
                Padding = "10,10,10,10",
            };
            B_Join.Attacheds.Add(DockPanel.Dock, Dock.Right);
            var B_StackPanel = new DockPanel
            {
                MarginRight = 0,//Orientation = Orientation.Horizontal,
                Children =
                {
                    B_Join,
                    new TextBlock
                    {
                        Text = "666"
                    },
                },
            };
            //Children.Add(B_StackPanel);
            Padding = "10,10,10,10";
            var a = DockPanel.Dock.GetAttachedPropertyName();
            var stack = new DockPanel
            {
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Attacheds =
                        {
                            {
                                DockPanel.Dock,
                                Dock.Left
                            }
                        },
                        MarginTop = 0,
                        MarginRight = 20,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "CPF控件1",
                            },
                            new TextBlock
                            {
                                Text = "CPF控件1",
                            },
                            new TextBlock
                            {
                                Text = "CPF控件1",
                            },
                        },
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Attacheds =
                        {
                            {
                                DockPanel.Dock,
                                Dock.Right
                            }
                        },
                        MarginTop = 0,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "CPF控件1",
                            },
                            new TextBlock
                            {
                                Text = "CPF控件1",
                            },
                            B_StackPanel,
                            new DockPanel
                            {
                                
                                MarginRight = 0,//Orientation = Orientation.Horizontal,
                                Children =
                                {
                                    new Border
                                    {
                                        Width = 10,
                                        Height = 10,
                                        BorderType = BorderType.BorderThickness,
                                        BorderThickness = new Thickness(1, 1, 1, 1),
                                        BorderFill = "red",
                                        Padding = "10,10,10,10",
                                        Attacheds = {
                                            { DockPanel.Dock,Dock.Right},
                                        },
                                    },
                                    new TextBox
                                    {
                                        Text = "666"
                                    },
                                },
                            },
                        },
                    },
                }
            };
            Children.Add(new Slider
            {
                Maximum = 100f,
                Name = "slider",
                PresenterFor = this,
                Classes = "el-slider",
                Width = 123.5f,
            });
        }
    }
}
