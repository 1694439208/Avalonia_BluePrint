using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.DataType
{
    public enum EveType
    {
        MouseUp, MouseDown
    }
    public class JoinEventType
    {
        public EveType eveType { set; get; }
        public object Value { set; get; }
    }
}
