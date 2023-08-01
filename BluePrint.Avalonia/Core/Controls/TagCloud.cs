using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace BluePrint.Core.Controls
{
    public class TagCloud : Control
    {
        /// <summary>
        /// 使用Guid生成种子
        /// </summary>
        /// <returns></returns>
        static int GetRandomSeedbyGuid()
        {
            return Guid.NewGuid().GetHashCode();
        }
        //static Random _random = null;
        Random a = new Random();
        //public Random GetRandom()
        //{
        //    if (_random != null)
        //    {
        //        return _random;
        //    }
        //    //Random a = new Random();
        //    _random = new Random();// new Random(GetRandomSeedbyGuid());
        //    return _random;
        //}
        zd d = new zd();
        public bool ddd { set; get; }//判断鼠标是否移动
        public bool mmm { set; get; }//是否停止

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            shuaxin(dc);
            
            
        }
        List<zd> list = new List<zd>();
        List<zd> list_1 = new List<zd>();
        Point kkk = new Point();
        public void huizhi()
        {
            d.y_int = 1;
            kkk = new Point(Width,Height);
            mmm = true;
            //设置初始点
            for (int y = 0; y < 20; y++)
            {
                zd pp = new zd();
                pp.X = a.Next(1, (int)kkk.X);
                pp.Y = a.Next(1, (int)kkk.Y);
                //Console.Write(a.Next(1, 628).ToString()+"\r\n");
                list.Add(pp);
            }

            foreach (zd k in list)
            {
                zd g = new zd();
                int h = a.Next(0, 4);
                switch (h)
                {
                    case 0:
                        g.X = k.X;//- 1;
                        g.Y = k.Y;//+ 1;
                        g.x_int = 0;
                        break;
                    case 1:
                        g.X = k.X;// - 1;
                        g.Y = k.Y;// - 1;
                        g.x_int = 1;
                        break;
                    case 2:
                        g.X = k.X;// + 1;
                        g.Y = k.Y;// - 1;
                        g.x_int = 2;
                        break;
                    case 3:
                        g.X = k.X;//+ 1;
                        g.Y = k.Y;// + 1;
                        g.x_int = 3;
                        break;
                }
                list_1.Add(g);
            }
        }
        public void shuaxin(DrawingContext dc1)
        {
            
            List<zd> nam = new List<zd>();
            foreach (zd k in list_1)
            {
                zd kk = new zd();
                switch (k.x_int)
                {
                    case 0:
                        kk.X = k.X - 1;
                        kk.Y = k.Y + 1;
                        kk.x_int = 0;
                        break;
                    case 1:
                        kk.X = k.X - 1;
                        kk.Y = k.Y - 1;
                        kk.x_int = 1;
                        break;
                    case 2:
                        kk.X = k.X + 1;
                        kk.Y = k.Y - 1;
                        kk.x_int = 2;
                        break;
                    case 3:
                        kk.X = k.X + 1;
                        kk.Y = k.Y + 1;
                        kk.x_int = 3;
                        break;
                }
                if (k.X > Width || k.X < 0 || k.Y > Height || k.Y < 0)
                {
                    zd dd = new zd();
                    dd.X = a.Next(1, (int)kkk.X);
                    dd.Y = a.Next(1, (int)kkk.Y);
                    dd.x_int = a.Next(0, 5-1);
                    nam.Add(dd);
                }
                else
                {
                    nam.Add(kk);
                }
            }
            img_add(dc1, nam);
            list_1.Clear();
            foreach (zd k in nam)
            { list_1.Add(k); }
            nam.Clear();
            //Thread.Sleep(30);

            list.Clear();
            //list_1.RemoveAt(list_1.Count-1);
            foreach (zd hh in list)
            { list.Add(hh); }
            list.Clear();
        }

        public double gen(double shu)
        {
            return Math.Sqrt(shu);
        }
        public void img_add(DrawingContext this_obj, List<zd> list)
        {
            //Point kkk = new Point(Width,Height);
            //Color a = Color.Parse("#37000001");//Color.FromArgb(-922746881);
            try
            {
                //Pen col = new Pen(a, 6);
                //Pen coli = new Pen(System.Drawing.Color.White, 1);
                //col.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                //this.CreateGraphics();
                //this_obj.Clear(Color.Black);
                this_obj.DrawRectangle(Brushes.Black,null,new Rect(Bounds.Size));

                //Console.Write(d.X.ToString() + "\r\n");
                if (d.X > 0)
                {
                    ddd = true;
                    list.Add(d);
                }
                else { ddd = false; }
                foreach (zd pi in list)
                {
                    foreach (zd pii in list)
                    {

                        double l = gen((pii.X - pi.X) * (pii.X - pi.X) + (pi.Y - pii.Y) * (pi.Y - pii.Y));
                        if (l < 127)
                        {
                            double y = 250 - l * 2;
                            if (y > 255)
                            {
                                y = 255;
                            }
                            if (y < 0)
                            {
                                y = 0;
                            }
                            //Math.Ceiling(1.23);
                            var ba00 = Color.FromArgb((byte)y, 255, 255, 255);
                            //Rect rect1 = new Rect(ActualSize);
                            this_obj.DrawLine(new Pen(new SolidColorBrush(ba00), 1), new Point(pi.X, pi.Y), new Point(pii.X, pii.Y));

                        }

                    }
                }
                foreach (zd picc in list)
                {
                    this_obj.DrawEllipse(Brushes.White, new Pen(null,1), new Point(picc.X, picc.Y), 1, 1);
                }

                //gdi.DrawImage(b, 0, 0, b.Width, b.Height);
                //this_obj.Dispose();
                if (ddd == true)
                {
                    list.RemoveAt(list.Count - 1);
                }
            }
            catch (Exception e) { }


        }
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            var pos = e.GetPosition(this);
            d.X = (int)pos.X;
            d.Y = (int)pos.Y;
            d.x_int = 0;
            d.y_int = 0;
            //Console.Write("x" + e.X.ToString() + "\r\ny" + e.Y.ToString() + "\r\n");
        }
        /// <summary>
        /// 微秒级延迟,会稍有偏差
        /// </summary>
        /// <param name="time">延迟时间，1/毫秒，0.0500/500微秒</param>
        /// <returns></returns>
        public static double delayUs(double time)
        {
            System.Diagnostics.Stopwatch stopTime = new System.Diagnostics.Stopwatch();
            stopTime.Start();
            while (stopTime.Elapsed.TotalMilliseconds < time) { }
            stopTime.Stop();
            return stopTime.Elapsed.TotalMilliseconds;
        }
        Timer time_;
        protected override void OnInitialized()
        {
            Width = 300;
            Height = 300;
            ClipToBounds = true;
            //BitmapTag = new Bitmap(200,200);
            //dc1 = DrawingContext.FromBitmap(BitmapTag);
            huizhi();
            //Invalimage();
            time_ = new Timer(a => {
                //while (true)
                //{
                //    delayUs(500);
                //    this.InvalidateVisual();
                //}
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // 通知Avalonia刷新UI
                    InvalidateVisual();
                });
            },null,0,30);
        }
        public void Invalimage()
        {
            this.InvalidateVisual();
        }
    }
    //-------------
    public class zd
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int x_int { set; get; }
        public int y_int { set; get; }
    }
    public class dong
    {
        public int x { set; get; }
        public int y { set; get; }
        public int x_int { set; get; }
        public bool bb { set; get; }
    }
}
