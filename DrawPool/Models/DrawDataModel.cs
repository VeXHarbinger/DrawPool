namespace DrawPool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

    /// <summary>
    /// Draw Data Model
    /// </summary>
    public class DrawDataModel
    {
        /// <summary>
        /// Gets or sets the <see cref="Card"/> list.
        /// </summary>
        /// <value>The <see cref="Card"/> list.</value>
        public List<Card> CardList { get; set; } = new List<Card>();

        /// <summary>
        /// Gets or sets the deck information related to related minions.
        /// </summary>
        /// <value>The deck information related to related minions.</value>
        public string DeckData { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the statistical data on draw odds.
        /// </summary>
        /// <value>The statistical data on draw odds.</value>
        public string StatisticalData { get; set; } = string.Empty;
    }
}