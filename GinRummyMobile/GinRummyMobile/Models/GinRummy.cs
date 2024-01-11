using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GinRummyMobile.Models
{
    public class GinRummy
    {

        public static bool IsGinRummy(Hand hand)
        {
            // Check if the hand has 10 cards
            if (hand.Cards.Count != 10)
            {
                return false;
            }

            // Sort the cards in the hand
            hand.Cards.Sort((card1, card2) => card1.Score.CompareTo(card2.Score));

            // Check for a set or run of four cards
            for (int i = 0; i <= 6; i++)
            {
                if (hand.Cards[i].Rank == hand.Cards[i + 1].Rank &&
                    hand.Cards[i + 1].Rank == hand.Cards[i + 2].Rank &&
                    hand.Cards[i + 2].Rank == hand.Cards[i + 3].Rank)
                {
                    return IsGinRummy(new Hand
                    {
                        Cards = hand.Cards.Except(new[] { hand.Cards[i], hand.Cards[i + 1], hand.Cards[i + 2], hand.Cards[i + 3] }).ToList()
                    });
                }
            }

            // Check for two sets or runs of three cards each
            var remainingCards = new List<Card>(hand.Cards);

            for (int i = 0; i <= 7; i++)
            {
                for (int j = i + 1; j <= 8; j++)
                {
                    for (int k = j + 1; k <= 9; k++)
                    {
                        var set1 = new List<Card> { hand.Cards[i], hand.Cards[j], hand.Cards[k] };
                        remainingCards.RemoveAll(card => set1.Contains(card));

                        for (int m = 0; m <= 5; m++)
                        {
                            for (int n = m + 1; n <= 6; n++)
                            {
                                for (int o = n + 1; o <= 7; o++)
                                {
                                    var set2 = new List<Card> { remainingCards[m], remainingCards[n], remainingCards[o] };
                                    remainingCards.RemoveAll(card => set2.Contains(card));

                                    if (IsSet(set1) && IsSet(set2))
                                    {
                                        return true;
                                    }

                                    // Restore the removed cards for the next iteration
                                    remainingCards.AddRange(set2);
                                }
                            }
                        }

                        // Restore the removed cards for the next iteration
                        remainingCards.AddRange(set1);
                    }
                }
            }

            return false;
        }

        private static bool IsSet(List<Card> cards)
        {
            cards.Sort((card1, card2) => card1.Score.CompareTo(card2.Score));

            if (cards[0].Rank == cards[1].Rank && cards[1].Rank == cards[2].Rank)
            {
                return true;
            }

            if (cards[0].Suit == cards[1].Suit &&
                cards[1].Suit == cards[2].Suit &&
                cards[2].Suit == cards[0].Suit &&
                cards[0].Score + 1 == cards[1].Score &&
                cards[1].Score + 1 == cards[2].Score)
            {
                return true;
            }

            return false;
        }

        public static bool HasGin(Hand oghand)
        {
            Hand hand = oghand.Copy();

            //var fiveCardMelds = GetFiveCardMelds(hand);
            //foreach (var fiveCard in fiveCardMelds)
            //{
            //    var fiveCardHand = new Hand { Cards = hand.Cards.Except(fiveCard).ToList() };
            //    if (GetFiveCardMelds(fiveCardHand).Count() > 0)
            //    {
            //        Console.WriteLine("Gin!");
            //    }
            //}

            var fourCardMelds = GetFourCardMelds(hand);

            foreach (var fourCard in fourCardMelds)
            {
                var sixCardHand = new Hand { Cards = hand.Cards.Except(fourCard).ToList() };
                var threeCardMelds = GetThreeCardMelds(sixCardHand);
                foreach (var threeCard in threeCardMelds)
                {
                    var threeCardHand = new Hand { Cards = sixCardHand.Cards.Except(threeCard).ToList() };
                    var finalMeld = GetThreeCardMelds(threeCardHand);
                    if (finalMeld.Count > 0)
                    {
                        Console.WriteLine("Gin!");
                        foreach (var card in finalMeld[0])
                        {
                            PrintCard(card);
                        }
                        Console.Write(" ");
                        foreach (var card in threeCard)
                        {
                            PrintCard(card);
                        }
                        Console.Write(" ");
                        foreach (var card in fourCard)
                        {
                            PrintCard(card);
                        }
                        Console.Write("\n\n");

                        return true;
                    }
                    return false;
                }
            }

            GetThreeCardMelds(hand);

            return false;
        }

        public static List<List<Card>> GetThreeCardMelds(Hand hand)
        {
            var sets = hand.Cards.GroupBy(card => card.Rank)
                .Where(group => group.Count() == 3)
                .SelectMany(group => group.Combinations(3))
                .Select(m => m.ToList()).ToList();

            var sortedcards = hand.Cards;
            sortedcards.Sort();
            List<List<Card>> runs = new List<List<Card>>();
            for (int i = 0; i <= sortedcards.Count - 3; i++)
            {
                if (sortedcards[i].Suit == sortedcards[i + 1].Suit && sortedcards[i + 1].Suit == sortedcards[i + 2].Suit
                    && sortedcards[i].Rank + 1 == sortedcards[i + 1].Rank && sortedcards[i + 1].Rank + 1 == sortedcards[i + 2].Rank)
                {
                    runs.Add(new List<Card> { sortedcards[i], sortedcards[i + 1], sortedcards[i + 2] });
                }
            }
            var melds = sets.Concat(runs).ToList();

            if (melds.Count > 0)
            {
                Console.WriteLine("Melds:");
                foreach (var meld in melds)
                {
                    Console.Write("\t");
                    foreach (var card in meld)
                    {
                        PrintCard(card);
                    }
                    Console.WriteLine();
                }
            }
            return melds;
        }

        public static List<List<Card>> GetFourCardMelds(Hand hand)
        {
            var sets = hand.Cards.GroupBy(card => card.Rank)
                .Where(group => group.Count() == 4)
                .SelectMany(group => group.Combinations(4))
                .Select(m => m.ToList()).ToList();

            var sortedcards = hand.Cards;
            sortedcards.Sort();
            List<List<Card>> runs = new List<List<Card>>();
            for (int i = 0; i <= sortedcards.Count - 4; i++)
            {
                if (sortedcards[i].Suit == sortedcards[i + 1].Suit && sortedcards[i + 1].Suit == sortedcards[i + 2].Suit && sortedcards[i + 2].Suit == sortedcards[i + 3].Suit
                    && sortedcards[i].Rank + 1 == sortedcards[i + 1].Rank && sortedcards[i + 1].Rank + 1 == sortedcards[i + 2].Rank && sortedcards[i + 2].Rank + 1 == sortedcards[i + 3].Rank)
                {
                    runs.Add(new List<Card> { sortedcards[i], sortedcards[i + 1], sortedcards[i + 2], sortedcards[i + 3] });
                }
            }
            var melds = sets.Concat(runs).ToList();

            if (melds.Count > 0)
            {
                Console.WriteLine("Melds:");
                foreach (var meld in melds)
                {
                    Console.Write("\t");
                    foreach (var card in meld)
                    {
                        PrintCard(card);
                    }
                    Console.WriteLine();
                }
            }
            return melds;
        }

        public static List<List<Card>> GetFiveCardMelds(Hand hand)
        {
            var sets = hand.Cards.GroupBy(card => card.Rank)
                .Where(group => group.Count() == 5)
                .SelectMany(group => group.Combinations(5))
                .Select(m => m.ToList()).ToList();

            var sortedcards = hand.Cards;
            sortedcards.Sort();
            List<List<Card>> runs = new List<List<Card>>();
            for (int i = 0; i <= sortedcards.Count - 5; i++)
            {
                if (sortedcards[i].Suit == sortedcards[i + 1].Suit && sortedcards[i + 1].Suit == sortedcards[i + 2].Suit && sortedcards[i + 2].Suit == sortedcards[i + 3].Suit && sortedcards[i + 3].Suit == sortedcards[i + 4].Suit
                    && sortedcards[i].Rank + 1 == sortedcards[i + 1].Rank && sortedcards[i + 1].Rank + 1 == sortedcards[i + 2].Rank && sortedcards[i + 2].Rank + 1 == sortedcards[i + 3].Rank && sortedcards[i + 3].Rank + 1 == sortedcards[i + 4].Rank)
                {
                    runs.Add(new List<Card> { sortedcards[i], sortedcards[i + 1], sortedcards[i + 2], sortedcards[i + 3], sortedcards[i + 4] });
                }
            }
            var melds = sets.Concat(runs).ToList();

            if (melds.Count > 0)
            {
                Console.WriteLine("Melds:");
                foreach (var meld in melds)
                {
                    Console.Write("\t");
                    foreach (var card in meld)
                    {
                        PrintCard(card);
                    }
                    Console.WriteLine();
                }
            }
            return melds;
        }


        public static void PrintCard(Card card)
        {
            if (card.Suit == "H" || card.Suit == "D")
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(card.ToString());
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(card.ToString());
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
