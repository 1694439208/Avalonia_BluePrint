﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Controls.Notifications;
using Splat;
using Avalonia_BluePrint.Views;

namespace 蓝图重制版.BluePrint.IJoin
{
    public static class UIElementTool
    {
        public static T Get<T>(this List<object> element, int name)
        {
            return (T)(element[name]);
        }
        /// <summary>
        /// 获取id变量名
        /// </summary>
        /// <param name="element"></param>
        /// <param name="IsD">是否加入前缀表示使用变量</param>
        /// <returns></returns>
        public static string GetID(this int element, bool IsD = true)
        {
            return $"{(IsD ? "var " : "")}temp_{element}";
        }
        public static string join(this List<string> element, string str)
        {
            return string.Join(str, element.Where(a => a != "").ToList());
        }

        public static Point GetOffset(this Control element)
        {
            // 获取元素的当前偏移量
            //var transform = element.RenderTransform as TranslateTransform;
            //return new Point(transform.X, transform.Y);
            return element.TranslatePoint(default, element.GetVisualParent()) ?? default;
        }
        public static Point GetParentOffset(this Control element, Visual parent)
        {

            // 获取控件相对于其父元素的偏移量
            return element.TranslatePoint(default, parent) ?? default;

            // TODO: Use the offset value here
        }
        /// <summary>
        /// 递归获取相对于父元素的坐标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Point GetPosition(this Control element,Point point)
        {
            //改成从canvas取
            return new Point(Canvas.GetLeft(element), Canvas.GetTop(element)) + point;
            //return GetParentPosition(element, point, parent);
        }
        public static Point GetParentPosition(Control element,Point point, Visual parent) {
            if (element == null)
            {
                return point;
            }
            if (element.Parent == parent)//element.Parent == null||TranslatePoint
            {
                return element.TranslatePoint(point, parent)??default;
            }

            return GetParentPosition(element.Parent as Control, element.TranslatePoint(point, parent) ?? default, parent);
        }
        //static List<ToastControl> controls = new List<ToastControl>();
        public static void Toast(BluePrint control, string title, Point point, float time = 0.3f)
        {
            MainWindow._manager?.Show(new Notification("提示", title, NotificationType.Error));
            //if (controls.Count > 0)
            //{
            //    foreach (var item in controls)
            //    {
            //        item.Start(title, point, time);
            //        return;
            //    }
            //}
            //else
            //{
            //    var toast = new ToastControl(title, point, time);
            //    toast.ZIndex = 100;
            //    control.AddChildren1(toast);
            //    controls.Add(toast);
            //}

        }
    }
}
