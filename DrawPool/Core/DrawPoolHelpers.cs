namespace DrawPool
{
    using System;
    using System.Collections.Generic;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// Available Card(s) Helper(s)
    /// </summary>
    public static class DrawPoolHelpers
    {



        /// <summary>
        /// Fixes the card's "Is Created" indicator and merges them into one big pile.
        /// </summary>
        /// <param name="cards">The list of drawable <see cref="Card">Cards</see>.</param>
        /// <returns></returns>
        public static List<Card> FixCreatedCards(this List<Card> cards)
        {
            cards.ForEach(c => {
                if (c.IsCreated)
                    c.IsCreated = false;
            });

            return cards;
        }
    }
}
