namespace DrawPool
{
    using DrawPool.Logic;
    using global::DrawPool.Controls;
    using Hearthstone_Deck_Tracker.Plugins;
    using System;
    using System.Reflection;
    using System.Windows.Controls;
    using Settings = Properties.Settings;

    /// <summary>
    /// The DrawPool plug-in Instance
    /// </summary>
    /// <seealso cref="Hearthstone_Deck_Tracker.Plugins.IPlugin" />
    public class DrawPoolPlugin : IPlugin
    {
        internal DrawPoolInstance drawPoolInstance;

        /// <summary>
        /// The author.
        /// </summary>
        /// <value>The author.</value>
        public string Author => "VeX Harbinger";

        /// <summary>
        /// The button text.
        /// </summary>
        /// <value>The button text.</value>
        public string ButtonText => StringTools.GetLocalized("SettingsLabel");

        /// <summary>
        /// The plug-in description.
        /// </summary>
        /// <value>The plug-in description.</value>
        public string Description => StringTools.GetLocalized("PluginDescription");

        /// <summary>
        /// Gets or sets the main <see cref="MenuItem">Menu Item</see>.
        /// </summary>
        /// <value>The main <see cref="MenuItem">Menu Item</see>.</value>
        public MenuItem MenuItem { get; set; } = null;

        /// <summary>
        /// The plug-in name.
        /// </summary>
        /// <value>The plug-in name.</value>
        public string Name => StringTools.GetLocalized("PluginName");

        /// <summary>
        /// The plugin version.
        /// </summary>
        /// <value>The plugin version.</value>
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Adds the menu item.
        /// </summary>
        private void AddMenuItem()
        {
            this.MenuItem = new MenuItem()
            {
                Header = Name
            };

            this.MenuItem.Click += (sender, args) =>
            {
                OnButtonPress();
            };
        }

        /// <summary>
        /// Checks for default settings, or applies them if missing.
        /// </summary>
        protected void CheckForDefaultSettings()
        {
            if (Settings.Default == null)
            {
                Settings.Default.IsMinstrelEnabled = Properties.Settings.Default.IsMinstrelEnabled || true;
                Settings.Default.IsPiperEnabled = Properties.Settings.Default.IsPiperEnabled || false;
                Settings.Default.DrawPoolScale = 100;
                Settings.Default.DrawPoolOpacity = 1.00;
                Settings.Default.DrawPoolTop = 8;
                Settings.Default.DrawPoolLeft = 8;
                Settings.Default.Save();
            }
            // Make Sure we save changes
            Settings.Default.PropertyChanged += (sender, e) => Settings.Default.Save();
        }

        public void OnButtonPress() => SettingsView.Flyout.IsOpen = true;

        public void OnLoad()
        {
            drawPoolInstance = new DrawPoolInstance();

            AddMenuItem();
        }

        /// <summary>
        /// Called when during the window clean-up.
        /// </summary>
        public void OnUnload()
        {
            Settings.Default.Save();

            drawPoolInstance?.Dispose();
            drawPoolInstance = null;
        }

        /// <summary>
        /// Called when [update].
        /// </summary>
        public void OnUpdate()
        { }
    }
}