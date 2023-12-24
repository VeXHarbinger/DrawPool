using DrawPool.DrawLogic;
using DrawPool.Logic;
using DrawPool.Properties;
using Hearthstone_Deck_Tracker.API;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DrawPool
{
    public class DrawPool : IDisposable
    {
        public static InputMoveManager inputMoveManager;
        public MinstrelPool minstrelPool;

        public DrawPool()
        {
            minstrelPool = new MinstrelPool();
            minstrelPool.Name = "MinstrelPoolView";
            minstrelPool.Visibility = System.Windows.Visibility.Collapsed;
            Core.OverlayCanvas.Children.Add(minstrelPool);

            Settings.Default.DrawPoolTop = CheckDefault(Settings.Default.DrawPoolTop);
            Settings.Default.DrawPoolLeft = CheckDefault(Settings.Default.DrawPoolLeft);
            Canvas.SetTop(minstrelPool, Settings.Default.DrawPoolTop);
            Canvas.SetLeft(minstrelPool, Settings.Default.DrawPoolLeft);

            inputMoveManager = new InputMoveManager(minstrelPool);

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);

            GameEvents.OnGameStart.Add(GameTypeCheck);
            GameEvents.OnGameEnd.Add(CleanUp);
        }

        private double CheckDefault(double n) {
            return n > 0 ? n : n * -1;
        }


        private void GameTypeCheck()
        {
            // ToDo : Enable toggle Props
            if (Core.Game.CurrentGameType == HearthDb.Enums.GameType.GT_RANKED ||
                Core.Game.CurrentGameType == HearthDb.Enums.GameType.GT_CASUAL ||
                Core.Game.CurrentGameType == HearthDb.Enums.GameType.GT_ARENA)
            {
                InitLogic();
            }
        }

        private void InitLogic()
        {
        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {            
            minstrelPool.RenderTransform = new ScaleTransform(Settings.Default.DrawPoolScale / 100, Settings.Default.DrawPoolScale / 100);
            minstrelPool.Opacity = Settings.Default.DrawPoolOpacity / 100;
        }

        public void CleanUp()
        {
        }

        public void Dispose()
        {
            Core.OverlayCanvas.Children.Remove(minstrelPool);
            inputMoveManager.Dispose();
        }
    }
}