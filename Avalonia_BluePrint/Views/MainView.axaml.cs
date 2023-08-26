using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json;
using System.IO;
using Avalonia.Controls.Notifications;
using System.Threading.Tasks;
using Avalonia.PrintToPDF;
using Avalonia.Controls.Templates;
using Avalonia_BluePrint.Nodes;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Primitives;
using System.Text;
using BluePrint.Core.IJoin;
using BluePrint.Core;
using BluePrint.Node.Common;
using SkiaSharp;
using BluePrint.Core.Node;
using Avalonia.OpenGL;
using HarfBuzzSharp;
using System;
using Avalonia_BluePrint.ViewModels;
using BluePrint.Core.INode;
using static OpenAI.ObjectModels.Models;
using Avalonia;

namespace Avalonia_BluePrint.Views
{
    public partial class MainView : UserControl
    {
        private WindowNotificationManager _manager;
        private TopLevel? _topLevel;
        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _topLevel = TopLevel.GetTopLevel(this);
            _manager = new WindowNotificationManager(_topLevel) { MaxItems = 3 };
            UIElementTool._manager = _manager;
            bp.RegisterCommonNode();
            bp.RegisterNode(typeof(AINode));
        }

        private async Task ShowSaveFileAsync(string extension, Stream cstream)
        {
            var storage = _topLevel?.StorageProvider;
            if (storage == null)
                return;
            var ret = await storage.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "选择保存文件目录",
                DefaultExtension = extension,
                FileTypeChoices = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("蓝图")
                        {
                            Patterns = new[] { extension }
                        }
                    }
            });

            if (ret != null)
            {
                using var stream = await ret.OpenWriteAsync();
                cstream.CopyTo(stream);
                _manager?.Show(new Notification("提示", "保存成功", NotificationType.Success));
            }
        }
        public async Task LoadDartnet() {
            try
            {
                var storage = _topLevel?.StorageProvider;
                if (storage == null)
                    return;
                var select = await storage.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = "选择加载cfg文件",
                    AllowMultiple = false,
                    FileTypeFilter = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("dartnet")
                        {
                            Patterns = new[] { "*.cfg" }
                        }
                    }
                });
                var selectFile = select?.FirstOrDefault();
                if (selectFile != null)
                {
                    var stream =  File.ReadAllText(selectFile.Path.LocalPath);//.OpenReadAsync();
                    var cfg = Parser.parse_network_cfg(stream);//@"I:\CAFFE\C++\YOLO3\tiny.cfg"
                                                                           //var nodes =  Parser.CreateLayer(cfg);
                    var x = 0;
                    var y = 0;
                    var width = 60;
                    List<KeyValuePair<int, KeyValuePair<int, IJoinControl>>> keyValues = new List<KeyValuePair<int, KeyValuePair<int, IJoinControl>>>();
                    List<NodeBase> layers = new List<NodeBase>();
                    for (int i = 0; i < cfg.Count; i++)
                    {
                        var indexa = 1;
                        var nodeInterface = bp.bp.CreateNode(typeof(sequence), x, y) as NodeBase;
                        x += width;
                        if (nodeInterface == null)
                        {
                            continue;
                        }


                        //反序列化创建的 把他输入输出都删掉重新创建
                        nodeInterface._IntPutJoin.Clear();
                        nodeInterface.ClearIntPut();
                        nodeInterface._OutPutJoin.Clear();
                        nodeInterface.ClearOntPut();

                        layers.Add(nodeInterface);
                        if (cfg[i].Key == "[route]")
                        {
                            var layerss = cfg[i].Value["layers"].Trim().Split(',');
                            if (layerss.Length == 1)
                            {
                                int laye = int.Parse(layerss[0]);
                                var layout1 = layers[i + laye]._OutPutJoin.FirstOrDefault();
                                //给这个层加个输出
                                indexa++;
                                var join = bp.bp.CreateJoin(typeof(BluePrint.Core.Join.ValueText), IJoinControl.NodePosition.Left, nodeInterface);
                                keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(indexa, new KeyValuePair<int, IJoinControl>(indexa, join)));
                                nodeInterface._IntPutJoin.Add((join, new Node_Interface_Data
                                {
                                    Title = "层参数",
                                    Value = "",
                                    Type = typeof(object),
                                    Tips = "层参数",
                                    IsTypeCheck = false,
                                }));

                                //执行线只支持一对一
                                var bP_Line1 = new BP_Line(bp.bp.bluePrint)
                                {
                                    Width = 1f,
                                    Height = 1f,
                                    //backound_color = "rgb(255,255,255)",
                                };
                                Blueprint_Canvas.SetLeft(bP_Line1, 0);
                                Blueprint_Canvas.SetTop(bP_Line1, 0);

                                bp.bp.bluePrint.AddLineChildren(bP_Line1);
                                bP_Line1.SetJoin(layout1.Item1, join);
                                //创建完成让他先刷新一下
                                bP_Line1.RefreshDrawBezier();
                            }
                            if (layerss.Length == 2)
                            {
                                int laye1 = int.Parse(layerss[0]);
                                int laye2 = int.Parse(layerss[1]);

                                var layout1 = layers[i + laye1]._OutPutJoin.FirstOrDefault();

                                var layout2 = layers[laye2]._OutPutJoin.FirstOrDefault();

                                //给这个层加个输出
                                indexa++;
                                var join = bp.bp.CreateJoin(typeof(BluePrint.Core.Join.ValueText), IJoinControl.NodePosition.Left, nodeInterface);
                                keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(indexa, new KeyValuePair<int, IJoinControl>(indexa, join)));
                                nodeInterface._IntPutJoin.Add((join, new Node_Interface_Data
                                {
                                    Title = "层参数",
                                    Value = "",
                                    Type = typeof(object),
                                    Tips = "层参数",
                                    IsTypeCheck = false,
                                }));

                                //执行线只支持一对一
                                var bP_Line1 = new BP_Line(bp.bp.bluePrint)
                                {
                                    Width = 1f,
                                    Height = 1f,
                                    //backound_color = "rgb(255,255,255)",
                                };
                                Blueprint_Canvas.SetLeft(bP_Line1, 0);
                                Blueprint_Canvas.SetTop(bP_Line1, 0);

                                bp.bp.bluePrint.AddLineChildren(bP_Line1);
                                bP_Line1.SetJoin(layout1.Item1, join);
                                //创建完成让他先刷新一下
                                bP_Line1.RefreshDrawBezier();

                                //给这个层加个输出
                                indexa++;
                                var join1 = bp.bp.CreateJoin(typeof(BluePrint.Core.Join.ValueText), IJoinControl.NodePosition.Left, nodeInterface);
                                keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(indexa, new KeyValuePair<int, IJoinControl>(indexa, join1)));
                                nodeInterface._IntPutJoin.Add((join1, new Node_Interface_Data
                                {
                                    Title = "层参数",
                                    Value = "",
                                    Type = typeof(object),
                                    Tips = "层参数",
                                    IsTypeCheck = false,
                                }));

                                //执行线只支持一对一
                                var bP_Line11 = new BP_Line(bp.bp.bluePrint)
                                {
                                    Width = 1f,
                                    Height = 1f,
                                    //backound_color = "rgb(255,255,255)",
                                };
                                Blueprint_Canvas.SetLeft(bP_Line11, 0);
                                Blueprint_Canvas.SetTop(bP_Line11, 0);

                                bp.bp.bluePrint.AddLineChildren(bP_Line11);
                                bP_Line11.SetJoin(layout2.Item1, join1);
                                //创建完成让他先刷新一下
                                bP_Line11.RefreshDrawBezier();
                            }
                        }
                        else
                        {
                            indexa++;
                            var join1 = bp.bp.CreateJoin(typeof(BluePrint.Core.Join.ValueText), IJoinControl.NodePosition.Left, nodeInterface);
                            keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(indexa, new KeyValuePair<int, IJoinControl>(indexa, join1)));
                            nodeInterface._IntPutJoin.Add((join1, new Node_Interface_Data
                            {
                                Title = "输入参数",
                                Value = "",
                                Type = typeof(object),
                                Tips = "输入参数",
                                IsTypeCheck = false,
                            }));

                            indexa++;
                            var join2 = bp.bp.CreateJoin(typeof(BluePrint.Core.Join.ValueText), IJoinControl.NodePosition.right, nodeInterface);
                            keyValues.Add(new KeyValuePair<int, KeyValuePair<int, IJoinControl>>(indexa, new KeyValuePair<int, IJoinControl>(indexa, join2)));
                            nodeInterface._OutPutJoin.Add((join2, new Node_Interface_Data
                            {
                                Title = "输出参数",
                                Value = "",
                                Type = typeof(object),
                                Tips = "输出参数",
                                IsTypeCheck = false,
                            }));
                        }
                        nodeInterface.SetTitle(cfg[i].Key);
                        nodeInterface.RefreshNodes();
                    }

                    //var BPObject = JsonConvert.DeserializeObject<BParent.BPByte>(reader.ReadToEnd());
                    //if (BPObject != null)
                    //{
                    //    bp.SetBP(BPObject);
                    //    _manager?.Show(new Notification("提示", "加载成功", NotificationType.Success));
                    //}
                }
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("错误", ex.Message, NotificationType.Error));
            }
        }
        public async Task SaveBP()
        {
            try
            {
                using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bp.GetBP())));
                await ShowSaveFileAsync("*.bp", contentStream);
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("错误", ex.Message, NotificationType.Error));
            }
        }

        public async Task SavePNG()
        {
            try
            {
                using var contentStream = Avalonia.PrintToPDF.Print.ToPNGStream(bp);
                await ShowSaveFileAsync("*.png", contentStream);
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("错误", ex.Message, NotificationType.Error));
            }
        }

        public async Task SavePDF()
        {
            try
            {
                using var contentStream = Avalonia.PrintToPDF.Print.ToPDFStream(bp);
                await ShowSaveFileAsync("*.pdf", contentStream);
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("错误", ex.Message, NotificationType.Error));
            }
        }

        public async Task LoadBP()
        {
            try
            {
                var storage = _topLevel?.StorageProvider;
                if (storage == null)
                    return;
                var select = await storage.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = "选择加载bp文件",
                    AllowMultiple = false,
                    FileTypeFilter = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("蓝图")
                        {
                            Patterns = new[] { "*.bp" }
                        }
                    }
                });
                var selectFile = select?.FirstOrDefault();
                if (selectFile != null)
                {
                    using var stream = await selectFile.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    var BPObject = JsonConvert.DeserializeObject<BParent.BPByte>(reader.ReadToEnd());
                    if (BPObject != null)
                    {
                        bp.SetBP(BPObject);
                        _manager?.Show(new Notification("提示", "加载成功", NotificationType.Success));
                    }
                }
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("错误", ex.Message, NotificationType.Error));
            }
        }

        public void ClearBP()
        {
            bp.ClearBP();

            var f = this.FontFamily;

            _manager?.Show(new Notification("Test", f.Name, NotificationType.Success));
        }

        public void PerformanceTest()
        {
            var node = new _StartNode(bp.bp)
            {
            };
            bp.bp.bluePrint.AddChildren(node);
            //var node2 = new sequence(bp)
            //{
            //};
            //bp.bluePrint.AddChildren(node2);
            //Canvas.SetLeft(node2, 100);
            //Canvas.SetTop(node2, 100);
            Blueprint_Canvas.SetLeft(node, 100);
            Blueprint_Canvas.SetTop(node, 100);
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
                var node1 = new Branch(bp.bp);
                bp.bp.bluePrint.AddChildren(node1);
                Blueprint_Canvas.SetLeft(node1, x);
                Blueprint_Canvas.SetTop(node1, y);
                var line = new BP_Line(bp.bp.bluePrint)
                {
                    Width = 1f,
                    Height = 1f
                };
                //bp.bluePrint.AddChildren(line);
                bp.bp.bluePrint.AddLineChildren(line);
                line.SetJoin(node._OutPutJoin[0].Item1, node1._IntPutJoin[0].Item1);
                //line.RefreshDrawBezier();
            }
        }
    }
}