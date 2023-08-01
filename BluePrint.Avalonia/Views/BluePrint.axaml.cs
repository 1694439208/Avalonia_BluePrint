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
using BluePrint.Core.Node;
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

            _nodeTypes = new List<Type>{
                    typeof(_StartNode),
                    typeof(Branch),
                    typeof(ImageShow),
                    typeof(ImageSplit),
                    typeof(sequence),
                    typeof(ThanGreater),
                    typeof(ScriptType),
                    typeof(CreateVar),
                    typeof(GetVar),
                    typeof(Annotation),
                    typeof(ThanEqual),
                    typeof(ThanLess),
                    typeof(ThanStrEqual),

                    //-----------
                    typeof(Sharp_StartNode),
                    //Quicker
                    typeof(Sharp_GetVar),
                    typeof(Sharp_SetVar),
                    //列表功能
                    typeof(Sharp_List_Length),
                    typeof(Sharp_Str_ListJoin),
                    typeof(Sharp_where),
                    typeof(Sharp_select),
                    typeof(Sharp_Any),
                    typeof(Sharp_GetStarValue),
                    typeof(Sharp_FindIndex),
                    typeof(Sharp_GetEnd),
                    typeof(Sharp_OrderBy),
                    typeof(Sharp_OrderByDescending),
                    //流程
                    typeof(Sharp_sequence),
                    //其他
                    typeof(Print),
                    //逻辑运算
                    typeof(Sharp_and),
                    typeof(Sharp_or),
                    //数据转换
                    typeof(Sharp_ToEval),
                    typeof(Sharp_ToJson),
                    //表达式(条件)
                    typeof(Sharp_TextExpression),
                    typeof(Sharp_Str_Equal),
                    typeof(Sharp_Str_IndexOf),
                    typeof(Sharp_Str_StartsWith),
                    typeof(Sharp_Str_EndsWith),
                    //字符串处理
                    typeof(Sharp_StrAppend),
                    typeof(Sharp_Str_Replace),
                    typeof(Sharp_Str_Split),
                    typeof(Sharp_Str_Length),
                    //
                    /*typeof(Sharp_Str_ListToLamble),
                    typeof(Sharp_OrderBy),,*/
                };

            //NodeTypes.Add(typeof(_StartNode));
            //NodeTypes.Add(typeof(Branch));
            //设置节点上下文
            bp.SetContext(_nodeTypes);
            bp.Initialized += (s, e) =>
            {
                var node = new _StartNode(bp)
                {
                };
                bp.bluePrint.AddChildren(node);
                //var node2 = new sequence(bp)
                //{
                //};
                //bp.bluePrint.AddChildren(node2);
                //Canvas.SetLeft(node2, 100);
                //Canvas.SetTop(node2, 100);
                Canvas.SetLeft(node, 100);
                Canvas.SetTop(node, 100);
                int x = 200;
                int y = 30;
                for (int i = 0; i < 5; i++)
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
                    var line = new BP_Line(bp.bluePrint)
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
