namespace DrawPool
{
    using System;
    using System.Collections.Generic;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    public static class DrawPoolHelpers
    {
        /// <summary>
        /// Fixes the card's "Is Created" indicator and merges them into one big pile.
        /// </summary>
        /// <param name="cards">The list of cards cards.</param>
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
