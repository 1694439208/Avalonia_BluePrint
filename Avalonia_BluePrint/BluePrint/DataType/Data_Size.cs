using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_BluePrint.BluePrint.DataType
{
    public class Data_Size
    {
        public Data_Size(int v1, int v2)
        {
            this.Width = v1;
            this.Height = v2;
        }

        public double Width { set; get; }
        public double Height { set; get; }
    }
}
