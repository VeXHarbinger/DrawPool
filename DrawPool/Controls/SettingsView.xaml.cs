namespace DrawPool.Controls
{
    using DrawPool.DrawLogic;
    using MahApps.Metro.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using Strings = Logic.StringTools;

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

        public IEnumerable<Orientation> OrientationTypes => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();

        private static Flyout CreateSettingsFlyout()
        {
            var settings = new Flyout();
            settings.Position = Position.Left;
            Panel.SetZIndex(settings, 100);
            settings.Header = Strings.GetLocalized("PluginName") + " " + Strings.GetLocalized("SettingsLabel");
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        private void BtnShowHide_Click(object sender, RoutedEventArgs e)
        {
            MinstrelPool poolView = Core.OverlayCanvas.FindChild<MinstrelPool>("MinstrelPoolView");
            bool wasVisible = poolView.PoolIsVisibile();

            if (wasVisible)
            {
                BtnShowHide.Content = Strings.GetLocalized("ShowLabel");
                poolView.Hide();
            }
            else
            {
                BtnShowHide.Content = Strings.GetLocalized("HideLabel");
                poolView.Show();
            }
        }

        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            MinstrelPool poolView = Core.OverlayCanvas.FindChild<MinstrelPool>("MinstrelPoolView");
            bool wasSet = poolView.IsPoolWindowDraggable();
            if (wasSet)
            {
                BtnUnlock.Content = Strings.GetLocalized("UnlockLabel");
                BtnShowHide.IsEnabled = true;
                poolView.Pin();
            }
            else
            {
                BtnUnlock.Content = Strings.GetLocalized("LockLabel");
                BtnShowHide.Content = Strings.GetLocalized("HideLabel");
                BtnShowHide.IsEnabled = false;
                poolView.UnPin();
            }
        }
    }
}