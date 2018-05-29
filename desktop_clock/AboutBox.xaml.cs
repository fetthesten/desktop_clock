using System.Windows;

namespace desktop_clock
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();

            button_quit.Click += ClosePage;

        }

        private void ClosePage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
