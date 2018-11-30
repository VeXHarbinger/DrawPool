namespace DrawPool
{
    using DrawPool.Models;
    using Hearthstone_Deck_Tracker.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

    /// <summary>
    /// Elven Minstrel <see cref="DisplayControl">Display Control</see>
    /// </summary>
    /// <seealso cref="DrawPool.DisplayControl" />
    /// <seealso cref="DrawPool.IDraw" />
    internal class ElvenMinstrelControl : DisplayControl, IDraw
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElvenMinstrelControl" /> class.
        /// </summary>
        public ElvenMinstrelControl()
        {
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
        }

        /// <summary>
        /// The <see cref="PlayerCardList">Deck</see> object reference for the <see cref="Card">Cards</see> data.
        /// </summary>
        /// <value><returns>The list of current <see cref="Card">Cards</see></returns></value>
        public List<Card> QueryDeck { get; set; }

        /// <summary>
        /// Returns the list of Minions Grouped by their Counts, for statistical purposes.
        /// </summary>
        /// <returns>The list of Minions Grouped by their Counts, for statistical purposes</returns>
        internal List<IGrouping<int, Card>> GroupedMinion() => QueryDeck.GroupBy(c => c.Count).OrderByDescending(grp => grp.Count()).OrderBy(g => g.Key).ToList();

        /// <summary>
        /// Gets or sets the minion count.
        /// </summary>
        /// <value>The minion count.</value>
        internal int MinionCount() => this.QueryDeck.Sum(c => c.Count);

        /// <summary>
        /// Queries the <see cref="PlayerCardList">Deck</see> for specific scoped <see cref="Card">Cards</see>.
        /// </summary>
        /// <returns>The scoped list of <see cref="Card">Cards</see></returns>
        public List<Card> BuildQueryDeck()
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
                .FixCreatedCards();

            var dups = playerDeck
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
            playerDeck.RemoveAll(c => c.Count == 0);
            return playerDeck;
        }

        /// <summary>
        /// Gets the unique card identifier.
        /// </summary>
        /// <value>The card identifier.</value>
        public string CardId() => "LOOT_211";

        /// <summary>
        /// Does the math.
        /// </summary>
        public void DoMath()
        {
            // First, figure out our remaining card mix
            lblDeckMix.Content = WriteDeckMix(MinionCount(), Hearthstone_Deck_Tracker.API.Core.Game.Player.DeckCount);
            if (MinionCount() >= 1)
            {
                lblProbability.Content = "";
                var gm = GroupedMinion();
                // Next, figure out our odds
                lblProbability.Content = WriteDrawProbability(gm.First<IGrouping<int, Card>>().First<Card>().Count, MinionCount(), 2);
                if (gm.Count >= 2)
                {
                    lblProbability.Content += " ";
                    lblProbability.Content += WriteDrawProbability(gm[1].First<Card>().Count, MinionCount(), 2);
                    if (gm.Count >= 3)
                    {
                        lblProbability.Content += " ";
                        lblProbability.Content += WriteDrawProbability(gm.Last<IGrouping<int, Card>>().First<Card>().Count, MinionCount(), 2);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the list of <see cref="Card">Cards</see>, by combining copies
        /// </summary>
        /// <param name="cards">The List of <see cref="Card">Cards</see>.</param>
        public void LoadCards()
        {
            this.QueryDeck = BuildQueryDeck();
            this.CardList.Update(QueryDeck, true);
        }

        /// <summary>
        /// When the Player mouses over a <see cref="Card" /> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card" />.</param>
        public void PlayerHandMouseOver(Card card)
        {
            if (card.Id != CardId())
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (CheckDeckChanged())
                {
                    LoadCards();
                    DoMath();
                }
                ShowDisplay(new CurtainCall { CallingView = ViewModes.ElvenMinstrel, ShouldShow = true }, new EventArgs());
            }
        }
    }
}