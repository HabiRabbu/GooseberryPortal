using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GooseberryPortalApp.Models;
using GooseberryPortalApp.Services;
using Microsoft.Web.WebView2.Core;

namespace GooseberryPortalApp
{
    public partial class MainWindow : Window
    {
        #region fields

        private readonly ObservableCollection<TabInfo> tabs =
            new ObservableCollection<TabInfo>();

        private readonly Dictionary<string, Button> tabUrlMap =
            new(StringComparer.OrdinalIgnoreCase);

        private Button? currentActiveButton;
        private string currentUrl = string.Empty;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            InitBrowser();
            Closing += (_, _) => TabStorage.SaveTabs(tabs);
        }

        #region Initialisation

        private void InitBrowser()
        {
            MainWebView.CoreWebView2InitializationCompleted += WebView2_Initialized;
            _ = MainWebView.EnsureCoreWebView2Async();
        }

        private void WebView2_Initialized(object? sender,
                                          CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show($"WebView2 failed: {e.InitializationException?.Message}",
                                "Startup error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWebView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            MainWebView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            // ---------- load defaults then override with saved file ----------
            tabs.Clear();

            var saved = TabStorage.LoadTabs(tabs);
            if (saved.Count > 0)
            {
                tabs.Clear();
                foreach (var t in saved) tabs.Add(t);
            }

            BuildSidebarFromTabs();

            // select first real tab (index 1 after cog)
            if (TabPanel.Children.Count > 1 && TabPanel.Children[1] is Button firstBtn)
                TabButton_Click(firstBtn, null);
        }

        #endregion

        #region Sidebar

        private void BuildSidebarFromTabs()
        {
            // keep cog (child 0), remove everything after
            while (TabPanel.Children.Count > 1)
                TabPanel.Children.RemoveAt(1);

            tabUrlMap.Clear();

            int insertIndex = 1; // after cog
            foreach (var t in tabs)
            {
                var btn = CreateTabButton(t);
                TabPanel.Children.Insert(insertIndex++, btn);
                tabUrlMap[t.Url.TrimEnd('/')] = btn;
            }
        }

        // now an **instance** method
        private Button CreateTabButton(TabInfo t)
        {
            var btn = new Button
            {
                Tag = t.Url,
                Style = (Style)FindResource("TabButtonStyle")
            };

            var stack = new StackPanel { Orientation = Orientation.Horizontal };
            if (!string.IsNullOrWhiteSpace(t.Icon) && File.Exists(t.Icon))
            {
                stack.Children.Add(new Image
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(t.Icon)),
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(0, 0, 6, 0)
                });
            }
            stack.Children.Add(new TextBlock { Text = t.Name });
            btn.Content = stack;

            btn.Click += TabButton_Click;
            return btn;
        }

        #endregion

        #region WebView link interception

        private void CoreWebView2_NavigationStarting(object? sender,
                                                     CoreWebView2NavigationStartingEventArgs e)
        {
            string target = e.Uri?.TrimEnd('/') ?? string.Empty;
            if (string.IsNullOrEmpty(target)) return;

            foreach (var (knownUrl, button) in tabUrlMap)
            {
                if (!target.StartsWith(knownUrl, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (button == currentActiveButton) return;   // same tab
                e.Cancel = true;
                TabButton_Click(button, null);               // switch
                return;
            }
        }

        private void CoreWebView2_NewWindowRequested(object? sender,
                                                     CoreWebView2NewWindowRequestedEventArgs e)
        {
            string target = e.Uri?.TrimEnd('/') ?? string.Empty;
            if (string.IsNullOrEmpty(target)) return;

            foreach (var (knownUrl, button) in tabUrlMap)
            {
                if (!target.StartsWith(knownUrl, StringComparison.OrdinalIgnoreCase))
                    continue;

                e.Handled = true;
                TabButton_Click(button, null);
                return;
            }

            // external → default browser
            e.Handled = true;
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(target)
                {
                    UseShellExecute = true
                });
            }
            catch { /* ignore */ }
        }

        #endregion

        #region Tab click handler

        private void TabButton_Click(object? sender, RoutedEventArgs? e)
        {
            if (sender is not Button btn) return;
            string url = btn.Tag as string ?? string.Empty;
            if (string.IsNullOrWhiteSpace(url)) return;

            if (btn == currentActiveButton && url == currentUrl)
            {
                MainWebView.CoreWebView2?.Reload();
                return;
            }

            currentActiveButton?.ClearValue(BackgroundProperty);

            btn.Background = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xCC));
            currentActiveButton = btn;
            currentUrl = url;
            MainWebView.Source = new Uri(url);
        }

        #endregion

        #region Settings dialog

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new TabManagerWindow(tabs) { Owner = this };
            dlg.ShowDialog();

            BuildSidebarFromTabs();
            TabStorage.SaveTabs(tabs);
        }

        #endregion
    }
}
