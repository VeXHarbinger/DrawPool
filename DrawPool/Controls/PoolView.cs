namespace DrawPool.Controls
{
    using DrawPool.Properties;
    using Hearthstone_Deck_Tracker;
    using Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    public partial class PoolView : UserControl
    {
        private User32.MouseInput _mouseInput;

        /// <summary>
        /// The deck hash reference
        /// </summary>
        private int deckHash;

        /// <summary>
        /// Indicates if the window is movable or pined
        /// </summary>
        private bool isDraggable = false;

        private bool panelSelected = false;

        internal Point mousePosition;

        /// <summary>
        /// The list of <see cref="Card">Cards</see>
        /// </summary>
        public List<Card> Cards;

        /// <summary>
        /// The Hearthstone Text Block With the view's styled Title within
        /// </summary>
        public HearthstoneTextBlock winTitleLabel;

        private double ScreenRatio => (4.0 / 3.0) / (Core.OverlayCanvas.Width / Core.OverlayCanvas.Height);

        /// <summary>
        /// Gets the deck Hash Code.
        /// </summary>
        /// <value>The deck HashCode.</value>
        public int DeckHash => Core.Game.Player.Deck.GetHashCode();

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(Parent as Window);
            this.CaptureMouse();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                this.ReleaseMouseCapture();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Vector diff = e.GetPosition(Parent as Window) - mousePosition;
            if (this.IsMouseCaptured)
            {
                (RenderTransform as TranslateTransform).X = diff.X;
                (RenderTransform as TranslateTransform).Y = diff.Y;
            }
        }

        private void MouseInputOnLmbDown(object sender, MouseButtonEventArgs e)
        {
            var _mousePos = User32.GetMousePos();
            var p = new Point(_mousePos.X, _mousePos.Y);
            if (IsPoolWindowDraggable())
            {
                panelSelected = true;
            }
            else
            {
                panelSelected = false;
            }
        }

        private void MouseInputOnLmbUp(object sender, MouseButtonEventArgs e)
        {
            var _mousePos = User32.GetMousePos();
            var p = Core.OverlayCanvas.PointFromScreen(new Point(_mousePos.X, _mousePos.Y));
            if (IsPoolWindowDraggable())
            {
                Settings.Default.DrawPoolTop = p.Y;
                Settings.Default.DrawPoolLeft = p.X;
            }
            panelSelected = false;
        }

        /// <summary>
        /// Returns the list of Minions Grouped by their Counts, for statistical purposes.
        /// </summary>
        /// <returns>The list of Minions Grouped by their Counts, for statistical purposes</returns>
        protected List<IGrouping<int, Card>> GroupedMinion() => Cards.GroupBy(c => c.Count).OrderByDescending(grp => grp.Count()).OrderBy(g => g.Key).ToList();

        /// <summary>
        /// Gets or sets the minion count.
        /// </summary>
        /// <value>The minion count.</value>
        protected int MinionCount() => Cards.Sum(c => c.Count);

        /// <summary>
        /// Writes the deck mix of minions in cards.
        /// </summary>
        /// <param name="possibleDraw">The count of possible cards that could be draw.</param>
        /// <param name="deckCount">The count of cards in the deck.</param>
        /// <returns>The deck mix of minions in cards as Minion Count/Deck Count</returns>
        protected string WriteDeckMix(int possibleDraw, int deckCount)
        {
            return $"{possibleDraw}";
        }

        /// <summary>
        /// Writes the formatted draw probability text.
        /// </summary>
        /// <param name="c">The <see cref="Card" />.</param>
        /// <param name="minionCount">The minion count.</param>
        /// <returns>The formatted draw probability text</returns>
        protected string WriteDrawProbability(int copyCount, int minionCount, int drawCount)
        {
            return $"{DrawProbability(minionCount, copyCount, drawCount)}%";
        }

        /// <summary>
        /// Checks if the deck changed since the last time we displayed the view.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the Deck has Changed;otherwise, <c>false</c>
        /// </returns>
        public bool CheckDeckChanged() => (deckHash != Core.Game.Player.Deck.GetHashCode());

        /// <summary>
        /// Calculates the Draw the probability.
        /// </summary>
        /// <param name="poolsize">The pool size you will draw from.</param>
        /// <param name="copies">The number of copies of a card.</param>
        /// <param name="draw">The number of cards to draw.</param>
        /// <param name="dec">The decimal place to round to.</param>
        /// <returns>The Draw probability.</returns>
        public Double DrawProbability(int poolsize, int copies = 1, int draw = 1, int dec = 0)
        {
            return Math.Round(
                Helper.DrawProbability(copies, poolsize, draw) * 100, dec);
        }

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
        public bool IsPoolWindowDraggable() => isDraggable;

        /// <summary>
        /// Called when the mouse focus moves off the card.
        /// </summary>
        public void OnMouseOff()
        {
            Hide();
        }

        public void Pin()
        {
            isDraggable = false;
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
            isDraggable = true;
            Show();

            //this.MouseLeftButtonDown += MouseInputOnLmbDown;
            //this.MouseLeftButtonUp += MouseInputOnLmbUp;
            //_mouseInput = new User32.MouseInput();
            //_mouseInput.LmbDown += MouseInputOnLmbDown;
            //_mouseInput.LmbUp += MouseInputOnLmbUp;
            //_mouseInput.MouseMoved += MouseInputOnMouseMoved;
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