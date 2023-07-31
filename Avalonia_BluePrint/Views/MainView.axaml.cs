using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint;
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
            dialog.Filters.Add(new FileDialogFilter { Name = "选择保存bp文件目录", Extensions = { "bp" } });


            var result = await dialog.ShowAsync(MainWindow._MainWindow);

            if (!string.IsNullOrEmpty(result))
            {
                string savePath = result;

                // 处理选定的保存目录
                var bptext = JsonConvert.SerializeObject(bp.GetBP());
                File.WriteAllText(savePath, bptext);
                MainWindow._manager?.Show(new Notification("提示", "保存成功", NotificationType.Error));
            }
        }
        public async Task SavePNG()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter { Name = "选择保存png文件目录", Extensions = { "png" } });


            var result = await dialog.ShowAsync(MainWindow._MainWindow);

            if (!string.IsNullOrEmpty(result))
            {
                string savePath = result;

                // 处理选定的保存目录
                Print.ToPNGFile(savePath, bp);
                MainWindow._manager?.Show(new Notification("提示", "保存成功", NotificationType.Error));
            }
        }
        public async Task SavePDF()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter { Name = "选择保存pdf文件目录", Extensions = { "pdf" } });


            var result = await dialog.ShowAsync(MainWindow._MainWindow);

            if (!string.IsNullOrEmpty(result))
            {
                string savePath = result;

                // 处理选定的保存目录
                Print.ToFile(savePath, bp);
                MainWindow._manager?.Show(new Notification("提示", "保存成功", NotificationType.Error));
            }
        }

        public async Task LoadBP()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.AllowMultiple = false;
                dialog.Filters.Add(new FileDialogFilter { Name = "选择加载bp文件", Extensions = { "bp" } });

                var result = await dialog.ShowAsync(MainWindow._MainWindow);

                if (result != null && result.Length > 0)
                {
                    string filePath = result[0];
                    // 处理选定的文件路径
                    var strjson = File.ReadAllText(filePath);
                    var BPObject = JsonConvert.DeserializeObject<BParent.BPByte>(strjson);
                    if (BPObject != null)
                    {
                        bp.SetBP(BPObject);
                        MainWindow._manager?.Show(new Notification("提示", "加载成功", NotificationType.Error));
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