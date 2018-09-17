namespace DrawPool
{
    using System.Collections.Generic;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

    /// <summary>
    /// Interface for a card that invokes a draw, as opposed to a mechanic
    /// </summary>
    public interface IDraw
    {
        /// <summary>
        /// The <see cref="PlayerCardList">Deck</see> object reference for the <see cref="Card">Cards</see> data.
        /// </summary>
        /// <value><returns>The list of current <see cref="Card">Cards</see></returns></value>
        List<Card> QueryDeck { get; set; }

        /// <summary>
        /// Gets the unique card identifier.
        /// </summary>
        /// <value>The card identifier.</value>
        string CardId();

        /// <summary>
        /// Does the math.
        /// </summary>
        void DoMath();

        /// <summary>
        /// Loads the cards, sorts and filters as needed.
        /// </summary>
        void LoadCards();

        /// <summary>
        /// When the Player mouses over a <see cref="Card" /> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card" />.</param>
        void PlayerHandMouseOver(Card card);

        /// <summary>
        /// Resets this instance's <see cref="Card" /> lists.
        /// </summary>
        void Reset();
    }
}