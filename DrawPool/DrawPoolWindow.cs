namespace DrawPool
{
    using DrawPool.Models;
    using Hearthstone_Deck_Tracker.API;
    using MahApps.Metro.Controls;
    using System;
    using System.Reflection;
    using System.Windows;
    using Settings = DrawPool.Properties.Settings;

    /// <summary>
    /// Logic related to the DrawPoolWindow data processing
    /// </summary>
    /// <seealso cref="MahApps.Metro.Controls.MetroWindow" />
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class DrawPoolWindow : MetroWindow
    {
        /// <summary>
        /// Gets or sets the current display view.
        /// </summary>
        /// <value>The current display view.</value>
        private ViewModes currentView;

        /// <summary>
        /// Gets or sets the currently displayed view.
        /// </summary>
        /// <value>The currently displayed view.</value>
        public ViewModes CurrentView
        {
            get { return currentView; }
            set
            {
                DisplayBox.FindChild<System.Windows.Controls.UserControl>(currentView.ToString()).Visibility = Visibility.Collapsed;
                currentView = value;
                DisplayBox.FindChild<System.Windows.Controls.UserControl>(currentView.ToString()).Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Called when [mouse off].
        /// </summary>
        private void OnMouseOff()
        {
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Helper event for the Display control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Display_Helper(object sender, EventArgs e)
        {
            CurtainCall cc = (CurtainCall)sender;
            if (cc != null && cc.ShouldShow)
            {
                CurrentView = cc.CallingView;
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Initializes the By Card modules.
        /// </summary>
        public void InitializeByCardModules()
        {
            if (Settings.Default.IsPiperEnabled)
            {
                var dc = new WitchWoodPiperControl()
                {
                    Name = "WitchWoodPiper"
                };
                dc.RaiseCurtain += new EventHandler(Display_Helper);
                DisplayBox.Children.Add(dc);
            }
            if (Settings.Default.IsMinstrelEnabled)
            {
                var dc = new ElvenMinstrelControl()
                {
                    Name = "ElvenMinstrel"
                };
                dc.RaiseCurtain += new EventHandler(Display_Helper);
                DisplayBox.Children.Add(dc);
            }
        }

        /// <summary>
        /// Initializes the window.
        /// </summary>
        public void InitializeDrawPool()
        {
            InitializeWinOpts();
            InitializeOpts();
            InitializeByCardModules();

            // Game Triggers
            GameEvents.OnGameStart.Add(Reset);
            GameEvents.OnGameEnd.Add(Reset);
            // User Triggers
            GameEvents.OnMouseOverOff.Add(OnMouseOff);
        }

        /// <summary>
        /// Initializes the window options, to resolve control transparency.
        /// </summary>
        private void InitializeWinOpts()
        {

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = System.Windows.Media.Brushes.Transparent;
        }

        /// <summary>
        /// Initializes the Options.
        /// </summary>
        public void InitializeOpts()
        {
            UserOptionsControl uc = (UserOptionsControl)DisplayBox.FindChild<System.Windows.Controls.UserControl>(ViewModes.Options.ToString());
            if (uc != null)
            {
                uc.btnToggle.Checked += (sender, args) =>
                {
                    uc.btnToggle.Content = "Lock";
                    IsWindowDraggable = true;
                };
                uc.btnToggle.Unchecked += (sender, args) =>
                {
                    uc.btnToggle.Content = "Unlock";
                    IsWindowDraggable = false;
                };
                uc.btnDone.Click += (sender, args) =>
                {
                    uc.btnToggle.IsChecked = false;
                    uc.Visibility = Visibility.Hidden;
                    Visibility = Visibility.Collapsed;
                };
                uc.lblVersionValue.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            // ToDo : see how this loop needs to evolve with mechanics for recruit
            foreach (IDraw dc in DisplayBox.Children)
            {
                dc.Reset();
            }
            Visibility = Visibility.Collapsed;
        }
    }
}