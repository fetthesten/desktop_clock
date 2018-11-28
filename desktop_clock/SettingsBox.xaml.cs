using System.Windows;
using System.Drawing;
using System.Drawing.Text;
using System.Collections.Generic;
using System;
using System.Windows.Controls;

namespace desktop_clock
{
    /// <summary>
    /// Interaction logic for SettingsBox.xaml
    /// </summary>
    public partial class SettingsBox : Window
    {
        private MainWindow mainWin;

        private Dictionary<string, FontListItem> fonts;
        private int[] color;
        public class FontListItem
        {
            public string name { get; set; }
            public FontFamily fontFamily { get; set; }

            public FontListItem(string name, FontFamily fontFamily)
            {
                this.name = name;
                this.fontFamily = fontFamily;
            }
        }

        public SettingsBox(MainWindow win)
        {
            this.mainWin = win;
            InitializeComponent();

            color = new int[3];

            // list fonts
            fonts = new Dictionary<string, FontListItem>();
            using (InstalledFontCollection fontsCollection = new InstalledFontCollection())
            {
                FontFamily[] fontFamilies = fontsCollection.Families;

                int currentIndex = 0;
                foreach (FontFamily font in fontFamilies)
                {
                    if (font.Name != string.Empty)
                    {
                        var fontListItem = new FontListItem(font.Name, font);
                        fonts.Add(font.Name, fontListItem);
                        list_fonts.Items.Add(fontListItem);
                        if (font.Name == Properties.Settings.Default.font_family.Split(',')[0])
                            list_fonts.SelectedIndex = currentIndex;
                        currentIndex++;
                    }
                }
            }

            // load app settings
            list_fonts.SelectionMode = System.Windows.Controls.SelectionMode.Single;
            slider_textsize.Value = Properties.Settings.Default.font_size;
            l_textsizelabel.Content = "Size: " + Properties.Settings.Default.font_size.ToString();
            check_bold.IsChecked = Properties.Settings.Default.font_bold;
            check_shadow.IsChecked = Properties.Settings.Default.font_shadow;
            check_movesnap.IsChecked = Properties.Settings.Default.move_snap;
            int i = 0;
            foreach (ListBoxItem item in combo_separator.Items)
            {
                if (((string)item.Content)[0] == Properties.Settings.Default.clock_separator)
                    combo_separator.SelectedIndex = i;
                i++;
            }

            var color_str = Properties.Settings.Default.font_color.Split(',');
            for (int c = 0; c < 3; c++)
                int.TryParse(color_str[c], out color[c]);
            slider_red.Value = color[0];
            slider_green.Value = color[1];
            slider_blue.Value = color[2];
            tx_red.Text = color[0].ToString();
            tx_green.Text = color[1].ToString();
            tx_blue.Text = color[2].ToString();

            UpdateColorPreview();

            slider_red.ValueChanged += UpdateColorBoxes;
            slider_green.ValueChanged += UpdateColorBoxes;
            slider_blue.ValueChanged += UpdateColorBoxes;
            tx_red.TextChanged += UpdateColorSliders;
            tx_green.TextChanged += UpdateColorSliders;
            tx_blue.TextChanged += UpdateColorSliders;
            slider_textsize.ValueChanged += UpdateTextSize;
            button_saveSettings.Click += SaveAndClose;
        }

        private void UpdateColorSliders(object sender, TextChangedEventArgs e)
        {
            int.TryParse(tx_red.Text, out color[0]);
            int.TryParse(tx_green.Text, out color[1]);
            int.TryParse(tx_blue.Text, out color[2]);
            slider_red.Value = color[0];
            slider_green.Value = color[1];
            slider_blue.Value = color[2];

            UpdateColorPreview();
        }

        private void UpdateColorBoxes(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            color[0] = Convert.ToInt32(slider_red.Value);
            color[1] = Convert.ToInt32(slider_green.Value);
            color[2] = Convert.ToInt32(slider_blue.Value);
            tx_red.Text = color[0].ToString();
            tx_green.Text = color[1].ToString();
            tx_blue.Text = color[2].ToString();

            UpdateColorPreview();
        }

        private void UpdateColorPreview()
        {
            var bgcol = new System.Windows.Media.Color
            {
                R = (byte)color[0],
                G = (byte)color[1],
                B = (byte)color[2],
                A = 255
            };
            var bg = new System.Windows.Media.SolidColorBrush(bgcol);
            img_colorpreview.Background = bg;
        }

        private void SaveAndClose(object sender, RoutedEventArgs e)
        {
            FontListItem selectedFont = (FontListItem)list_fonts.SelectedItem;
            if (selectedFont == null)
                selectedFont = fonts["Segoe UI"];
            Properties.Settings.Default.font_family = selectedFont.name;
            Properties.Settings.Default.font_size = Convert.ToInt32(slider_textsize.Value);
            Properties.Settings.Default.font_bold = (bool)check_bold.IsChecked;
            Properties.Settings.Default.font_shadow = (bool)check_shadow.IsChecked;
            var sep_item = (ListBoxItem)combo_separator.SelectedItem;
            var sep_string = (string)sep_item.Content;
            Properties.Settings.Default.clock_separator = sep_string[0];
            Properties.Settings.Default.font_color = color[0].ToString() + "," + color[1].ToString() + "," + color[2].ToString();
            Properties.Settings.Default.move_snap = (bool)check_movesnap.IsChecked;

            mainWin.LoadSettings();
            this.Close();
        }

        private void UpdateTextSize(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            l_textsizelabel.Content = "Size: " + Convert.ToInt32(slider_textsize.Value).ToString();
        }
    }
}
