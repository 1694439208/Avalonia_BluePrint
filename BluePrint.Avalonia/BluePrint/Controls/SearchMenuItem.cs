using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Hm_Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 蓝图重制版.BluePrint.Controls;
using 蓝图重制版.BluePrint;
using 蓝图重制版.BluePrint.IJoin;
using ReactiveUI;
using Avalonia.Media;
using Color = Avalonia.Media.Color;
using DynamicData;
using Avalonia.Data;
using System.Xml.Linq;
using Avalonia.VisualTree;
using OAvalonia = Avalonia;

namespace BluePrint.Avalonia.BluePrint.Controls
{
    public class ViewModel : ReactiveObject
    {
        private ObservableCollection<TreeViewItem> _Nodes;
        public ObservableCollection<TreeViewItem> Nodes
        {
            get => _Nodes;
            set => this.RaiseAndSetIfChanged(ref _Nodes, value);
        }
    }

    internal class SearchMenuItem : Panel
    {
        List<Type> NodeTypes = new List<Type>();
        Dictionary<string, List<(NodeBaseInfoAttribute, Type)>> valuePairs = new Dictionary<string, List<(NodeBaseInfoAttribute, Type)>>();
        public SearchMenuItem(List<Type> nodetypes, BParent bParent)
        {
            NodeTypes = nodetypes;
            parent = bParent;
            this.DataContext = new ViewModel();
        }
        private BParent parent;

