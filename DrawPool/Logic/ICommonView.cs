namespace DrawPool.DrawLogic
{
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

    public interface ICommonView
    {
        /// <summary>
        /// Does the math.
        /// </summary>
        void DoMath();

        /// <summary>
        /// Determines whether the player the required trigger card or not.
        /// </summary>
        /// <returns>
        ///   <c>true</c> If the player the required trigger card; otherwise, <c>false</c>.
        /// </returns>
        bool HasTriggerCard();

        /// <summary>
        /// Loads the cards, sorts and filters as needed.
        /// </summary>
        void LoadCards();

        void PoolRules();

        /// <summary>
        /// When the Player mouses over a <see cref="Card" /> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card" />.</param>
        void PlayerHandMouseOver(Card card);

        /// <summary>
        ///  The name og the pool view.
        /// </summary>
        /// <returns></returns>
        string PoolName();

        /// <summary>
        /// Resets this instance's <see cref="Card" /> lists.
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets the unique card identifier.
        /// </summary>
        /// <value>The card identifier.</value>
        string TriggerCardId();
    }
}