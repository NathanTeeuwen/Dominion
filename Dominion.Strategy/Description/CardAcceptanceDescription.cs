﻿using Dominion;
using System.Linq;

namespace Dominion.Strategy.Description
{
    public class CardAcceptanceDescription
    {
        public readonly Card card;
        public readonly MatchDescription[] matchDescriptions;

        public CardAcceptanceDescription(Card card, CountSource countSource, Card matchOn, Comparison comparison, int threshhold)
            : this(card, new MatchDescription(countSource, matchOn, comparison, threshhold))
        {            
        }

        public CardAcceptanceDescription(Card card, params MatchDescription[] matchDescriptions)
        {
            this.card = card;
            this.matchDescriptions = matchDescriptions;
        }

        public bool GameStatePredicate(GameState gameState)
        {
            foreach (MatchDescription description in this.matchDescriptions)
            {
                if (!description.GameStatePredicate(gameState))
                    return false;
            }
            return true;
        }

        public CardAcceptance ToCardAcceptance()
        {
            return new CardAcceptance(this.card, this.GameStatePredicate);
        }

        public CardAcceptanceDescription Clone()
        {
            return new CardAcceptanceDescription(this.card, matchDescriptions.Select(m => m.Clone()).ToArray());
        }

        public void WriteText(System.IO.TextWriter writer)
        {
            writer.Write(card.name);
            this.matchDescriptions[0].WriteText(writer);
        }
    }
}