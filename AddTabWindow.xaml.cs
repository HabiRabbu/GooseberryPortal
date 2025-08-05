using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace GooseberryPortalApp
{
    public partial class AddTabWindow : Window
    {
        public string TabName => NameBox.Text.Trim();
        public string TabUrl { get; private set; } = string.Empty;
        public string IconFilePath { get; private set; } = string.Empty;

        public AddTabWindow() => InitializeComponent();

        private void ChooseIcon_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.ico)|*.png;*.jpg;*.ico",
                Title = "Select Icon"
            };
            if (dlg.ShowDialog(this) == true)
            {
                IconFilePath = dlg.FileName;
                IconPathText.Text = Path.GetFileName(dlg.FileName);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UrlBox.Text))
            {
                MessageBox.Show(this, "Please enter a URL.",
                                "Missing URL",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TabUrl = UrlBox.Text.Trim();
            DialogResult = true; // closes window
        }
    }
}
