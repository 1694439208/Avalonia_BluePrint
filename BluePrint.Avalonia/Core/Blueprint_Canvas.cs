using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using BluePrint.Core.INode.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BluePrint.Core
{
    public class Blueprint_Canvas : VirtualizingPanel, INavigableContainer
    {
        /// <summary>
        /// 设置需要移动 默认移动一次
        /// </summary>
        public static readonly AttachedProperty<bool> IsMoveProperty =
            AvaloniaProperty.RegisterAttached<Blueprint_Canvas, Control, bool>("IsMove", true);
        /// <summary>
        /// Defines the Left attached property.
        /// </summary>
        public static readonly AttachedProperty<double> LeftProperty =
            AvaloniaProperty.RegisterAttached<Blueprint_Canvas, Control, double>("Left", double.NaN);

        /// <summary>
        /// Defines the Top attached property.
        /// </summary>
        public static readonly AttachedProperty<double> TopProperty =
            AvaloniaProperty.RegisterAttached<Blueprint_Canvas, Control, double>("Top", double.NaN);

        /// <summary>
        /// Defines the Right attached property.
        /// </summary>
        public static readonly AttachedProperty<double> RightProperty =
            AvaloniaProperty.RegisterAttached<Blueprint_Canvas, Control, double>("Right", double.NaN);

        /// <summary>
        /// Defines the Bottom attached property.
        /// </summary>
        public static readonly AttachedProperty<double> BottomProperty =
            AvaloniaProperty.RegisterAttached<Blueprint_Canvas, Control, double>("Bottom", double.NaN);

        /// <summary>
        /// Initializes static members of the <see cref="Blueprint_Canvas"/> class.
        /// </summary>
        static Blueprint_Canvas()
        {
            ClipToBoundsProperty.OverrideDefaultValue<Blueprint_Canvas>(false);
            AffectsParentArrange<Blueprint_Canvas>(LeftProperty, TopProperty, RightProperty, BottomProperty, IsMoveProperty);
        }

        /// <summary>
        /// Gets the value of the Left attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <returns>The control's left coordinate.</returns>
        public static double GetLeft(AvaloniaObject element)
        {
            return element.GetValue(LeftProperty);
        }
        public static bool GetIsMove(AvaloniaObject element)
        {
            return element.GetValue(IsMoveProperty);
        }
        public static void SetIsMove(AvaloniaObject element,bool value)
        {
            element.SetValue(IsMoveProperty, value);
        }
        /// <summary>
        /// Sets the value of the Left attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <param name="value">The left value.</param>
        public static void SetLeft(AvaloniaObject element, double value)
        {
            SetIsMove(element,true);
            element.SetValue(LeftProperty, value);
        }

        /// <summary>
        /// Gets the value of the Top attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <returns>The control's top coordinate.</returns>
        public static double GetTop(AvaloniaObject element)
        {
            return element.GetValue(TopProperty);
        }

        /// <summary>
        /// Sets the value of the Top attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <param name="value">The top value.</param>
        public static void SetTop(AvaloniaObject element, double value)
        {
            SetIsMove(element, true);
            element.SetValue(TopProperty, value);
        }

        /// <summary>
        /// Gets the value of the Right attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <returns>The control's right coordinate.</returns>
        public static double GetRight(AvaloniaObject element)
        {
            return element.GetValue(RightProperty);
        }

        /// <summary>
        /// Sets the value of the Right attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <param name="value">The right value.</param>
        public static void SetRight(AvaloniaObject element, double value)
        {
            SetIsMove(element, true);
            element.SetValue(RightProperty, value);
        }

        /// <summary>
        /// Gets the value of the Bottom attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <returns>The control's bottom coordinate.</returns>
        public static double GetBottom(AvaloniaObject element)
        {
            return element.GetValue(BottomProperty);
        }

        /// <summary>
        /// Sets the value of the Bottom attached property for a control.
        /// </summary>
        /// <param name="element">The control.</param>
        /// <param name="value">The bottom value.</param>
        public static void SetBottom(AvaloniaObject element, double value)
        {
            SetIsMove(element, true);
            element.SetValue(BottomProperty, value);
        }


        ///// <summary>
        ///// Measures the control.
        ///// </summary>
        ///// <param name="availableSize">The available size.</param>
        ///// <returns>The desired size of the control.</returns>
        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    //availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

        //    //foreach (Control child in Children)
        //    //{
        //    //    child.Measure(availableSize);
        //    //}

        //    //return new Size();

        //    //return base.MeasureOverride(availableSize);
        //}

        /// <summary>
        /// Arranges a single child.
        /// </summary>
        /// <param name="child">The child to arrange.</param>
        /// <param name="finalSize">The size allocated to the Blueprint_Canvas.</param>
        protected virtual void ArrangeChild(Control child, Size finalSize)
        {
            double x = 0.0;
            double y = 0.0;
            double elementLeft = GetLeft(child);

            if (!double.IsNaN(elementLeft))
            {
                x = elementLeft;
            }
            else
            {
                // Arrange with right.
                double elementRight = GetRight(child);
                if (!double.IsNaN(elementRight))
                {
                    x = finalSize.Width - child.DesiredSize.Width - elementRight;
                }
            }

            double elementTop = GetTop(child);
            if (!double.IsNaN(elementTop))
            {
                y = elementTop;
            }
            else
            {
                double elementBottom = GetBottom(child);
                if (!double.IsNaN(elementBottom))
                {
                    y = finalSize.Height - child.DesiredSize.Height - elementBottom;
                }
            }

            child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
        }

        /// <summary>
        /// Arranges the control's children.
        /// </summary>
        /// <param name="finalSize">The size allocated to the control.</param>
        /// <returns>The space taken.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var index = 0;
            foreach (Control child in Children)
            {
                //if (GetIsMove(child))
                //{
                //    index++;
                //    ArrangeChild(child, finalSize);
                //    SetIsMove(child, false);
                //}
                ArrangeChild(child, finalSize);
            }
            //Debug.Print($"计算数量：{index}");
            return finalSize;

            //return base.ArrangeOverride(finalSize);
        }
        protected override Control ScrollIntoView(int index)
        {
            throw new NotImplementedException();
        }

        protected override Control ContainerFromIndex(int index)
        {
            throw new NotImplementedException();
        }

        protected override int IndexFromContainer(Control container)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Control> GetRealizedContainers()
        {
            throw new NotImplementedException();
        }

        protected override IInputElement GetControl(NavigationDirection direction, IInputElement from, bool wrap)
        {
            throw new NotImplementedException();
        }
    }
}
