using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Controls;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DrawPool.DrawViews
{
    public class PluginManager
    {
        public PluginManager()
        {
            GameEvents.OnGameStart.Add(GameTypeCheck);
            GameEvents.OnGameEnd.Add(CleanUp);
        }

        private void GameTypeCheck()
        {
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

        private void CleanUp()
        {
        }
    }
}