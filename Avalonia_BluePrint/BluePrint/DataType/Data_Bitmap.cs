using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.DataType
{
    public class Data_Bitmap
    {
        public string Title1 { get; set; }


        private string? _bitmap_path;
        public string? bitmap_path
        {
            get { return _bitmap_path; }
            set
            {
                if (value != null)
                {
                    try
                    {
                        bitmap = new Bitmap(value);
                    }
                    catch (Exception)
                    {
                    }
                }
                _bitmap_path = value;
            }
        }

        [JsonIgnore]
        public Bitmap? bitmap;
        public Data_Bitmap(string name)
        {
            Title1 = name;
        }
        [JsonConstructor]
        public Data_Bitmap(string name, string _path)
        {
            //这序列化有问题 后面再看
            Title1 = name;
            bitmap_path = _path;
            try
            {
                bitmap = new Bitmap(bitmap_path);
            }
            catch (Exception)
            {
            }
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
