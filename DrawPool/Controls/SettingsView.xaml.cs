namespace DrawPool.Controls
{
    using global::DrawPool.DrawLogic;
    using global::DrawPool.Logic;
    using MahApps.Metro.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ScrollViewer
    {
        private static Flyout _flyout;

        public SettingsView()
        {
            InitializeComponent();
        }

        public static Flyout Flyout
        {
            get
            {
                if (_flyout == null)
                {
                    _flyout = CreateSettingsFlyout();
                }
                return _flyout;
            }
        }

        public static bool IsUnlocked { get; private set; }
        public IEnumerable<Orientation> OrientationTypes => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();

        private static Flyout CreateSettingsFlyout()
        {
            var settings = new Flyout();
            settings.Position = Position.Left;
            Panel.SetZIndex(settings, 100);
            settings.Header = StringTools.GetLocalized("PluginName") + " " + StringTools.GetLocalized("SettingsLabel");
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        private void BtnShowHide_Click(object sender, RoutedEventArgs e)
        {
            MinstrelPool poolView = Core.OverlayCanvas.FindChild<MinstrelPool>("MinstrelPoolView");
            bool wasVisible = poolView.PoolIsVisibile() || false;

            if (wasVisible)
            {
                BtnShowHide.Content = StringTools.GetLocalized("ShowLabel");
                poolView.Hide();
            }
            else
            {
                BtnShowHide.Content = StringTools.GetLocalized("HideLabel");
                poolView.Show();
            }
        }

        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            IsUnlocked = DrawPool.inputMoveManager.Toggle();
            if (!IsUnlocked)
            {
                BtnUnlock.Content = StringTools.GetLocalized("UnlockLabel");
                BtnShowHide.IsEnabled = true;
            }
            else
            {
                BtnUnlock.Content = StringTools.GetLocalized("LockLabel");
                BtnShowHide.Content = StringTools.GetLocalized("HideLabel");
                BtnShowHide.IsEnabled = false;
            }
        }

        private void Translate(Flyout settings)
        {
            BtnUnlock.Content = StringTools.GetLocalized("UnlockLabel");
            BtnShowHide.Content = StringTools.GetLocalized("ShowLabel");

            //MinstrelToggleSwitch MinstrelLabel
        }
    }
}