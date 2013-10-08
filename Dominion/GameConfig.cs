﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion
{
    public class GameConfig
    {
        public readonly bool useShelters;
        public readonly bool useColonyAndPlatinum;
        public readonly Card[] kingdomPiles;
        public readonly IEnumerable<CardCountPair> startingDeck;

        public GameConfig(bool useShelters, bool useColonyAndPlatinum, Card[] supplyPiles, IEnumerable<CardCountPair> startingDeck)
        {
            this.useShelters = useShelters;
            this.useColonyAndPlatinum = useColonyAndPlatinum;
            this.kingdomPiles = supplyPiles;
            this.startingDeck = startingDeck;
        }

        public GameConfig(bool useShelters, bool useColonyAndPlatinum, params Card[] supplyPiles)
        {
            this.useShelters = useShelters;
            this.useColonyAndPlatinum = useColonyAndPlatinum;
            this.kingdomPiles = supplyPiles;
            this.startingDeck = null;
        }

        public GameConfig(params Card[] supplyPiles)
        {
            this.useShelters = false;
            this.useColonyAndPlatinum = false;
            this.kingdomPiles = supplyPiles;            
        }

        public IEnumerable<CardCountPair>[] StartingDecks(int playerCount)
        {
            return GetUniformStartingDecks(playerCount, this.StartingDeck);            
        }

        public static IEnumerable<CardCountPair>[] GetUniformStartingDecks(int playerCount, IEnumerable<CardCountPair> startingDeck)
        {            
            var result = new IEnumerable<CardCountPair>[playerCount];
            for (int i = 0; i < playerCount; ++i)
                result[i] = startingDeck;

            return result;
        }

        public IEnumerable<CardCountPair> StartingDeck
        {
            get
            {
                if (this.startingDeck != null)
                    return this.startingDeck;

                if (this.useShelters)
                {
                    return 
                        new CardCountPair[] {
                            new CardCountPair(Card.Type<CardTypes.Copper>(), 7),
                            new CardCountPair(Card.Type<CardTypes.Hovel>(), 1),
                            new CardCountPair(Card.Type<CardTypes.Necropolis>(), 1),
                            new CardCountPair(Card.Type<CardTypes.OvergrownEstate>(), 1)
                        };
                }
                else
                {
                    return
                        new CardCountPair[] {
                            new CardCountPair(Card.Type<CardTypes.Copper>(), 7),
                            new CardCountPair(Card.Type<CardTypes.Estate>(), 3) 
                        };
                }
            }
        }

        public PileOfCards[] GetSupplyPiles(int playerCount, Random random)
        {
            var supplyCardPiles = new List<PileOfCards>(capacity: 20);            

            int curseCount = (playerCount - 1) * 10;
            int ruinsCount = curseCount;
            int victoryCount = (playerCount == 2) ? 8 : 12;

            // cards always in the supply
            Add<CardTypes.Copper>(supplyCardPiles, 60);
            Add<CardTypes.Silver>(supplyCardPiles, 40);
            Add<CardTypes.Gold>(supplyCardPiles, 30);
            Add<CardTypes.Curse>(supplyCardPiles, curseCount);
            Add<CardTypes.Estate>(supplyCardPiles, victoryCount + (!this.useShelters ? playerCount * 3 : 0));
            Add<CardTypes.Duchy>(supplyCardPiles, victoryCount);
            Add<CardTypes.Province>(supplyCardPiles, victoryCount);
            
            if (this.useColonyAndPlatinum)
            {
                Add<CardTypes.Colony>(supplyCardPiles, victoryCount);
                Add<CardTypes.Platinum>(supplyCardPiles, 20);
            }

            if (this.kingdomPiles.Where(card => card.potionCost != 0).Any())
            {
                Add<CardTypes.Province>(supplyCardPiles, 16);
            }

            if (this.kingdomPiles.Where(card => card.requiresRuins).Any())
            {
                supplyCardPiles.Add(CreateRuins(ruinsCount, random));
            }          

            foreach (Card card in this.kingdomPiles)
            {
                if (card.isVictory)
                {
                    Add(supplyCardPiles, victoryCount, card);
                }
                else
                {
                    Add(supplyCardPiles, card.defaultSupplyCount, card);
                }                
            }

            return supplyCardPiles.ToArray();
        }

        public PileOfCards[] GetNonSupplyPiles()
        {
            var nonSupplyCardPiles = new List<PileOfCards>();
            
            if (this.kingdomPiles.Where(card => card.requiresSpoils).Any())
            {
                Add<CardTypes.Spoils>(nonSupplyCardPiles, 16);
            }

            if (this.kingdomPiles.Where(card => card.Is<CardTypes.Hermit>()).Any())
            {
                Add<CardTypes.Madman>(nonSupplyCardPiles, 10);
            }
            if (this.kingdomPiles.Where(card => card.Is<CardTypes.Urchin>()).Any())
            {
                Add<CardTypes.Mercenary>(nonSupplyCardPiles, 10);
            }            

            return nonSupplyCardPiles.ToArray();
        }

        private static PileOfCards CreateRuins(int ruinsCount, Random random)
        {
            int ruinCountPerPile = 10;
            var allRuinsCards = new ListOfCards();
            allRuinsCards.AddNCardsToTop(Card.Type<CardTypes.AbandonedMine>(), ruinCountPerPile);
            allRuinsCards.AddNCardsToTop(Card.Type<CardTypes.RuinedMarket>(), ruinCountPerPile);
            allRuinsCards.AddNCardsToTop(Card.Type<CardTypes.RuinedLibrary>(), ruinCountPerPile);
            allRuinsCards.AddNCardsToTop(Card.Type<CardTypes.RuinedVillage>(), ruinCountPerPile);
            allRuinsCards.AddNCardsToTop(Card.Type<CardTypes.Survivors>(), ruinCountPerPile);

            allRuinsCards.Shuffle(random);

            var result = new PileOfCards(Card.Type<CardTypes.Ruins>());

            for (int i = 0; i < ruinsCount; ++i)
            {
                Card card = allRuinsCards.DrawCardFromTop();
                if (card == null)
                {
                    throw new Exception("Not enough ruins available.");
                }
                result.AddCardToTop(card);
            }
            result.EraseKnownCountKnowledge();

            return result;
        }

        private static void Add<CardType>(List<PileOfCards> cardPiles, int initialCount)
            where CardType : Card, new()
        {

            Add(cardPiles, initialCount, Card.Type<CardType>());
        }

        private static void Add(List<PileOfCards> cardPiles, int initialCount, Card protoType)
        {
            cardPiles.Add(new PileOfCards(protoType, initialCount));
        }
    }
}
