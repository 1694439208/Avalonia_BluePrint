//using CPF;
//using CPF.Animation;
//using CPF.Controls;
//using CPF.Drawing;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BluePrint.IJoin
//{
//    public class ToastControl : Control
//    {
//        public ToastControl(string title, Point point, float time = 0.5f)
//        {

//            _point = point;
//            _time = time;
//            _title = title;
//        }
//        private Point _point;
//        private string _title;
//        private float _time;
//        public bool IsAnimaEnd = true;
//        protected override void InitializeComponent()
//        {
//            Background = "39,39,39,0";
//            Padding = "5,5,5,5";
//            CornerRadius = "5,5,5,5";
//            BorderThickness = "0,1,0,1";
//            BorderFill = "0,0,0";
//            BorderType = BorderType.BorderThickness;
//            Start(_title, _point, _time);
//            Children.Add(Title);
//            IsHitTestVisible = false;
//        }
//        public TextBlock Title = new TextBlock {
//            Foreground = "rgb(255,255,255)",
//        };
//        public void Start(string title, Point point, float time = 0.3f)
//        {
//            Title.Text = title;
//            IsAnimaEnd = false;
//            Visibility = Visibility.Visible;
//            MarginLeft = point.X;
//            MarginTop = point.Y;
//            this.TransitionValue(a => a.Background, "39,39,39,255", TimeSpan.FromSeconds(0.3), null, AnimateMode.Linear, () =>
//            {
                
//            });
//            this.TransitionValue(a => a.MarginTop, point.Y - 35, TimeSpan.FromSeconds(time), new PowerEase { }, AnimateMode.EaseIn, () =>
//            {
//                this.Delay(TimeSpan.FromSeconds(0.5), () => {
//                    IsAnimaEnd = true;
//                    Visibility = Visibility.Collapsed;
//                });
//            });
//        }
//    }
//}
