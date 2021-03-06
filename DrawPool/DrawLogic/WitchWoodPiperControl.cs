﻿namespace DrawPool
{
    using DrawPool.Models;
    using Hearthstone_Deck_Tracker.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

    /// <summary>
    /// WitchWood Piper Display Control
    /// </summary>
    /// <seealso cref="DrawPool.DisplayControl" />
    /// <seealso cref="DrawPool.IDraw" />
    internal class WitchWoodPiperControl : DisplayControl, IDraw
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WitchWoodPiperControl" /> class.
        /// </summary>
        public WitchWoodPiperControl()
        {
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
        }

        /// <summary>
        /// The <see cref="PlayerCardList">Deck</see> object reference for the <see cref="Card">Cards</see> data.
        /// </summary>
        /// <value><returns>The list of current <see cref="Card">Cards</see></returns></value>
        public List<Card> QueryDeck { get; set; }

        /// <summary>
        /// Queries the <see cref="PlayerCardList">Deck</see> for specific scoped <see cref="Card">Cards</see>.
        /// </summary>
        /// <returns>The scoped list of <see cref="Card">Cards</see></returns>
        internal List<Card> BuildQueryDeck()
        {

            var playerDeck = Hearthstone_Deck_Tracker.API.Core.Game.Player
                .PlayerCardList
                .Where(c =>
                    c.Type == "Minion" &&
                    c.Count > 0
                )
                .ToList<Card>()
                .FixCreatedCards()
                .OrderBy(c => c.Cost)
                .ThenBy(c => c.Count)
                .ThenBy(c => c.Name)
                .GroupBy(c => c.Cost)
                .First()
                .ToList<Card>()
                .FixDuplicateCards()
                ;

            //c.EqualsWithCount  c.Mechanics

            return playerDeck;
        }

        /// <summary>
        /// Gets or sets the minion count.
        /// </summary>
        /// <value>The minion count.</value>
        internal int MinionCount() => QueryDeck.Sum(c => c.Count);

        /// <summary>
        /// Gets the unique card identifier.
        /// </summary>
        /// <value>The card identifier.</value>
        public string CardId() => "GIL_584";

        /// <summary>
        /// Does the math.
        /// </summary>
        public void DoMath()
        {
            lblProbability.Content = "";
            lblDeckMix.Content = WriteDeckMix(MinionCount(), Hearthstone_Deck_Tracker.API.Core.Game.Player.DeckCount);
            if (QueryDeck.Count >= 1 || QueryDeck.Count == MinionCount())
            {
                lblProbability.Content = WriteDrawProbability(QueryDeck[0].Count, MinionCount(), 1);
            }
            if (QueryDeck.Count >= 2)
            {
                if (QueryDeck[1].Count != QueryDeck[0].Count)
                {
                    lblProbability.Content += " ";
                    lblProbability.Content += WriteDrawProbability(QueryDeck[1].Count, MinionCount(), 1);
                }

                if (QueryDeck.Count >= 3)
                {
                    lblProbability.Content += " ";
                    lblProbability.Content += WriteDrawProbability(QueryDeck.Last().Count, MinionCount(), 1);
                }
            }
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
                    // ToDo: Should do math be moved into the load cards process?
                    DoMath();
                }
                ShowDisplay(new CurtainCall { CallingView = ViewModes.WitchWoodPiper, ShouldShow = true }, new EventArgs());
            }
        }
    }
}