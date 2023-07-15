using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json;
using ��ͼ���ư�.BluePrint.IJoin;
using ��ͼ���ư�.BluePrint;
using System.IO;
using Avalonia.Controls.Notifications;
using System.Threading.Tasks;

namespace Avalonia_BluePrint.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
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

            }
        }

        public void ClearBP()
        {
            bp.ClearBP();
        }
    }
}