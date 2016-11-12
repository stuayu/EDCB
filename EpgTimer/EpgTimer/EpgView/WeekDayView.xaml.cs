﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace EpgTimer.EpgView
{
    /// <summary>
    /// WeekDayView.xaml の相互作用ロジック
    /// </summary>
    public partial class WeekDayView : UserControl
    {
        public WeekDayView()
        {
            InitializeComponent();
            this.Background = CommonManager.Instance.EpgWeekdayBorderColor;
        }

        public void ClearInfo()
        {
            stackPanel_day.Children.Clear();
        }

        public void SetDay(List<DateTime> dayList)
        {
            try
            {
                stackPanel_day.Children.Clear();
                foreach (DateTime time in dayList)
                {
                    var item = new TextBlock();

                    item.Width = Settings.Instance.ServiceWidth - 1;
                    item.Text = time.ToString("M/d\r\n(ddd)");

                    Color backgroundColor;
                    if (time.DayOfWeek == DayOfWeek.Saturday)
                    {
                        item.Foreground = Brushes.DarkBlue;
                        backgroundColor = Colors.Lavender;
                    }
                    else if (time.DayOfWeek == DayOfWeek.Sunday)
                    {
                        item.Foreground = Brushes.DarkRed;
                        backgroundColor = Colors.MistyRose;
                    }
                    else
                    {
                        item.Foreground = Brushes.Black;
                        backgroundColor = Colors.White;
                    }
                    if (Settings.Instance.EpgGradationHeader == false)
                    {
                        item.Background = new SolidColorBrush(backgroundColor);
                    }
                    else
                    {
                        item.Background = ColorDef.GradientBrush(backgroundColor, 0.8);
                    }

                    item.Padding = new Thickness(0, 0, 0, 2);
                    item.Margin = new Thickness(0, 1, 1, 1);
                    item.TextAlignment = TextAlignment.Center;
                    item.FontSize = 12;
                    item.FontWeight = FontWeights.Bold;
                    stackPanel_day.Children.Add(item);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace); }
        }
    }
}
