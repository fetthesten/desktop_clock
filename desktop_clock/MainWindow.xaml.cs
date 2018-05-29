using System;
using System.Windows;
using System.Windows.Input;
using System.Configuration;
using System.Collections.Generic;

using System.Threading;

namespace desktop_clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string STR_NULL = "";

        private System.Timers.Timer clockUpdateTimer;

        public MainWindow()
        {
            InitializeComponent();

            LoadSettings();

            clockUpdateTimer = new System.Timers.Timer(500);
            clockUpdateTimer.Elapsed += UpdateClock;
            clockUpdateTimer.Start();

            this.MouseDown += MoveWindow;
            this.MouseUp += SetWindowPos;
        }

        private void ShowSettingsBox(object sender, RoutedEventArgs e)
        {
            var settingsBox = new SettingsBox(this)
            {
                Left = this.Left,
                Top = this.Top
            };
            settingsBox.Show();
        }

        private void ShowAboutBox(object sender, RoutedEventArgs e)
        {
            var aboutBox = new AboutBox
            {
                Left = this.Left,
                Top = this.Top
            };
            aboutBox.Show();
        }

        private void QuitApp(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        public void LoadSettings()
        {
            var fontFamily = new System.Windows.Media.FontFamily(Properties.Settings.Default.font_family);
            l_ClockDisplay.FontFamily = fontFamily;
            l_ClockDisplay.FontSize = Properties.Settings.Default.font_size;
            l_ClockDisplay.FontWeight = Properties.Settings.Default.font_bold ? FontWeights.Bold : FontWeights.Regular;

            var c = Properties.Settings.Default.font_color.Split(',');
            byte[] col = new byte[3];
            for (int i = 0; i < 3; i++)
                byte.TryParse(c[i], out col[i]);
            var fgcol = new System.Windows.Media.Color
            {
                R = col[0],
                G = col[1],
                B = col[2],
                A = 255
            };
            var fg = new System.Windows.Media.SolidColorBrush(fgcol);
            l_ClockDisplay.Foreground = fg;

            this.Left = Properties.Settings.Default.window_pos.X;
            this.Top = Properties.Settings.Default.window_pos.Y;
        }

        private void SetWindowPos(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.window_pos = new Point(this.Left, this.Top);
        }

        private void MoveWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void UpdateClock(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() => l_ClockDisplay.Content = DateTime.Now.ToString("HH" + Properties.Settings.Default.clock_separator +  "mm")));
        }
    }
}
