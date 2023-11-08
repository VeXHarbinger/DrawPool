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

        public AnimatedCardList AnimatedCardLister() => this.AnimatedCards;

        public void DoMath()
        {
            // throw new NotImplementedException();
        }

        public bool HasTriggerCard() => Core.Game.Player.PlayerCardList.FindIndex(c => c.Id.Contains(HearthDb.CardIds.Collectible.Rogue.ElvenMinstrel)) > -1;

        /// <summary>
        /// Loads the cards from <see cref="PlayerCardList"/> to <see cref="AnimatedCardList"/>, then sorts and filters as needed.
        /// </summary>
        public void LoadCards()
        {
            Cards = Core.Game.Player.PlayerCardList
            .Where(c =>
                c.Type == "Minion" &&
                (c.Count - c.InHandCount) > 0
            )
            .OrderBy(c => c.Cost)
            .ThenBy(c => c.Count)
            .ThenBy(c => c.Name)
            .ToList<Card>();
            //
            AnimatedCardLister().Update(Cards, true);
        }

        public void PoolRules()
        {
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
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

        public void Reset()
        {
            List<Card> Cards = new List<Card>();
            AnimatedCardLister().Update(null, true);
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