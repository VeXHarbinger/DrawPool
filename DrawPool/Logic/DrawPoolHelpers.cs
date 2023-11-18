namespace DrawPool.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

    /// <summary>
    /// Available Card(s) Helper(s)
    /// </summary>
    public static class DrawPoolHelpers
    {
        /// <summary>
        /// Queries the <see cref="PlayerCardList">Deck</see> for specific scoped <see cref="Card">Cards</see>.
        /// </summary>
        /// <returns>The scoped list of <see cref="Card">Cards</see></returns>
        public static List<Card> BuildQueryDeck()
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
        /// Fixes the card's "Is Created" indicator and merges them into one big pile.
        /// </summary>
        /// <param name="cards">The list of drawable <see cref="Card">Cards</see>.</param>
        /// <returns>The list of drawable <see cref="Card">Cards</see></returns>
        public static List<Card> FixCreatedCards(this List<Card> cards)
        {
            cards.ForEach(c =>
            {
                if (c.IsCreated)
                    c.IsCreated = false;
            });

            return cards;
        }

        /// <summary>
        /// Fixes the duplicate cards and merges them into one big pile.
        /// </summary>
        /// <param name="cards">The list of drawable <see cref="Card">Cards</see>.</param>
        /// <returns>The list of drawable <see cref="Card">Cards</see></returns>
        public static List<Card> FixDuplicateCards(this List<Card> cards)
        {
            var dups = cards
                .GroupBy(c => c.Id)
                .Where(d => d.Count() > 1)
                .ToList();

            if (dups.Count >= 1)
            {
                foreach (var d in dups.ToList())
                {
                    var count = 0;
                    Card first = d.First();
                    foreach (var i in d)
                    {
                        count += i.Count;
                        i.Count = 0;
                    }
                    first.Count = count;
                }
            }
            cards.RemoveAll(c => c.Count == 0);
            return cards;
        }
    }
}