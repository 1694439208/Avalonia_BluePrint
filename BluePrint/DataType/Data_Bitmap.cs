using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.DataType
{
    public class Data_Bitmap
    {
        public string Title;
        public Bitmap bitmap;
        public string bitmap_path;
        public Data_Bitmap(string _Title)
        {
            Title = _Title;
        }
        public Data_Bitmap(string _Title,string _path)
        {
            Title = _Title;
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
            return Title;
        }
    }
}
