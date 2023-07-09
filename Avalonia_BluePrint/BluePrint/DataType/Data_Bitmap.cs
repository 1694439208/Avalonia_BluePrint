using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.DataType
{
    public class Data_Bitmap
    {
        public string Title1 { get; set; }
        [JsonIgnore]
        public Bitmap? bitmap;
        public string? bitmap_path { get; set; }
        public Data_Bitmap(string name)
        {
            Title1 = name;
        }
        [JsonConstructor]
        public Data_Bitmap(string name, string? _path)
        {
            //这序列化有问题 后面再看
            Title1 = name;
            if (_path == null)
            {
                return;
            }
            bitmap_path = _path;
            bitmap = new Bitmap(_path);
            //CPF.Styling.ResourceManager.GetImage(_path,(img)=>{
            //    bitmap = new Bitmap(img);
            //});
        }
        
        public void SetBitmap(Bitmap _bitmap) {
            bitmap = _bitmap;
        }
        public override string ToString()
        {
            return Title1;
        }
    }
}
