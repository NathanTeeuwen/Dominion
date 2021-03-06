﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dominulator
{
    public class CardAcceptanceDescription
    {
        public const int CountAsManyAsPossible = 11;  // this value comes from the 12th index in the combo box.  0-10 + always

        public DependencyObjectDecl<DominionCard> Card { get; private set; }  // card being bought
        public DependencyObjectDecl<int> Count { get; private set; }          // how many to get

        // secondary match description.  optional
        public DependencyObjectDecl<DominionCard> TestCard { get; private set; }
        public DependencyObjectDecl<Dominion.Strategy.Description.CountSource> CountSource { get; private set; }
        public DependencyObjectDecl<Dominion.Strategy.Description.Comparison> Comparison { get; private set; }
        public DependencyObjectDecl<int> Threshhold { get; private set; }

        public DependencyObjectDecl<bool> SecondaryMatchVisible{ get; private set; }
        public DependencyObjectDecl<bool> SecondaryMatchCardVisible{ get; private set; }
        public DependencyObjectDecl<bool> CountConditionVisible { get; private set; }
        public DependencyObjectDecl<bool> CanSimulateCard { get; private set; }

        public CardAcceptanceDescription()
        {
            this.Card = new DependencyObjectDecl<DominionCard>(this);
            this.Count = new DependencyObjectDecl<int>(this);
            this.TestCard = new DependencyObjectDecl<DominionCard>(this);
            this.CountSource = new DependencyObjectDecl<Dominion.Strategy.Description.CountSource>(this);
            this.Comparison = new DependencyObjectDecl<Dominion.Strategy.Description.Comparison>(this);
            this.Threshhold = new DependencyObjectDecl<int>(this);
            this.SecondaryMatchVisible = new DependencyObjectDecl<bool>(this);
            this.SecondaryMatchCardVisible = new DependencyObjectDecl<bool>(this);
            this.CountConditionVisible = new DependencyObjectDecl<bool>(this);
            this.CanSimulateCard = new DependencyObjectDecl<bool>(this);

            this.CountSource.PropertyChanged += CountSource_PropertyChanged;
            this.Card.PropertyChanged += Card_PropertyChanged;
        }

        void Card_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CanSimulateCard.Value = this.Card.Value.CanSimulate;
        }

        void CountSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(this.CountSource.Value)
            {
                case Dominion.Strategy.Description.CountSource.Always:
                {
                    this.SecondaryMatchVisible.Value = false;
                    this.SecondaryMatchCardVisible.Value = false;
                    this.CountConditionVisible.Value = false;
                    break;
                }
                case Dominion.Strategy.Description.CountSource.AvailableCoin:
                {
                    this.SecondaryMatchVisible.Value = true;
                    this.SecondaryMatchCardVisible.Value = false;
                    this.CountConditionVisible.Value = true;
                    break;
                }
                case Dominion.Strategy.Description.CountSource.CardBeingPlayedIs:
                {
                    this.SecondaryMatchVisible.Value = true;
                    this.SecondaryMatchCardVisible.Value = true;
                    this.CountConditionVisible.Value = false;
                    break;
                }  
                default:
                {
                    this.SecondaryMatchVisible.Value = true;
                    this.SecondaryMatchCardVisible.Value = true;
                    this.CountConditionVisible.Value = true;
                    break;
                }
            }
        }

        public CardAcceptanceDescription(string name)
            : this()
        {
            this.Card.Value = new DominionCard(name);
        }

        public CardAcceptanceDescription(DominionCard card)
            : this()
        {
            this.Card.Value = card;
            this.Count.Value = CountAsManyAsPossible;
            this.SecondaryMatchVisible.Value = false;
            this.TestCard.Value = card;
            this.Comparison.Value = Dominion.Strategy.Description.Comparison.LessThan;
            this.Threshhold.Value = 1;
            this.CountSource.Value = Dominion.Strategy.Description.CountSource.Always;            
        }

        public string CountText
        {
            get
            {
                return this.Count.Value.ToString();
            }
        }        

        public static CardAcceptanceDescription PopulateFrom(Dominion.Strategy.Description.CardAcceptanceDescription descr)
        {
            var result = new CardAcceptanceDescription();
            result.Card.Value = DominionCard.Create(descr.card);

            if (descr.matchDescriptions.Length != 1 && descr.matchDescriptions.Length != 2)
                throw new Exception("Support only one match description in addition to count all owned");

            if (descr.matchDescriptions[0].countSource == Dominion.Strategy.Description.CountSource.Always)
            {
                result.Count.Value = CountAsManyAsPossible;
            }
            else if (descr.matchDescriptions[0].countSource == Dominion.Strategy.Description.CountSource.CountAllOwned &&
                     descr.matchDescriptions[0].cardType == descr.card && 
                     descr.matchDescriptions[0].comparison == Dominion.Strategy.Description.Comparison.LessThan) 
            {
                result.Count.Value = descr.matchDescriptions[0].countThreshHold;
            }
            else
            {
                throw new Exception("First match description needs to describe how many of the card to own");
            }

            if (descr.matchDescriptions.Length == 2)
            {
                result.TestCard.Value = descr.matchDescriptions[1].cardType != null ? DominionCard.Create(descr.matchDescriptions[1].cardType) : null;
                result.Threshhold.Value = descr.matchDescriptions[1].countThreshHold;
                result.Comparison.Value = descr.matchDescriptions[1].comparison;
                result.CountSource.Value = descr.matchDescriptions[1].countSource;
            }
            else
            {
                result.CountSource.Value = Dominion.Strategy.Description.CountSource.Always;
            }
            
            return result;
        }

        public Dominion.Strategy.Description.CardAcceptanceDescription ConvertToDominionStrategy()

        {
            var list = new List<Dominion.Strategy.Description.MatchDescription>();

            if (this.Count.Value == CountAsManyAsPossible)
            {
                list.Add(new Dominion.Strategy.Description.MatchDescription(this.Card.Value.dominionCard));
            }
            else
            {
                list.Add(new Dominion.Strategy.Description.MatchDescription(
                    Dominion.Strategy.Description.CountSource.CountAllOwned,
                    this.Card.Value.dominionCard,
                    Dominion.Strategy.Description.Comparison.LessThan,
                    this.Count.Value));
            }

            if (this.CountSource.Value != Dominion.Strategy.Description.CountSource.Always)
            {                                
                list.Add(new Dominion.Strategy.Description.MatchDescription(
                        this.CountSource.Value,
                        this.TestCard.Value != null ? this.TestCard.Value.dominionCard : null,
                        this.Comparison.Value,
                        this.Threshhold.Value));             
            }
            
            return new Dominion.Strategy.Description.CardAcceptanceDescription(
                this.Card.Value.dominionCard,
                list.ToArray());               
        }
    }
}