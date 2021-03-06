﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EpgTimer
{
    /// <summary>
    /// SuspendCheckWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SuspendCheckWindow : Window
    {
        /// <summary>表示モード mode 0:再起動、1:スタンバイ、2:休止、3:シャットダウン</summary>
        public SuspendCheckWindow(UInt32 mode = 0)
        {
            InitializeComponent();

            progressBar.Maximum = Settings.Instance.SuspendChkTime;
            progressBar.Value = progressBar.Maximum;

            var countTimer = new DispatcherTimer();
            countTimer.Interval = TimeSpan.FromSeconds(1);
            countTimer.Tick += (sender, e) =>
            {
                if (progressBar.Value-- == 0)
                {
                    button_work_now.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            };
            this.Loaded += (sender, e) => countTimer.Start();
            this.Closing += (sender, e) => countTimer.Stop();

            button_work_now.Click += (sender, e) => DialogResult = true;
            button_cancel.Focus();//スペースキーも効く

            var msg = new[] { "再起動", "スタンバイに移行", "休止に移行", "シャットダウン" };
            label_msg.Content = (CommonManager.Instance.NWMode == false ? "" : "録画サーバを")
                                + (mode > 3 ? " ？？ " : msg[mode])
                                + (CommonManager.Instance.NWMode == false ? "します。" : "させます。");
        }
    }
}
