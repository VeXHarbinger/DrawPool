namespace DrawPool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Helper = Hearthstone_Deck_Tracker.Helper;

    /// <summary>
    /// The Possible Draw List Display Control
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class DisplayControl : UserControl
    {
        /// <summary>
        /// The deck hash reference
        /// </summary>
        private int deckHash;

        /// <summary>
        /// Occurs when [raise curtain], meaning there is a display with data.
        /// </summary>
        public event EventHandler RaiseCurtain;

        /// <summary>
        /// Gets the deck hash.
        /// </summary>
        /// <value>The deck hash.</value>
        public int DeckHash => Hearthstone_Deck_Tracker.API.Core.Game.Player.Deck.GetHashCode();

        /// <summary>
        /// The <see cref="PlayerCardList">Deck</see> object reference for the <see cref="Card">Cards</see> data.
        /// </summary>
        /// <value><returns>The list of current <see cref="Card">Cards</see></returns></value>
        public List<Card> QueryDeck { get; set; }

        /// <summary>
        /// Queries the <see cref="PlayerCardList">Deck</see> for specific scoped <see cref="Card">Cards</see>.
        /// </summary>
        /// <returns>The scoped list of <see cref="Card">Cards</see></returns>
        internal virtual List<Card> BuildQueryDeck()
        {
            var playerDeck = Hearthstone_Deck_Tracker.API.Core.Game.Player
                .PlayerCardList
                .Where(c =>
                    c.Type == "Minion" &&
                    (c.Count - c.InHandCount) > 0
                )
                .OrderBy(c => c.Cost)
                .ThenBy(c => c.Count)
                .ThenBy(c => c.Name)
                .ToList<Card>()
                .FixCreatedCards()
                .FixDuplicateCards();
            return playerDeck;
        }

        /// <summary>
        /// Checks if the deck has changed, since the last check..
        /// </summary>
        /// <returns>True, if the deck has changed, since the last check.</returns>
        internal bool CheckDeckChanged()
        {
            if (deckHash != Hearthstone_Deck_Tracker.API.Core.Game.Player.Deck.GetHashCode())
            {
                deckHash = Hearthstone_Deck_Tracker.API.Core.Game.Player.Deck.GetHashCode();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the minion count.
        /// </summary>
        /// <value>The minion count.</value>
        internal int MinionCount() => QueryDeck.Sum(c => c.Count);

        /// <summary>
        /// Checks the panel to see if there is data to display.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        internal void ShowDisplay(object sender, EventArgs e)
        {
            if (this.RaiseCurtain != null)  // hack: && this.CardList.Items.Count > 0
            {
                RaiseCurtain(sender, e);
            }
        }

        /// <summary>
        /// Writes the deck mix of minions in cards.
        /// </summary>
        /// <param name="possibleDraw">The count of possible cards that could be draw.</param>
        /// <param name="deckCount">The count of cards in the deck.</param>
        /// <returns>The deck mix of minions in cards as Minion Count/Deck Count</returns>
        internal string WriteDeckMix(int possibleDraw, int deckCount)
        {
            return $"{possibleDraw}/{deckCount}";
        }

        /// <summary>
        /// Writes the formatted draw probability text.
        /// </summary>
        /// <param name="c">The <see cref="Card" />.</param>
        /// <param name="minionCount">The minion count.</param>
        /// <returns>The formatted draw probability text</returns>
        internal string WriteDrawProbability(int copyCount, int minionCount, int drawCount)
        {
            return $"({copyCount}): {DrawProbability(minionCount, copyCount, drawCount)}%";
        }

        /// <summary>
        /// Calculates the Draw the probability.
        /// </summary>
        /// <param name="poolsize">The pool size you will draw from.</param>
        /// <param name="copies">The number of copies of a card.</param>
        /// <param name="draw">The number of cards to draw.</param>
        /// <param name="dec">The decimal place to round to.</param>
        /// <returns>The Draw probability.</returns>
        public Double DrawProbability(int poolsize, int copies = 1, int draw = 1, int dec = 0)
        {
            return Math.Round(
                Helper.DrawProbability(copies, poolsize, draw) * 100, dec);
        }

        /// <summary>
        /// Loads the cards, sorts and filters as needed.
        /// </summary>
        public void LoadCards()
        {
            this.QueryDeck = BuildQueryDeck();
            this.CardList.Update(this.QueryDeck, true);
        }

        /// <summary>
        /// Resets this instance's <see cref="Card" /> lists.
        /// </summary>
        public void Reset()
        {
            CardList.Update(null, true);
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}