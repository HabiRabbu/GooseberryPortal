using System;

namespace GooseberryPortalApp.Models
{
    /// <summary>POCO that describes a single sidebar tab.</summary>
    public sealed class TabInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        public TabInfo Clone() => (TabInfo)MemberwiseClone();
    }
}
