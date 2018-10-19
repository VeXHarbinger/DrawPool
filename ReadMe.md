# Drawpool

## An HDT Plugin, to help clear-up your minion draw pool possibilities.

This is a plug-in for the [Hearthstone Deck Tracker](https://github.com/HearthSim/Hearthstone-Deck-Tracker).
Currently, it shows you the possible cards that could be drawn from your hand with the [Elven Minstrel](https://hearthstone.gamepedia.com/Elven_Minstrel) or the [Witchwood Piper](https://hearthstone.gamepedia.com/Witchwood_Piper).

I hope to add in the mechanics for [Recruit](https://hearthstone.gamepedia.com/Recruit) soon.

## What this plug-in does for the ..

### [Elven Minstrel](https://hearthstone.gamepedia.com/Elven_Minstrel)

![Display Example](https://raw.githubusercontent.com/VeXHarbinger/DrawPool/master/images/MinstrelDisplaySample.png)

* In the lower left hand corner you see the draw probability for a card that you have one or two copies of.

* If you have one (or more) copies of a card, it will also display the card draw percentage for the card you have the most copies of.

* In the lower right hand corner, it shows the total number of minions in the draw pool and the total deck card count.

### [Witchwood Piper](https://hearthstone.gamepedia.com/Witchwood_Piper)

![Display Example](https://raw.githubusercontent.com/VeXHarbinger/DrawPool/master/images/PiperDisplaySample.png)

* In the lower left hand corner you'll see the draw probability for the lowest cost minion card for your lowest cost minion card, or the minion with this mana cost which you have the fewest copies of.

* If you have multiple minion types for this mana cost, it will also display the card draw probability for the card you have the second most copies of.

  * If you happen to have three or more copies of a minion at this mana cost, it will also display a third statistic indicating the minion's copy count and it's draw probability for the minion type you have the most copies of.

* In the lower right hand corner, it shows the total number of minions in the draw pool and the total deck card count.


## Configuration
You can set where the window is displayed when the player hand hovers over the trigger card from the Options => Plug-in menu