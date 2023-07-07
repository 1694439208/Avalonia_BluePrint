using System;
using System.Collections.Generic;
using System.Text;

namespace 蓝图重制版.BluePrint.IJoin
{
    public class JoinType
    {
        public string Title;
        public JoinType(string name) {
            Title = name;
        }
        public override string ToString()
        {
            return Title;
        }

    }
}
