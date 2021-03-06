﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CardTypes = Dominion.CardTypes;
using Dominion;
using Dominion.Strategy;

namespace Dominion.Strategy
{
    public class MissingDefaults
    {
        public static CardShapedObject[] CardsWithoutDefaultBehaviors = new CardShapedObject[]
        {
            // implemented cards that require default behaviors            
            Cards.Embargo,            
            Cards.Graverobber,
            Cards.Haven,            
            Cards.Inn,
            Cards.Journeyman,        
            Cards.Mandarin,            
            Cards.Minion,          
            Cards.NativeVillage,
            Cards.Navigator,
            Cards.Oracle,
            Cards.Pawn,
            Cards.PirateShip,            
            Cards.Squire,                        
            Cards.Torturer,
            Cards.Tournament,       
       
            // Adventures

            Cards.Amulet,
            Cards.Artificer,
            Cards.BridgeTroll,
            Cards.CaravanGuard,
            Cards.Champion,
            Cards.CoinOfTheRealm,
            Cards.Disciple,
            Cards.DistantLands,
            Cards.Duplicate,            
            Cards.Fugitive,
            Cards.Gear,
            Cards.Giant,
            Cards.Guide,
            Cards.HauntedWoods,
            Cards.Hero,
            Cards.Hireling,            
            Cards.Page,
            Cards.Peasant,            
            Cards.Ranger,
            Cards.RatCatcher,            
            Cards.Relic,
            Cards.RoyalCarriage,
            Cards.Soldier,
            Cards.SwampHag,
            Cards.Teacher,
            Cards.Transmogrify,
            Cards.TreasureHunter,            
            Cards.Warrior,
            Cards.WineMerchant,

            // events

            Cards.Alms,
            Cards.Ball,
            Cards.Borrow,
            Cards.Bonfire,
            Cards.Expedition,
            Cards.Ferry,
            Cards.Inheritance,
            Cards.LostArts,
            Cards.Mission,
            Cards.PathFinding,
            Cards.Pilgrimage,
            Cards.Plan,
            Cards.Quest,
            Cards.Raid,
            Cards.Save,
            Cards.ScoutingParty,
            Cards.Seaway,
            Cards.Trade,
            Cards.Training,
            Cards.TravellingFair
        };

        public static IEnumerable<CardShapedObject> FullyImplementedKingdomCards()
        {
            foreach(Card card in Dominion.Cards.AllKingdomCardsList)
            {
                if (Cards.UnimplementedCards.Contains(card))
                    continue;
                if (CardsWithoutDefaultBehaviors.Contains(card))
                    continue;

                yield return card;
            }
        }

        public static IEnumerable<CardShapedObject> UnImplementedKingdomCards()
        {

            foreach (CardShapedObject card in Cards.UnimplementedCards)
            {
                yield return card;
            }

            foreach (CardShapedObject card in CardsWithoutDefaultBehaviors)
            {
                yield return card;
            }
        }
    }
}
