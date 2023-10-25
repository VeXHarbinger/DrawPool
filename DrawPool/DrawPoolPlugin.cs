namespace DrawPool
{
    using Hearthstone_Deck_Tracker.Plugins;
    using System;
    using System.Reflection;
    using System.Windows.Controls;
    using Settings = DrawPool.Properties.Settings;

    /// <summary>
    /// The DrawPool plug-in Instance
    /// </summary>
    /// <seealso cref="Hearthstone_Deck_Tracker.Plugins.IPlugin" />
    public class DrawPoolPlugin : IPlugin
    {
        /// <summary>
        /// The <see cref="DrawPoolWindow">DrawPool Window</see> reference
        /// </summary>
        private DrawPoolWindow win;

        /// <summary>
        /// The author.
        /// </summary>
        /// <value>The author.</value>
        public string Author => "VeX Harbinger";

        /// <summary>
        /// The button text.
        /// </summary>
        /// <value>The button text.</value>
        public string ButtonText => "Settings";

        /// <summary>
        /// The plug-in description.
        /// </summary>
        /// <value>The plug-in description.</value>
        public string Description => @"Helps to see scoped draw pools from the current cards in your deck.";

        /// <summary>
        /// Gets or sets the main <see cref="MenuItem">Menu Item</see>.
        /// </summary>
        /// <value>The main <see cref="MenuItem">Menu Item</see>.</value>
        public MenuItem MenuItem { get; set; } = null;

        /// <summary>
        /// The plug-in name.
        /// </summary>
        /// <value>The plug-in name.</value>
        public string Name => "DrawPool";

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
                Settings.Default.Scale = 100;
                Settings.Default.Opacity = 1.00;
                Settings.Default.Top = 400;
                Settings.Default.Left = 300;
                Settings.Default.Save();
            }
            // Make Sure we save changes
            Settings.Default.PropertyChanged += (sender, e) => Settings.Default.Save();
        }

        /// <summary>
        /// Called when [button press].
        /// </summary>
        public void OnButtonPress()
        {
            win.CurrentView = DrawPool.ViewModes.Options;
            win.Show();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        public void OnLoad()
        {
            win = new DrawPoolWindow();
            win.InitializeDrawPool();
            AddMenuItem();
        }

        /// <summary>
        /// Called when during the window clean-up.
        /// </summary>
        public void OnUnload()
        {
            win.Close();
        }

        /// <summary>
        /// Called when [update].
        /// </summary>
        public void OnUpdate()
        {
        }
    }
}