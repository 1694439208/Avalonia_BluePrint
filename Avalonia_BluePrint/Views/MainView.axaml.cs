using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json;
using 蓝图重制版.BluePrint.IJoin;
using 蓝图重制版.BluePrint;
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

            }
        }

        public void ClearBP()
        {
            bp.ClearBP();
        }
    }
}