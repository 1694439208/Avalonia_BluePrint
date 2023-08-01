using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrint.Core.INode
{
    public class NodeStyle
    {
        public Color? OKStateColor { get; set; }
        public Color? RunningStateColor { get; set; } = Colors.Blue;
        public Color? ErrorStateColor { get; set; } = Colors.Red;
    }
}
