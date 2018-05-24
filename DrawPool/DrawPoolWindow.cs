namespace DrawPool
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows;
    using Hearthstone_Deck_Tracker.API;
    using Settings = DrawPool.Properties.Settings;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using System.ComponentModel;
    using DrawPool.Models;

    /// <summary>
    /// Logic related to the DrawPoolWindow data processing
    /// </summary>
    /// <seealso cref="MahApps.Metro.Controls.MetroWindow"/>
    /// <seealso cref="System.Windows.Controls.UserControl"/>
    /// <seealso cref="System.Windows.Markup.IComponentConnector"/>
    public partial class DrawPoolWindow : MetroWindow
    {
        /// <summary>
        /// Gets or sets the current display view.
        /// </summary>
        /// <value>The current display view.</value>
        private static ViewModes currentView;

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
        /// Initializes the window.
        /// </summary>
        public void Initialize()
        {
            // Core.Game.PlayerMinionCount Helper.OptionsMain. GameEvents.OnPlayerCreateInDeck
            // GameEvents.OnPlayerGet GameEvents.OnTurnStart
            InitializeOpts();
            InitializeByCardModules();

            // Game Triggers
            GameEvents.OnGameStart.Add(Reset);
            GameEvents.OnGameEnd.Add(Reset);
            // User Triggers
            GameEvents.OnMouseOverOff.Add(OnMouseOff);

            Core.Game.Player.PropertyChanged += DeckCountChanged;
            Core.Game.Player.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Deck" || e.PropertyName == "DeckCount")
                {
                }
            };
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
        /// Initializes the Options.
        /// </summary>
        public void InitializeOpts()
        {
            currentView = ViewModes.Options;
            var uc = (UserOptionsControl)DisplayBox.FindChild<System.Windows.Controls.UserControl>(currentView.ToString());
            if (uc != null)
                uc.WinPositionButtonClick += new EventHandler(WinPositionMode);

            uc.btnDone.Click += (sender, args) =>
            {
                Settings.Default.Save();
                Display_Helper(new CurtainCall(), args);
            };
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            // ToDo : see how this loop needs to evolve with mechanics
            foreach (var dc in DisplayBox.Children)
            {
                if (dc is IDraw)
                {
                    ((IDraw)dc).Reset();
                }
            }
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Helper event for the Display control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Display_Helper(object sender, EventArgs e)
        {
            CurtainCall cc = (CurtainCall)sender;

            if (cc.ShouldShow)
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
        /// Deck count changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="PropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void DeckCountChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Deck" || e.PropertyName == "DeckCount")
            {
            }
        }

        /// <summary>
        /// Called when [mouse off].
        /// </summary>
        private void OnMouseOff()
        {
            Visibility = Visibility.Collapsed;
        }
    }
}