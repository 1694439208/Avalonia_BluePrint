using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.DataType
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
