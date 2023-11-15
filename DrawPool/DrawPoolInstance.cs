namespace DrawPool
{
    using DrawLogic;
    using Hearthstone_Deck_Tracker.API;
    using Properties;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    public class DrawPoolInstance: IDisposable
    {
        public MinstrelPool minstrelPool;

        public DrawPoolInstance()
        {
            initPools();
        }

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
            if (Settings.Default.IsMinstrelEnabled)
            {
                minstrelPool = new MinstrelPool();
                minstrelPool.Name = "MinstrelPoolView";
                minstrelPool.Visibility = System.Windows.Visibility.Collapsed;

                //minstrelPool.MouseLeftButtonDown += minstrelPool.Grid_MouseLeftButtonDown_UserControl;
                // ((Border)minstrelPool.FindName("InnerBorder")).MouseLeftButtonDown += minstrelPool.Grid_MouseLeftButtonDown_InnerBorder;

                Border b = (Border)minstrelPool.FindName("OuterBorder");

               // b.MouseLeftButtonDown += Grid_MouseLeftButtonDown_UserControl;
                //((Grid)minstrelPool.FindName("InnerGrid")).MouseLeftButtonDown += minstrelPool.Grid_MouseLeftButtonDown;

                GameEvents.OnMouseOverOff.Add(minstrelPool.OnMouseOff);
                Core.OverlayCanvas.Children.Add(minstrelPool);
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