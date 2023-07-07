using CPF;
using CPF.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace 蓝图重制版.BluePrint
{
    class test
    {
        public static void Log(string data)
        {
            File.AppendAllText($"日志_Node", $"{data}\r\n");
        }
    }
}