        public void SetItems()
        {
            if (NodeTypes != null)
            {
                foreach (var item in NodeTypes)
                {
                    var args = item.GetCustomAttributes(typeof(NodeBaseInfoAttribute), false);
                    if (args != null && args.Length > 0)
                    {
                        var NodeBaseInfo = (args[0] as NodeBaseInfoAttribute);
                        if (valuePairs.ContainsKey(NodeBaseInfo.NodeGroup))
                        {
                            valuePairs[NodeBaseInfo.NodeGroup].Add((NodeBaseInfo, item));
                        }
                        else
                        {
                            valuePairs.Add(NodeBaseInfo.NodeGroup, new List<(NodeBaseInfoAttribute, Type)>() {
                                (NodeBaseInfo, item),
                            });
                        }
                    }
                }
                SetNodes(valuePairs);
                /*this.Delay(TimeSpan.FromSeconds(1),()=> {
                    Debug.WriteLine(tree.GetChildren().Count);
                    Debug.WriteLine(tree.Items.Count);
                });*/

            }
        }
        public void SetNodes(Dictionary<string, List<(NodeBaseInfoAttribute, Type)>> valuePair)
        {
            if (DataContext is ViewModel model)
            {
                if (model.Nodes == null)
                {
                    model.Nodes = new ObservableCollection<TreeViewItem>();
                }
                //model.Nodes = new List<TreeViewItem>();
                model.Nodes.Clear();
                foreach (var item in valuePair)
                {
                    var treev1 = new TreeViewItem
                    {
                        Header = item.Key,
                        Foreground = new SolidColorBrush(Color.FromArgb(221, 221, 221, 200)),
                    };
                    foreach (var item1 in item.Value)
                    {
                        treev1.Items.Add(new TreeViewItem
                        {
                            Header = item1.Item1.NodeName,
                            Tag = item1.Item2,
                            Foreground = new SolidColorBrush(Color.FromArgb(221, 221, 221, 200)),
                        });
                    }
                    model.Nodes.Add(treev1);
                }
            }
            
        }
        /// <summary>
        /// 展开所有节点
        /// </summary>
        public void Open()
        {
            //var tree = this.FindControl<TreeView>("TreeView1");
            // 遍历树并展开所有节点
            foreach (var node in tree.Items)
            {
               tree.ExpandSubTree(node as TreeViewItem);
            }
        }
        /// <summary>
        /// 关闭所有节点
        /// </summary>
        public void close()
        {
            //var tree = FindPresenterByName<TreeView>("TreeView1");
            //foreach (var item in tree.AllItems())
            //{
            //    item.IsExpanded = false;
            //}
        }
        TreeView tree;
        protected override void OnInitialized()
        {
            //_popup = Parent as Flyout;
            
            Width = 280;
            Height = 380;
            Background = new SolidColorBrush(Color.FromArgb(255,38, 38, 38));
            //BorderType = BorderType.BorderThickness;
            //BorderThickness = new Thickness(1);
            //BorderFill = "0,0,0";
            //CornerRadius = "3,3,3,3";
            var stack = new Grid { };
            // Define the Columns.
            var colDef1 = new RowDefinition();
            colDef1.Height = new GridLength(1, GridUnitType.Auto);
            var colDef2 = new RowDefinition();
            colDef2.Height = new GridLength(1, GridUnitType.Auto);
            var colDef3 = new RowDefinition();
            colDef3.Height = new GridLength(50, GridUnitType.Star);

            stack.RowDefinitions.Add(colDef1);
            stack.RowDefinitions.Add(colDef2);
            stack.RowDefinitions.Add(colDef3);

            var c1 = new CheckBox
            {
                Content = "情景关联",
                Foreground = new SolidColorBrush(Color.FromArgb(221, 221, 221, 200)),
                FontSize = 15,
            };
            DockPanel.SetDock(c1,Dock.Right);

            var c2 = new TextBlock
            {
                Text = "此蓝图的所有操作",
                Foreground = new SolidColorBrush(Color.FromArgb(221, 221, 221, 200)),
                FontSize = 15,
                VerticalAlignment = OAvalonia.Layout.VerticalAlignment.Center
            };
            DockPanel.SetDock(c2, Dock.Left);

            var p1 = new DockPanel
            {
                Children =
                {
                    c1,
                    c2
                },
                //Background = new SolidColorBrush(Color.FromRgb(60,50,30)),
                VerticalAlignment = OAvalonia.Layout.VerticalAlignment.Center,
                Margin = new Thickness(3)
            };
            Grid.SetRow(p1,0);
            stack.Children.Add(p1);


            var p2 = new Panel
            {
                Children =
                {
                    new TextBox
                    {
                        Name = "SearchElTextBox",
                        Watermark = "搜索",
                    },
                    new TextBlock
                    {
                        Name = "closetext",
                        //Classes = "el-icon,el-icon-close",
                        //MarginRight =0f,
                        Width = 20,
                        Foreground = new SolidColorBrush(Color.FromArgb(255,0,0,0)),
                    },
                },
            };
            var SearchElTextBox = p2.TemporaryFix<TextBox>("SearchElTextBox");


            //var textBox1 = p2.FindViewControl<TextBox>("SearchElTextBox");
            if (SearchElTextBox is TextBox)
            {
                SearchElTextBox.TextChanged += (s, e) => {
                    //(s as TextBox).Text
                    //         //Debug.WriteLine((s as ElTextBox).Text);
                    var data = ((TextBox)s).Text;
                    if (data == string.Empty || data == "")
                    {
                        SetNodes(valuePairs);
                        return;
                    }
                    var ret = valuePairs.Where(a => a.Value.Any(b => b.Item1.NodeName.IndexOf(data) != -1))
                    .Select(a =>
                    {
                        return new KeyValuePair<string, List<(NodeBaseInfoAttribute, Type)>>(
                            a.Key,
                            a.Value.Where(b => b.Item1.NodeName.IndexOf(data) != -1).ToList()
                        );
                    });
                    var reta = ret.ToDictionary(a => a.Key, b => b.Value);//string,List<(IJoin.NodeBaseInfoAttribute,Type)>
                    SetNodes(reta);
                    Open();
                };
            }
            

            Grid.SetRow(p2,1);
            stack.Children.Add(p2);


            tree = new TreeView
            {
                DataContext = DataContext,
                Name = "TreeView1",
                //Background = new SolidColorBrush(Color.FromArgb(255,60,80,90)),
                /*Items =
                {
                    new TreeViewItem{ 
                        Header = "666"
                    },
                    new TreeViewItem{
                        Header = "666",
                        Items = { new TreeViewItem{
                        Header = "666"
                    }}
                    }
                },*/
                [!TreeView.ItemsSourceProperty] = new Binding("Nodes", BindingMode.Default)
                //Size= SizeField.Fill,
                //DisplayMemberPath=nameof(NodeData.Text),
                //ItemsMemberPath=nameof(NodeData.Nodes),
                //Bindings = {
                //    {nameof(TestTreeView.Items),nameof(Nodes), this}
                //},
                //Commands = {
                //    {nameof(TreeView.ItemMouseUp),(s,e)=>{
                //        var itemview = e as TreeViewItemMouseEventArgs;
                //        if (itemview.Item is SearchTreeViewItem && itemview.Item.Tag!=null)
                //        {

                //            parent.CreateNode((Type)itemview.Item.Tag,parent.MousepanelPupopPos.MarginLeft.Value,parent.MousepanelPupopPos.MarginTop.Value);
                //             //var Control = System.Activator.CreateInstance((Type)itemview.Item.Tag,new object[]{ parent});
                //             //parent.bluePrint.AddChildren((CPF.Controls.Control)Control);
                //            _popup.Visibility = Visibility.Hidden;
                //        }
                //    }}
                //},
            };
            tree.SelectionChanged += (s,e) => {
                if (s is TreeView itemview)
                {
                    if (itemview.SelectedItem is TreeViewItem tree1)
                    {
                        if (tree1.Tag != null)
                        {
                            parent.CreateNode((Type)tree1.Tag, Canvas.GetLeft(parent.MousepanelPupopPos), Canvas.GetTop(parent.MousepanelPupopPos));
                            //var Control = System.Activator.CreateInstance((Type)itemview.Item.Tag,new object[]{ parent});
                            //parent.bluePrint.AddChildren((CPF.Controls.Control)Control);
                            parent.popup?.Hide();
                        }
                    }
                }

            };
            var p3 = new Panel
            {
                Children =
                {
                    tree
                },
            };
            Grid.SetRow(p3, 2);
            stack.Children.Add(p3);
            Children.Add(stack);

            
            //this.Triggers.Add(nameof(IsMouseOver), Relation.Me, null, (nameof(Background), "#fff"));
            SetItems();

            //parent.earchStr = FindPresenterByName<ElTextBox>("SearchElTextBox");
        }
    }
}
