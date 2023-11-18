namespace DrawPool
{
    using DrawLogic;
    using DrawPool.Controls;
    using DrawPool.Logic;
    using Hearthstone_Deck_Tracker.Controls;
    using Properties;
    using System;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// An Active Instance of the plugin
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DrawPoolInstance : IDisposable
    {
        public static ControlMovementManager controlMovementManager;
        public MinstrelPool minstrelPool;

        public DrawPoolInstance()
        {
            initPools();
        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            minstrelPool.RenderTransform = new ScaleTransform(Settings.Default.DrawPoolScale / 100, Settings.Default.DrawPoolScale / 100);
            Canvas.SetTop(minstrelPool, Settings.Default.DrawPoolTop);
            Canvas.SetLeft(minstrelPool, Settings.Default.DrawPoolLeft);
        }

        /// <summary>
        /// Removes the window from the canvas overlay.
        /// </summary>
        public void CleanPools()
        {
            if (Settings.Default.IsMinstrelEnabled)
            {
                Core.OverlayCanvas.Children.Remove(minstrelPool);
            }
        }

        public void Dispose()
        {
            CleanPools();

            //Input.Dispose();
        }

        public void initPools()
        {
            controlMovementManager = new ControlMovementManager(minstrelPool, SettingsView.IsUnlocked);
            if (Settings.Default.IsMinstrelEnabled)
            {
                minstrelPool = new MinstrelPool();
                minstrelPool.Name = "MinstrelPoolView";
                minstrelPool.Visibility = System.Windows.Visibility.Collapsed;
                //Border b = (Border)minstrelPool.FindName("OuterBorder");
                //GameEvents.OnMouseOverOff.Add(minstrelPool.OnMouseOff);
                Core.OverlayCanvas.Children.Add(minstrelPool);
                Settings.Default.PropertyChanged += SettingsChanged;
            }

           
        }

        public void OnUnload()
        {
            CleanPools();
        }

        public void Reset()
        {
            CleanPools();
        }
    }
}