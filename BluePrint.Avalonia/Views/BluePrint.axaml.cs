using Avalonia.Controls;
using System.Collections.Generic;
using System;
using BluePrint;
using Avalonia.Media;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAvalonia = Avalonia;
using BluePrint.Core;
using BluePrint.Core.INode;

namespace BluePrint.Views
{
    public partial class BluePrint : UserControl
    {
        // 自定义一个JsonConverter类型的对象，用于在序列化和反序列化时带上类型信息
        class ObjectWithTypeConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return true;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                // 使用JsonSerializer.Deserialize方法反序列化
                return serializer.Deserialize(reader, Type.GetType((string)((JObject)JToken.ReadFrom(reader))["$type"]));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                // 写入类型信息和对象信息
                writer.WriteStartObject();
                writer.WritePropertyName("$type");
                writer.WriteValue(value.GetType().FullName);
                writer.WritePropertyName("$value");
                serializer.Serialize(writer, value);
                writer.WriteEndObject();
            }
        }
        /// <summary>
        /// 蓝图jobject的参数转为net对象
        /// </summary>
        /// <param name="bPByte"></param>
        public void JobjectTobject(BParent.BPByte bPByte)
        {
            foreach (var item in bPByte.NodeList)
            {
                //然后再重新创建
                foreach (var item1 in item._IntPutJoin)
                {
                    if (item1.Data.Value is JObject jobj)
                    {
                        if (item1.Data.Type == null)
                        {
                            item1.Data.Value = jobj.ToObject<object>();
                        }
                        else
                        {
                            item1.Data.Value = jobj.ToObject(item1.Data.Type);
                        }
                    }
                    if (item1.Data.Value is JArray arr)
                    {
                        if (item1.Data.Type == null)
                        {
                            item1.Data.Value = arr.ToObject<object>();
                        }
                        else
                        {
                            item1.Data.Value = arr.ToObject(item1.Data.Type);
                        }
                    }
                }
                foreach (var item1 in item._OutPutJoin)
                {
                    if (item1.Data.Value is JObject jobj)
                    {
                        if (item1.Data.Type == null)
                        {
                            item1.Data.Value = jobj.ToObject<object>();
                        }
                        else
                        {
                            item1.Data.Value = jobj.ToObject(item1.Data.Type);
                        }
                    }
                }
            }
        }

        public BParent bp { get; set; }

        private List<Type> _nodeTypes;

        public BluePrint()
        {
            InitializeComponent();


            DockPanel stackPanel = new DockPanel()
            {
                HorizontalAlignment = OAvalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = OAvalonia.Layout.VerticalAlignment.Stretch,
            };

            bp = new BParent
            {
                Margin = new OAvalonia.Thickness(0, 0, 0, 0),
                //HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,// HorizontalAlignment.Stretch
            };
            //bp.Background = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(bp, 0);
            Canvas.SetTop(bp, 0);

            _nodeTypes = new List<Type>();

            //NodeTypes.Add(typeof(_StartNode));
            //NodeTypes.Add(typeof(Branch));
            //设置节点上下文
            bp.SetContext(_nodeTypes);
            stackPanel.Children.Add(bp);

            this.Content = stackPanel;
        }
        public void AddNode<T>(double x, double y, Action<T>? action = null) where T : NodeBase
        {
            var node = Activator.CreateInstance(typeof(T), bp) as T;
            if (node != null)
            {
                node.OnNodeInitEveTemp += (s, e) =>
                {
                    if (action != null)
                    {
                        action(node);
                    }
                };
                bp.bluePrint.AddChildren(node);
                Canvas.SetLeft(node, x);
                Canvas.SetTop(node, y);
            }
        }

        public void RegisterNode(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                RegisterNode(type);
            }
        }

        public void UnRegisterNode(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                UnRegisterNode(type);
            }
        }

        public void RegisterNode(Type type)
        {
            if (!_nodeTypes.Contains(type))
                _nodeTypes.Add(type);
        }

        public void UnRegisterNode(Type type)
        {
            if (_nodeTypes.Contains(type))
                _nodeTypes.Remove(type);
        }

        public BParent.BPByte GetBP()
        {
            return bp.SerializeBP();
        }

        public void SetBP(BParent.BPByte bp)
        {
            JobjectTobject(bp);
            this.bp.Deserialize(bp);
        }

        public void ClearBP()
        {
            bp.bluePrint.Clear();
        }
    }
}
