using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrint.Core.INode.Model
{
    public interface IBP_Render
    {
        /// <summary>
        /// 渲染状态
        /// </summary>
        public bool Render { get; set; }
    }
}
