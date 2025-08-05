using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GooseberryPortalApp.Models;

namespace GooseberryPortalApp.Services
{
    /// <summary>Persists the tab list to %APPDATA%\GooseberryPortal\tabs.json.</summary>
    internal static class TabStorage
    {
        private static readonly string Folder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "GooseberryPortal");

        private static readonly string FilePath = Path.Combine(Folder, "tabs.json");

        public static IList<TabInfo> LoadTabs(IList<TabInfo> defaults)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    var data = JsonSerializer.Deserialize<List<TabInfo>>(json);
                    if (data is { Count: > 0 }) return data;
                }
            }
            catch
            {
                // corrupted JSON – fall back to defaults silently
            }

            return new List<TabInfo>(defaults);
        }

        public static void SaveTabs(IEnumerable<TabInfo> tabs)
        {
            Directory.CreateDirectory(Folder);

            var json = JsonSerializer.Serialize(tabs,
                           new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(FilePath, json);
        }
    }
}
