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
    using Settings = DrawPool.Properties.Settings;
    using Hearthstone_Deck_Tracker.API;
    using DrawPool.Models;

    /// <summary>
    /// Elven Minstrel <see cref="DisplayControl">Display Control</see>
    /// </summary>
    /// <seealso cref="DrawPool.DisplayControl"/>
    /// <seealso cref="DrawPool.IDraw"/>
    internal class ElvenMinstrelControl : DisplayControl, IDraw
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElvenMinstrelControl"/> class.
        /// </summary>
        public ElvenMinstrelControl()
        {
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
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
            int mc = countMinions();
            // First, figure out our remaining card mix
            lblDeckMix.Content = $"{CardList.Items.Count}/{Hearthstone_Deck_Tracker.API.Core.Game.Player.DeckCount}";
            // Next, figure out our odds
            lblProbability.Content = $"(1): {DrawProbability(mc, 1, 1)}% / (2): {DrawProbability(mc, 1, 2)}%";
            // Finally see if we have any large card counts
            var match = CardList.Items.Cast<AnimatedCard>()
                .OrderByDescending(c => c.Card.Count)
                .FirstOrDefault()
                .Card;

            if (match.Count > 2)
            {
                lblProbability.Content += $" / ({match.Count}): {DrawProbability(mc, match.Count, 2)}%";
            }
        }

        /// <summary>
        /// Loads the list of <see cref="Card">Cards</see>, by combining copies
        /// </summary>
        /// <param name="cards">The List of <see cref="Card">Cards</see>.</param>
        public void LoadCards()
        {
            CardList.Update(QueryDeck(), true);
        }

        /// <summary>
        /// When the Player mouses over a <see cref="Card"/> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card"/>.</param>
        public void PlayerHandMouseOver(Card card)
        {
            if (card.Id == CardId())
            {
                this.Visibility = Visibility.Visible;
                LoadCards();
                DoMath();
                ShowDisplay(new CurtainCall { CallingView = ViewModes.ElvenMinstrel, ShouldShow = true }, new EventArgs());
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Queries the <see cref="PlayerCardList">Deck</see> for specific scoped <see cref="Card">Cards</see>.
        /// </summary>
        /// <returns>The scoped list of <see cref="Card">Cards</see></returns>
        public List<Card> QueryDeck()
        {
            var playerDeck = Hearthstone_Deck_Tracker.API.Core.Game.Player
                .PlayerCardList
                .Where(c =>
                    c.Type == "Minion" &&
                    (c.Count - c.InHandCount) > 0
                )
                .ToList();

            var dups = playerDeck
                .GroupBy(c => c.Id)
                .Where(d => d.Count() > 1)
                .ToList();

            if (dups.Count() >= 1)
            {
                foreach (var d in dups.ToList())
                {
                    var count = 0;
                    Card first = d.First();
                    foreach (var i in d)
                    {
                        i.IsCreated = false;
                        count += i.Count;
                        i.Count = 0;
                    }
                    first.Count = count;
                }
            }
            playerDeck.RemoveAll(c => c.Count == 0);
            return playerDeck.ToList<Card>();
        }

        /// <summary>
        /// Counts the minions available in the draw pool.
        /// </summary>
        /// <returns></returns>
        internal int countMinions()
        {
            return QueryDeck().Sum(c => c.Count);
        }
    }
}