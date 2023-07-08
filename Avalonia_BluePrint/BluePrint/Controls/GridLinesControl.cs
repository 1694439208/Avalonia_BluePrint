using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_BluePrint.BluePrint.Controls
{
    internal class GridLinesControl : Control
    {
        public GridLinesControl()
        {

        }
        PathGeometry XPath = new PathGeometry();
        PathGeometry YPath = new PathGeometry();
        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);
            var rect = finalRect;

            var xcontext = XPath.Open();
            var ycontext = YPath.Open();
            for (int i = 0; i < finalRect.Height; i++)
            {
                if (i % 12 == 0)
                {
                    xcontext.BeginFigure(new Point(0, i), false);
                    xcontext.LineTo(new Point(finalRect.Width, i));
                }
                if (i % 84 == 0)
                {
                    ycontext.BeginFigure(new Point(0, i), false);
                    ycontext.LineTo(new Point(rect.Width, i));
                }
            }

            for (int i = 0; i < rect.Width; i++)
            {
                if (i % 12 == 0)
                {
                    xcontext.BeginFigure(new Point(i, 0), false);
                    xcontext.LineTo(new Point(i, rect.Height));
                }
                if (i % 84 == 0)
                {
                    ycontext.BeginFigure(new Point(i, 0), false);
                    ycontext.LineTo(new Point(i, rect.Height));
                }
            }
        }
        readonly IPen XPathColor = new ImmutablePen(Color.FromArgb(255, 52, 52, 52).ToUInt32(), 1d, null, PenLineCap.Round, PenLineJoin.Round);
        readonly IPen YPathColor = new ImmutablePen(Color.FromArgb(150, 0, 0, 0).ToUInt32(), 1d, null, PenLineCap.Round, PenLineJoin.Round);

        public override void Render(DrawingContext dc)
        {
            var rect = this.Bounds;
            base.Render(dc);
            // new SolidColorBrush(Color.FromRgb(39, 39, 39));
            dc.FillRectangle(new SolidColorBrush(Color.FromRgb(255, 255, 255)), rect);

            dc.DrawGeometry(null, XPathColor, XPath);
            dc.DrawGeometry(null, YPathColor, YPath);
        }
    }
}
