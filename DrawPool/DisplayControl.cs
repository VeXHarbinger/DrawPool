namespace DrawPool
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
    using Settings = DrawPool.Properties.Settings;
    using System.ComponentModel;
    using Hearthstone_Deck_Tracker.Hearthstone;
    using System.Security.Cryptography;
    using DrawPool.Models;

    public partial class DisplayControl : UserControl
    {
        /// <summary>
        /// Occurs when [raise curtain] There is a display with data.
        /// </summary>
        public event EventHandler RaiseCurtain;

        /// <summary>
        /// Calculates the Draw the probability.
        /// </summary>
        /// <param name="poolsize">The pool size you will draw from.</param>
        /// <param name="copies">The number of copies of a card.</param>
        /// <param name="draw">The number of cards to draw.</param>
        /// <param name="dec">The decimal place to round to.</param>
        /// <returns>The Draw the probability.</returns>
        public Double DrawProbability(int poolsize, int copies = 1, int draw = 1, int dec = 0)
        {
            return Math.Round(
                Helper.DrawProbability(copies, poolsize, draw) * 100, dec);
        }

        /// <summary>
        /// Checks the panel to see if there is data to display.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal void ShowDisplay(object sender, EventArgs e)
        {
            if (this.RaiseCurtain != null && this.CardList.Items.Count > 0)
            {
                RaiseCurtain(sender, e);
            }
        }
    }
}