namespace DrawPool.DrawLogic
{
    using Controls;
    using Hearthstone_Deck_Tracker.API;
    using Hearthstone_Deck_Tracker.Controls;
    using Logic;
    using System.Collections.Generic;
    using System.Linq;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// The Minstrel DrawPool View
    /// </summary>
    /// <seealso cref="DrawPool.Controls.PoolView" />
    /// <seealso cref="DrawPool.DrawLogic.ICommonView" />
    public class MinstrelPool : PoolView, ICommonView
    {
        public MinstrelPool()
        {
            SetTitle();
            LoadCards();
            PoolRules();
        }

        /// <summary>
        /// Does the math.
        /// </summary>
        public void DoMath()
        {
            this.Chance1.Text = "0%";
            this.Chance2.Text = "0%";
            if (MinionCount() >= 1)
            {
                var gm = GroupedMinion();
                // Next, figure out our odds
                this.Chance1.Text = WriteDrawProbability(gm.First<IGrouping<int, Card>>().First<Card>().Count, MinionCount(), 2);
                if (gm.Count >= 2)
                {
                    this.Chance2.Text = WriteDrawProbability(gm[1].First<Card>().Count, MinionCount(), 2);
                }
            }
        }

        /// <summary>
        /// Determines whether the player jas the required trigger card or not.  In this case the Elven Minstrel
        /// </summary>
        /// <returns>
        ///   <c>true</c> If the player the required trigger card; otherwise, <c>false</c>.
        /// </returns>
        public bool HasTriggerCard() => Core.Game.Player.PlayerCardList.FindIndex(c => c.Id.Contains(HearthDb.CardIds.Collectible.Rogue.ElvenMinstrel)) > -1;

        /// <summary>
        /// Loads the cards from <see cref="PlayerCardList"/> to <see cref="AnimatedCardList"/>, then sorts and filters as needed.
        /// </summary>
        public void LoadCards()
        {
            Cards = DrawPoolHelpers.BuildQueryDeck();
            DoMath();
            AnimatedCardLister.Update(Cards, true);
        }

        /// <summary>
        /// When the Player mouses over a <see cref="Card" /> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card" />.</param>
        public void PlayerHandMouseOver(Card card)
        {
            if (!card.Id.Contains(TriggerCardId()))
            {
                Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (CheckDeckChanged())
                {
                    LoadCards();
                }
                Visibility = System.Windows.Visibility.Visible;
            }
        }

        public string PoolName() => "MinstrelPoolView";

        public void PoolRules()
        {
            GameEvents.OnGameStart.Add(Reset);
            GameEvents.OnGameEnd.Add(Reset);
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
        }

        public void Reset()
        {
            List<Card> Cards = new List<Card>();
            AnimatedCardLister.Update(null, true);
            this.Chance1.Text = "0%";
            this.Chance2.Text = "0%";
            Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Gets the unique card identifier.
        /// </summary>
        /// <value>The card identifier.</value>
        public string TriggerCardId() => "LOOT_211";

        public new bool Update(Card card)
        {
            return card.Type == "Minion" &&
                card.Count > card.InHandCount &&
                base.Update(card);
        }
    }
}