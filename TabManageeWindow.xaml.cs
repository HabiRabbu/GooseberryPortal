using System.Collections.ObjectModel;
using System.Windows;
using GooseberryPortalApp.Models;

namespace GooseberryPortalApp
{
    public partial class TabManagerWindow : Window
    {
        public ObservableCollection<TabInfo> Tabs { get; }

        public TabManagerWindow(ObservableCollection<TabInfo> tabs)
        {
            InitializeComponent();
            Tabs = tabs;
            TabList.ItemsSource = Tabs;
        }

        #region CRUD buttons

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (TabList.SelectedItem is TabInfo sel)
                Tabs.Remove(sel);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddTabWindow { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                Tabs.Add(new TabInfo
                {
                    Name = string.IsNullOrWhiteSpace(dlg.TabName) ? dlg.TabUrl : dlg.TabName,
                    Url = dlg.TabUrl,
                    Icon = dlg.IconFilePath
                });
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var i = TabList.SelectedIndex;
            if (i > 0)
                Tabs.Move(i, i - 1);
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var i = TabList.SelectedIndex;
            if (i >= 0 && i < Tabs.Count - 1)
                Tabs.Move(i, i + 1);
        }

        #endregion
    }
}
