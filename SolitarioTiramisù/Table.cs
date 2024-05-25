using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using static SolitarioTiramisu.Deck;

namespace SolitarioTiramisu
{
    class Table
    {
        public Deck Deck = new Deck();

        // TODO: implementare coerenza tra carte generate a video e vari mazzetti gestiti nel backend come degli stack (guardare sotto)

        // mazzetti di appoggio su cui fare spostamenti
        public Stack<Card> MiniDeck1 = new Stack<Card>();
        public Stack<Card> MiniDeck2 = new Stack<Card>();
        public Stack<Card> MiniDeck3 = new Stack<Card>();
        public Stack<Card> MiniDeck4 = new Stack<Card>();

        // mazzetti principali in cui fare la scala
        public Stack<Card> StairDeck1 = new Stack<Card>();
        public Stack<Card> StairDeck2 = new Stack<Card>();
        public Stack<Card> StairDeck3 = new Stack<Card>();
        public Stack<Card> StairDeck4 = new Stack<Card>();

        // muovere carte da a, tra i mazzi inferiori 
        public bool MinorMoveCard(ref Card tmpCard, Stack<Card> to)
        {
            Stack<Card> from = tmpCard.Position;

            if (to.TryPeek(out Card tmpCard2))
            {
                if (tmpCard.Seed == tmpCard2.Seed)
                {
                    SetCardPosition(ref tmpCard, to);
                    PushInDeck(ref tmpCard2, to);
                    PushInDeck(ref tmpCard, to);
                    from.Pop();
                    return true;
                }

                PushInDeck(ref tmpCard2, to);
                PushInDeck(ref tmpCard, from);
                return false;
            }
            else
            {
                SetCardPosition(ref tmpCard, to);
                PushInDeck(ref tmpCard, to);
                from.Pop();
                return true;
            }
        }

        // muovere carte da mazzi inferiori a mazzi superiori per fare la scala
        public void StairMoveCard(Stack<Card> from, Stack<Card> to)
        {
            Card tmpCard = from.Pop();

            if (to.TryPeek(out Card tmpCard2))
            {
                if (tmpCard.Seed == tmpCard2.Seed && tmpCard.Value > tmpCard2.Value)
                {
                    to.Push(tmpCard2);
                    to.Push(tmpCard);
                }
                else
                {
                    from.Push(tmpCard);
                    throw new InvalidOperationException("Invalid move");
                }
            }
            else
            {
                to.Push(tmpCard);
            }
        }

        // rimischiare il mazzo quando finisce. si puo fare solo 1 volta. Da chiamare una volta per ogni mazzetto.
        public void RedistributeDeck(Stack<Card> miniDeck)
        {
            if (Deck.Count() == 0)
            {
                while (miniDeck.Count > 0)
                {
                    Card tmp = miniDeck.Pop();
                    Deck.Push(ref tmp);
                }
            }
        }

        public Card DrawCardFromDeck()
        {
            if (Deck.Count() > 0)
                return Deck.Pop();
            else
                throw new InvalidOperationException("Deck is empty.");
        }

        public int Win()
        {
            if (StairDeck1.Count == 10 && StairDeck2.Count == 10 && StairDeck3.Count == 10 && StairDeck4.Count == 10)
            {
                return 0; // ha vinto
            }
            else if (HasLost())
            {
                return 1; // ha perso
            }

            return 2; // può continuare a giocare
        }

        private bool HasLost()
        {
            List<Card> tmpList = new List<Card>
            {
                MiniDeck1.Count > 0 ? MiniDeck1.Peek() : null,
                MiniDeck2.Count > 0 ? MiniDeck2.Peek() : null,
                MiniDeck3.Count > 0 ? MiniDeck3.Peek() : null,
                MiniDeck4.Count > 0 ? MiniDeck4.Peek() : null
            }.Where(card => card != null).ToList();

            List<Card> stairList = new List<Card>
            {
                StairDeck1.Count > 0 ? StairDeck1.Peek() : null,
                StairDeck2.Count > 0 ? StairDeck2.Peek() : null,
                StairDeck3.Count > 0 ? StairDeck3.Peek() : null,
                StairDeck4.Count > 0 ? StairDeck4.Peek() : null
            }.Where(card => card != null).ToList();

            if (tmpList.All(card => card != null) && stairList.All(card => card != null))
            {
                foreach (Card tmpCard in tmpList)
                {
                    foreach (Card stairCard in stairList)
                    {
                        if (stairCard.Seed == tmpCard.Seed && stairCard.Value + 1 < tmpCard.Value)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void PushInDeck(ref Card card, Stack<Card> to)
        {
            to.Push(card);
        }

        public void SetCardPosition(ref Card card, Stack<Card> to)
        {
            card.Position = to;
        }
    }
}
