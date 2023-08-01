
using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrint.Core.INode
{
    public class INode
    {
        public DateTime When { get; set; }

    }
    /*class INode<T> : NodeContext where T : Control
    {
        public string Who { get; set; }
        public T NewState;
        public T OldState;
    }*/
}
