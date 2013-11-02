﻿using Dominion;
using Dominion.Strategy;
using CardTypes = Dominion.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategies
{
    public class BigMoneyCouncilRoomEarlyProvince
        : Strategy
    {
        public static PlayerAction Player()
        {
            return new PlayerAction(
                        "BigMoneyCouncilRoomEarlyProvince",
                        purchaseOrder: PurchaseOrder());
        }

        private static CardPickByPriority PurchaseOrder()
        {
            return new CardPickByPriority(
                        CardAcceptance.For(Cards.Province, gameState => CountAllOwned(Cards.Gold, gameState) > 0),
                        CardAcceptance.For(Cards.Duchy, gameState => CountOfPile(Cards.Province, gameState) <= 5),
                        CardAcceptance.For(Cards.Estate, gameState => CountOfPile(Cards.Province, gameState) <= 2),
                        CardAcceptance.For(Cards.Gold),
                        CardAcceptance.For(Cards.CouncilRoom, gameState => CountAllOwned(Cards.CouncilRoom, gameState) < 2),
                        CardAcceptance.For(Cards.Silver));
        }
    }
}

