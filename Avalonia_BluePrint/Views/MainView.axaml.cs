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

namespace Avalonia_BluePrint.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            bp.RegisterNode(typeof(AINode));
        }

        public async Task SaveBP()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter { Name = "ѡ�񱣴�bp�ļ�Ŀ¼", Extensions = { "bp" } });


            var result = await dialog.ShowAsync(MainWindow._MainWindow);

            if (!string.IsNullOrEmpty(result))
            {
                string savePath = result;

                // ����ѡ���ı���Ŀ¼
                var bptext = JsonConvert.SerializeObject(bp.GetBP());
                File.WriteAllText(savePath, bptext);
                MainWindow._manager?.Show(new Notification("��ʾ", "����ɹ�", NotificationType.Error));
            }
        }
        public async Task SavePNG()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter { Name = "ѡ�񱣴�png�ļ�Ŀ¼", Extensions = { "png" } });


            var result = await dialog.ShowAsync(MainWindow._MainWindow);

            if (!string.IsNullOrEmpty(result))
            {
                string savePath = result;

                // ����ѡ���ı���Ŀ¼
                Print.ToPNGFile(savePath, bp);
                MainWindow._manager?.Show(new Notification("��ʾ", "����ɹ�", NotificationType.Error));
            }
        }
        public async Task SavePDF()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter { Name = "ѡ�񱣴�pdf�ļ�Ŀ¼", Extensions = { "pdf" } });


            var result = await dialog.ShowAsync(MainWindow._MainWindow);

            if (!string.IsNullOrEmpty(result))
            {
                string savePath = result;

                // ����ѡ���ı���Ŀ¼
                Print.ToFile(savePath, bp);
                MainWindow._manager?.Show(new Notification("��ʾ", "����ɹ�", NotificationType.Error));
            }
        }

        public async Task LoadBP()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.AllowMultiple = false;
                dialog.Filters.Add(new FileDialogFilter { Name = "ѡ�����bp�ļ�", Extensions = { "bp" } });

                var result = await dialog.ShowAsync(MainWindow._MainWindow);

                if (result != null && result.Length > 0)
                {
                    string filePath = result[0];
                    // ����ѡ�����ļ�·��
                    var strjson = File.ReadAllText(filePath);
                    var BPObject = JsonConvert.DeserializeObject<BParent.BPByte>(strjson);
                    if (BPObject != null)
                    {
                        bp.SetBP(BPObject);
                        MainWindow._manager?.Show(new Notification("��ʾ", "���سɹ�", NotificationType.Error));
                    }
                }
            }
            catch (System.Exception ex)
            {
                File.WriteAllText("error.txt",ex.Message);
            }
        }

        public void ClearBP()
        {
            bp.ClearBP();
        }

        public void Test()
        {
            bp.UnRegisterNode(typeof(AINode));
        }
    }
}