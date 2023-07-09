using Avalonia.Controls;
using System.Collections.Generic;
using System;
using 蓝图重制版.BluePrint.INode;
using 蓝图重制版.BluePrint;
using Avalonia.Media;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;

namespace Avalonia_BluePrint.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _MainWindow = this;
        }
        public static WindowNotificationManager? _manager;
        public static Window? _MainWindow;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _manager = new WindowNotificationManager(this) { MaxItems = 3 };
        }
    }
}