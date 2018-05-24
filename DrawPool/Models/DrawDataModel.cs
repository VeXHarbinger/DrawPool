namespace DrawPool.Models
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Hearthstone_Deck_Tracker.Controls;
    using System.Collections.Generic;
    using System.Linq;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using Helper = Hearthstone_Deck_Tracker.Helper;
    using Hearthstone_Deck_Tracker;
    using Hearthstone_Deck_Tracker.API;
    using static Hearthstone_Deck_Tracker.Windows.MessageDialogs;

    public class CurtainCall
    {
        /// <summary>
        /// Gets or sets the calling view, that is sending the data.
        /// </summary>
        /// <value>The calling view.</value>
        public ViewModes CallingView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [should show] the window or hide it.
        /// </summary>
        /// <value><c>true</c> if [should show]; the window otherwise, <c>false</c> hide it.</value>
        public bool ShouldShow { get; set; } = false;
    }
}