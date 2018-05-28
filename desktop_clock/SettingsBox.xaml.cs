using System.Configuration;
using System.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace desktop_clock
{
    /// <summary>
    /// Interaction logic for SettingsBox.xaml
    /// </summary>
    public partial class SettingsBox : Window
    {
        MainWindow mainWin;

        public SettingsBox(MainWindow win)
        {
            InitializeComponent();
            this.mainWin = win;

            List<string> fonts = new List<string>();

            using (InstalledFontCollection fontsCollection = new InstalledFontCollection())
            {
                FontFamily[] fontFamilies = fontsCollection.Families;
                
                foreach (FontFamily font in fontFamilies)
                {
                    fonts.Add(font.Name);
                }
            }

            list_fonts.ItemsSource = fonts;


            //foreach (string setting in ConfigurationManager.AppSettings.Keys)
            //{
            //    switch (setting)
            //    {
            //        case "font":

            //            break;
            //    }
            //}
        }
    }
}
