using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrint.Avalonia.BluePrint.DataType
{
    public class Data_Point
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        public Data_Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the X position.
        /// </summary>
        public double X { set; get; }

        /// <summary>
        /// Gets the Y position.
        /// </summary>
        public double Y { set; get; }
    }
}
