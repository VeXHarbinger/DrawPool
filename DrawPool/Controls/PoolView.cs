﻿namespace DrawPool.Controls
{
    using Hearthstone_Deck_Tracker;
    using Hearthstone_Deck_Tracker.Controls;
    using Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    public partial class PoolView : StackPanel
    {
        /// <summary>
        /// The deck hash reference
        /// </summary>
        private int deckHash;

        /// <summary>
        /// The list of <see cref="Card">Cards</see>
        /// </summary>
        public List<Card> Cards;

        protected AnimatedCardList AnimatedCardLister => this.AnimatedCards;
        protected HearthstoneTextBlock Chance1 => (HearthstoneTextBlock)this.FindName("LblDrawChance1");
        protected HearthstoneTextBlock Chance2 => (HearthstoneTextBlock)this.FindName("LblDrawChance2");
        protected double ScreenRatio => (4.0 / 3.0) / (Core.OverlayCanvas.Width / Core.OverlayCanvas.Height);

        /// <summary>
        /// Gets the deck Hash Code.
        /// </summary>
        /// <value>The deck HashCode.</value>
        public int DeckHash => Core.Game.Player.Deck.GetHashCode();

        /// <summary>
        /// The Hearthstone Text Block With the view's styled Title within
        /// </summary>
        public HearthstoneTextBlock poolTitleLabel => (HearthstoneTextBlock)this.FindName("BlockTitleText");

        /// <summary>
        /// Returns the list of Minions Grouped by their Counts, for statistical purposes.
        /// </summary>
        /// <returns>The list of Minions Grouped by their Counts, for statistical purposes</returns>
        protected List<IGrouping<int, Card>> GroupedMinion() => Cards.GroupBy(c => c.Count).OrderByDescending(grp => grp.Count()).OrderBy(g => g.Key).ToList();

        /// <summary>
        /// Gets or sets the minion count.
        /// </summary>
        /// <value>The minion count.</value>
        protected int MinionCount() => Cards.Sum(c => c.Count);

        /// <summary>
        /// Writes the deck mix of minions in cards.
        /// </summary>
        /// <param name="possibleDraw">The count of possible cards that could be draw.</param>
        /// <param name="deckCount">The count of cards in the deck.</param>
        /// <returns>The deck mix of minions in cards as Minion Count/Deck Count</returns>
        protected string WriteDeckMix(int possibleDraw, int deckCount)
        {
            return $"{possibleDraw}";
        }

        /// <summary>
        /// Writes the formatted draw probability text.
        /// </summary>
        /// <param name="c">The <see cref="Card" />.</param>
        /// <param name="minionCount">The minion count.</param>
        /// <returns>The formatted draw probability text</returns>
        protected string WriteDrawProbability(int copyCount, int minionCount, int drawCount)
        {
            return $"{DrawProbability(minionCount, copyCount, drawCount)}%";
        }

        /// <summary>
        /// Checks if the deck changed since the last time we displayed the view.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the Deck has Changed;otherwise, <c>false</c>
        /// </returns>
        public bool CheckDeckChanged() => (deckHash != Core.Game.Player.Deck.GetHashCode());

        /// <summary>
        /// Calculates the Draw the probability.
        /// </summary>
        /// <param name="poolsize">The pool size you will draw from.</param>
        /// <param name="copies">The number of copies of a card.</param>
        /// <param name="draw">The number of cards to draw.</param>
        /// <param name="dec">The decimal place to round to.</param>
        /// <returns>The Draw probability.</returns>
        public Double DrawProbability(int poolsize, int copies = 1, int draw = 1, int dec = 1)
        {
            return Math.Round(Helper.DrawProbability(copies, poolsize, draw) * 100, dec);
        }

        /// <summary>
        /// Collapsed the view.
        /// </summary>
        public void Hide()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Called when the mouse focus moves off the card.
        /// </summary>
        public void OnMouseOff()
        {
            Hide();
        }

        public void SetTitle()
        {
            this.poolTitleLabel.Text = StringTools.GetLocalized("MinstrelLabel") + " " + StringTools.GetLocalized("PluginName");
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void Show()
        {
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public bool Update(Card card)
        {
            if (card.Type != "Minion")
            {
                return false;
            }

            var match = Cards.FirstOrDefault(c => c.Name == card.Name);

            if (match != null)
            {
                match.Count++;
            }
            else
            {
                Cards.Add(card.Clone() as Card);
            }

            return true;
        }
    }
}