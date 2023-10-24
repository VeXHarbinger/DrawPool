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
        /// Returns the list of Minions Grouped by their Counts, for statistical purposes.
        /// </summary>
        /// <returns>The list of Minions Grouped by their Counts, for statistical purposes</returns>
        internal List<IGrouping<int, Card>> GroupedMinion() => QueryDeck.GroupBy(c => c.Count).OrderByDescending(grp => grp.Count()).OrderBy(g => g.Key).ToList();

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
        /// When the Player mouses over a <see cref="Card" /> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card" />.</param>
        public void PlayerHandMouseOver(Card card)
        {
            if (!card.Id.Contains(CardId()))
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