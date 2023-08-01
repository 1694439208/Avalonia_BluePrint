using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.ResponseModels;
using System.IO;
using Microsoft.Maui.Graphics.Platform;
using OpenAI.Interfaces;
using Avalonia.Controls;
using System.Runtime.InteropServices;
using System.Collections;
using BluePrint.Core.IJoin;
using BluePrint.Core.INode;
using BluePrint.Core;
using BluePrint.Core.Join;
using BluePrint.Core.Runtime;
using BluePrint.Core.DataType;

namespace Avalonia_BluePrint.Nodes
{
    [NodeBaseInfo("AI组件", "功能")]
    [Guid("F61391C6-F524-4B6B-81EC-118E87DBA4F4")]
    public class AINode : NodeBase
    {
        public AINode() : base() { }
        public AINode(BParent _bParent) : base(_bParent)
        {
            Title = "AI组件";
            ///节点输出参数 设置
            _OutPutJoin = new List<(IJoinControl, Node_Interface_Data)>
            {
                (new ExecJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "执行结束的接头",
                    Value = new JoinType("执行结束"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new TextJoint(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "输出",
                    Tips = "输出",
                    Value="",
                    Type = typeof(string),
                    IsTypeCheck=false,
                }),
                (new AddCustomJoin(bParent, IJoinControl.NodePosition.right, this),new Node_Interface_Data{
                    Title = "添加输出",
                    Tips = "添加输出",
                    Value = new JoinType("添加输出"),
                    Type = typeof(JoinType),
                }),
            };

            ///节点输入参数 设置
            _IntPutJoin = new List<(IJoinControl, Node_Interface_Data)>
            {
                (new ExecJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "开始执行的接头",
                    Value = new JoinType("执行开始"),
                    Type = typeof(JoinType),
                    Tips = "test",
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this)
                {
                    Watermark="prompt"
                },new Node_Interface_Data{
                    Title = "prompt",
                    Value = "",
                    Type = typeof(string),
                    IsTypeCheck=false,
                    Tips = "",
                    ClassValue = new Dictionary<string, MyData>{
                        {nameof(TextBoxJoint.Enabled),new MyData<bool>(false) },
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("prompt" )},
                        {nameof(TextBoxJoint.Width),new MyData<double>(250 ) },
                        {nameof(TextBoxJoint.Height),new MyData<double>(125 ) },
                        {nameof(TextBoxJoint.Multiline),new MyData<bool>(true ) },
                    },
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this)
                {
                    Watermark="prompt2"
                },new Node_Interface_Data{
                    Title = "prompt2",
                    Value = "",
                    Type = typeof(string),
                    IsTypeCheck=false,
                    Tips = "",
                    ClassValue = new Dictionary<string, MyData>{
                        {nameof(TextBoxJoint.Enabled),new MyData<bool>(false) },
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("prompt2" )},
                        {nameof(TextBoxJoint.Width),new MyData<double>(250 ) },
                        {nameof(TextBoxJoint.Height),new MyData<double>(125 ) },
                        {nameof(TextBoxJoint.Multiline),new MyData<bool>(true ) },
                    },
                }),
                (new Checkjoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "图片模式",
                    Value = false,
                    Type = typeof(bool),
                    Tips = "test",
                }),
                (new ComboBoxJoin(bParent, IJoinControl.NodePosition.Left, this),new Node_Interface_Data{
                    Title = "模型",
                    Value = new List<string>(){"GPT3.5","GPT4"},
                    Type = typeof(List<string>),
                    Tips = "test",
                }),
                (new TextBoxJoint(bParent, IJoinControl.NodePosition.Left, this)
                {
                    Watermark="sk"
                },new Node_Interface_Data{
                    Title = "sk",
                    Value = "",
                    Type = typeof(string),
                    Tips = "",
                    ClassValue = new Dictionary<string, MyData>{
                        {nameof(TextBoxJoint.Enabled),new MyData<bool>(false) },
                        {nameof(TextBoxJoint.Watermark),new MyData<string>("sk" )},
                        {nameof(TextBoxJoint.Width),new MyData<double>(250 ) },
                        {nameof(TextBoxJoint.PasswordMode),new MyData<bool>(true ) },
                    },
                }),
            };
        }

        public override async Task Execute(object Context, List<object> arguments, Evaluate.Result result)
        {
            var tprompt = arguments[0];
            var tprompt2 = arguments[0];
            var prompt = tprompt?.ToString() ?? "";
            var prompt2 = tprompt2?.ToString() ?? "";
            if (tprompt.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                var enumerable = (IEnumerable)tprompt;
                prompt = string.Join("\r\n", enumerable);
            }
            if (tprompt2.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                var enumerable = (IEnumerable)tprompt2;
                prompt2 = string.Join("\r\n", enumerable);
            }
            var mode = Convert.ToBoolean(arguments[2]);
            var model = Convert.ToString(arguments[3]);
            var sk = Convert.ToString(arguments[4]);

            if (string.IsNullOrWhiteSpace(sk))
                return;

            var openAIService = new OpenAIService(new OpenAI.OpenAiOptions()
            {
                ApiKey = sk,
                BaseDomain = "https://api.youqianpay.com/"
            });

            _IntPutJoin[1].Item1.Set(new Node_Interface_Data { Value = prompt });
            _IntPutJoin[1].Item1.RenderData();

            if (string.IsNullOrWhiteSpace(prompt) && string.IsNullOrWhiteSpace(prompt2))
                return;

            await base.Execute(Context, arguments, result);

            for (int i = 0; i < _OutPutJoin.Count - 2; i++)
            {
                if (mode)
                {
                    var ret = await openAIService.Image.CreateImage(new ImageCreateRequest()
                    {
                        Prompt = (prompt ?? prompt2)!,
                        N = 1,
                        Size = "512x512",
                        ResponseFormat = "b64_json"
                    });
                    var retMsg = ret.Successful ? ret.Results.Select(x => x.B64).First() : "";
                    var image = new Data_Bitmap("");
                    var imageStream = new MemoryStream();
                    var imagef = PlatformImage.FromStream(new MemoryStream(Convert.FromBase64String(retMsg)));
                    await imagef.SaveAsync(imageStream);
                    imageStream.Position = 0;
                    image.SetBitmap(new Avalonia.Media.Imaging.Bitmap(imageStream));
                    if (image != null)
                    {
                        result.SetReturnValue(i, image);
                    }
                }
                else
                {
                    var msgs = new List<ChatMessage>();
                    if (!string.IsNullOrWhiteSpace(prompt))
                        msgs.Add(ChatMessage.FromUser(prompt));
                    if (!string.IsNullOrWhiteSpace(prompt2))
                        msgs.Add(ChatMessage.FromUser(prompt2));
                    var ret = await openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
                    {
                        Messages = msgs,
                        Model = model == "GPT4" ? Models.Gpt_4 : Models.Gpt_3_5_Turbo_16k,
                        MaxTokens = 1024
                    });
                    var retMsg = ret.Successful ? ret.Choices.First().Message.Content : ret.Error?.Message ?? "未知异常";
                    result.SetReturnValue(i, retMsg);
                }
            }
        }
    }
}
