using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.IJoin
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
