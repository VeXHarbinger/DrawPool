namespace DrawPool
{
    using System;
    using System.Collections.Generic;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// Interface for a card that invokes a draw, as opposed to a mechanic
    /// </summary>
    public interface IDraw
    {
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
        /// When the Player mouses over a <see cref="Card"/> in his hand.
        /// </summary>
        /// <param name="card">The <see cref="Card"/>.</param>
        void PlayerHandMouseOver(Card card);

        /// <summary>
        /// Queries the <see cref="PlayerCardList">Deck</see> for specific scoped <see cref="Card">Cards</see>.
        /// </summary>
        /// <returns>The scoped list of <see cref="Card">Cards</see></returns>
        List<Card> QueryDeck();

        /// <summary>
        /// Resets this instance's <see cref="Card"/> lists.
        /// </summary>
        void Reset();
    }
}