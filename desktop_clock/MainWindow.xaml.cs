using System;
using System.Windows;
using System.Windows.Input;
using System.Configuration;

using System.Threading;

namespace desktop_clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer clockUpdateTimer;
        public MainWindow()
        {
            InitializeComponent();

            var pos = LoadSettings();
            if (pos != String.Empty)
            {
                string[] coords = pos.Split(',');
                var windowPos = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
                this.Left = windowPos.X;
                this.Top = windowPos.Y;
            }

            clockUpdateTimer = new System.Timers.Timer(500);
            clockUpdateTimer.Elapsed += UpdateClock;
            clockUpdateTimer.Start();

            this.MouseDown += MoveWindow;
            this.MouseUp += SaveSettings;
        }

        private void ShowSettingsBox(object sender, RoutedEventArgs e)
        {
            
        }

        private void ShowAboutBox(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Left = this.Left;
            aboutBox.Top = this.Top;
            aboutBox.Show();
        }

        private void QuitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private string LoadSettings()
        {
            var result = String.Empty;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings["window_pos"] ?? String.Empty;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("error reading appsettings");
            }
            return result;
        }

        private void SaveSettings(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var cfgFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = cfgFile.AppSettings.Settings;
                var pos = new Point(this.Left, this.Top);
                if (settings["window_pos"] == null)
                    settings.Add("window_pos", pos.ToString());
                else
                    settings["window_pos"].Value = pos.ToString();
                cfgFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(cfgFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("error writing appsettings");
            }
        }

        private void MoveWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void UpdateClock(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() => l_ClockDisplay.Content = DateTime.Now.ToString("HH.mm")));
        }

        public void Test(string test)
        {
            Console.WriteLine(test);
        }
    }
}
