namespace DrawPool
{
    using System;
    using System.Windows;
    using Hearthstone_Deck_Tracker.Controls;
    using System.Collections.Generic;
    using System.Linq;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using DrawPool.Models;
    using Hearthstone_Deck_Tracker.API;
    using System.Windows.Controls;

    /// <summary>
    /// WitchWood Piper Display Control
    /// </summary>
    /// <seealso cref="DrawPool.DisplayControl"/>
    /// <seealso cref="DrawPool.IDraw"/>
    internal class WitchWoodPiperControl : DisplayControl, IDraw
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WitchWoodPiperControl"/> class.
        /// </summary>
        public WitchWoodPiperControl()
        {
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
        }

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
            lblDeckMix.Content = $"{countMinions()}/{Hearthstone_Deck_Tracker.API.Core.Game.Player.DeckCount}";

            if (CardList.Items.Count >= 1)
            {
                Card c1 = ((AnimatedCard)CardList.Items[0]).Card;
                lblProbability.Content = $"({c1.Count}): 100%";
                if (CardList.Items.Count >= 2)
                {
                    int mc = countMinions();
                    Card c2 = ((AnimatedCard)CardList.Items[1]).Card;
                    lblProbability.Content = $"({c1.Count}): {DrawProbability(mc, c1.Count, 1)}%" +
                        $"  /  " +
                        $"({2}): " +
                        $"{DrawProbability(mc, 2, 1)}%";
                }
            }
            else
            {
                lblProbability.Content = "";
            }
        }

        /// <summary>
        /// Loads the cards, sorts and filters as needed.
        /// </summary>
        public void LoadCards()
        {
            this.CardList.Update(QueryDeck(), true);
        }

        /// <summary>
        /// When the Player mouses over a <see cref="Card"/> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card"/>.</param>
        public void PlayerHandMouseOver(Card card)
        {
            if (card.Id == CardId())
            {
                LoadCards();
                DoMath();
                ShowDisplay(new CurtainCall { CallingView = ViewModes.WitchWoodPiper, ShouldShow = true }, new EventArgs());
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
            return Core.Game.Player.PlayerCardList
                               .Where(c => c.Type == "Minion" && (c.Count - c.InHandCount) > 0)
                               .OrderBy(c => c.Cost)
                               .ThenBy(c => c.Count)
                               .ThenBy(c => c.Name)
                               .GroupBy(c => c.Cost)
                               .First()
                               .ToList<Card>();
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