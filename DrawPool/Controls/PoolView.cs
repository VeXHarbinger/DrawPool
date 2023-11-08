namespace DrawPool.Controls
{
    using Hearthstone_Deck_Tracker;
    using Logic;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    public partial class PoolView : UserControl
    {
        /// <summary>
        /// The deck hash reference
        /// </summary>
        private int deckHash;

        /// <summary>
        /// Indicates if the window is movable or pined
        /// </summary>
        private bool isDraggable = false;

        /// <summary>
        /// The list of <see cref="Card">Cards</see>
        /// </summary>
        public List<Card> Cards;

        /// <summary>
        /// The Hearthstone Text Block With the view's styled Title within
        /// </summary>
        public HearthstoneTextBlock Label;

        private double ScreenRatio => (4.0 / 3.0) / (Core.OverlayCanvas.Width / Core.OverlayCanvas.Height);

        /// <summary>
        /// Gets the deck Hash Code.
        /// </summary>
        /// <value>The deck HashCode.</value>
        public int DeckHash => Core.Game.Player.Deck.GetHashCode();

        /// <summary>
        /// Checks if the deck changed since the last time we displayed the view.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the Deck has Changed;otherwise, <c>false</c>
        /// </returns>
        public bool CheckDeckChanged() => (deckHash != Core.Game.Player.Deck.GetHashCode());

        /// <summary>
        /// Collapsed the view.
        /// </summary>
        public void Hide()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window instance is draggable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if window instance is draggable; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindowDraggable() => isDraggable;

        /// <summary>
        /// Called when the mouse focus moves off the card.
        /// </summary>
        public void OnMouseOff()
        {
            Hide();
        }

        public void Pin()
        {
        }

        /// <summary>
        /// Indicates if the Pool is visibile.
        /// </summary>
        /// <returns>
        /// <c>true</c> if it is Visible;otherwise, <c>false</c>.
        /// </returns>
        public bool PoolIsVisibile() => (base.Visibility == System.Windows.Visibility.Visible);

        public void SetTitle()
        {
            HearthstoneTextBlock htb = (HearthstoneTextBlock)this.FindName("BlockTitleText");
            htb.Text = Strings.GetLocalized("MinstrelLabel") + " " + Strings.GetLocalized("PluginName");
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void Show()
        {
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public void UnPin()
        {
        }

        public bool Update(Card card)
        {
            if (card.Type != "Minion")
            {
                return false;
            }

            var match = Cards.FirstOrDefault(c => c.Name == card.Name);

            if (match != null)
            {
                match.Count++;
            }
            else
            {
                Cards.Add(card.Clone() as Card);
            }

            //Label.Visibility = Visibility.Visible;

            return true;
        }

        public void UpdatePosition()
        {
            /* OLD CODE; positions near top of screen
            Canvas.SetTop(this, Core.OverlayCanvas.Height * 3 / 100);
            var xPos = Hearthstone_Deck_Tracker.Helper.GetScaledXPos(8.0 / 100, (int)Core.OverlayCanvas.Width, ScreenRatio);

            if (isLocal)
            {
                Canvas.SetRight(this, xPos);
            }
            else
            {
                Canvas.SetLeft(this, xPos);
            }
            */

            // Canvas.SetRight(this, Hearthstone_Deck_Tracker.Helper.GetScaledXPos(5.0 / 100, (int)Core.OverlayCanvas.Width, ScreenRatio));
            if (true)
            {
                //   Canvas.SetTop(this, Core.OverlayCanvas.Height * 65 / 100);
            }
            else
            {
                //  Canvas.SetBottom(this, Core.OverlayCanvas.Height * 75 / 100);
            }
        }
    }
}