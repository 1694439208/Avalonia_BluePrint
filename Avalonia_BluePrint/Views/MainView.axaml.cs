using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json;
using ��ͼ���ư�.BluePrint.IJoin;
using ��ͼ���ư�.BluePrint;
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
            bp.RegisterNode(typeof(AINode));
        }

        private async Task ShowSaveFileAsync(string extension, Stream cstream)
        {
            var storage = _topLevel?.StorageProvider;
            if (storage == null)
                return;
            var ret = await storage.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "ѡ�񱣴��ļ�Ŀ¼",
                DefaultExtension = extension,
                FileTypeChoices = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("��ͼ")
                        {
                            Patterns = new[] { extension }
                        }
                    }
            });

            if (ret != null)
            {
                using var stream = await ret.OpenWriteAsync();
                cstream.CopyTo(stream);
                _manager?.Show(new Notification("��ʾ", "����ɹ�", NotificationType.Success));
            }
        }

        public async Task SaveBP()
        {
            try
            {
                using var contentStream = new MemoryStream();
                using var writer = new StreamWriter(contentStream);
                await writer.WriteAsync(JsonConvert.SerializeObject(bp.GetBP()));
                await ShowSaveFileAsync("*.bp", contentStream);
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("����", ex.Message, NotificationType.Error));
            }
        }

        public async Task SavePNG()
        {
            try
            {
                using var contentStream = Print.ToPNGStream(bp);
                await ShowSaveFileAsync("*.png", contentStream);
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("����", ex.Message, NotificationType.Error));
            }
        }

        public async Task SavePDF()
        {
            try
            {
                using var contentStream = Print.ToPDFStream(bp);
                await ShowSaveFileAsync("*.pdf", contentStream);
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("����", ex.Message, NotificationType.Error));
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
                    Title = "ѡ�����bp�ļ�",
                    AllowMultiple = false,
                    FileTypeFilter = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("��ͼ")
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
                        _manager?.Show(new Notification("��ʾ", "���سɹ�", NotificationType.Success));
                    }
                }
            }
            catch (System.Exception ex)
            {
                _manager?.Show(new Notification("����", ex.Message, NotificationType.Error));
            }
        }

        public void ClearBP()
        {
            bp.ClearBP();
        }
    }
}