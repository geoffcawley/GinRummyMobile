using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GinRummyMobile.Models
{
    public class Card : IComparable<Card>
    {
        public string Suit { get; set; }
        public int Rank { get; set; }
        public int Score
        {
            get
            {
                if (Rank <= 10) return Rank;
                return 10;
            }
        }

        public int Index
        {
            get
            {
                int i = Rank - 1;
                if (Suit == "S") i += 13;
                if (Suit == "H") i += 26;
                if (Suit == "D") i += 39;
                return i;
            }
        }

        public Card(string rank, string suit)
        {
            if (rank == "A") this.Rank = 1;
            else if (rank == "J") this.Rank = 11;
            else if (rank == "Q") this.Rank = 12;
            else if (rank == "K") this.Rank = 13;
            else this.Rank = int.Parse(rank);
            this.Suit = suit;
        }

        public override string ToString()
        {
            string s = string.Empty;
            switch (Rank)
            {
                case 1:
                    s = "A";
                    break;
                case 11:
                    s = "J";
                    break;
                case 12:
                    s = "Q";
                    break;
                case 13:
                    s = "K";
                    break;
                default:
                    s = Rank.ToString();
                    break;
            }

            switch (Suit.ToUpper())
            {
                case "H":
                    s += (char)003;
                    break;
                case "D":
                    s += (char)004;
                    break;
                case "C":
                    s += (char)005;
                    break;
                case "S":
                    s += (char)006;
                    break;
                default:
                    break;
            }
            return s;
        }

        public string ToLongString()
        {
            string s = string.Empty;
            switch (Rank)
            {
                case 1:
                    s = "Ace";
                    break;
                case 11:
                    s = "Jack";
                    break;
                case 12:
                    s = "Queen";
                    break;
                case 13:
                    s = "King";
                    break;
                default:
                    s = Rank.ToString();
                    break;
            }

            switch (Suit.ToUpper())
            {
                case "H":
                    s += " of Hearts";
                    break;
                case "D":
                    s += " of Diamonds";
                    break;
                case "C":
                    s += " of Clubs";
                    break;
                case "S":
                    s += " of Spades";
                    break;
                default:
                    break;
            }
            return s;
        }

        public int CompareTo(Card other)
        {
            if (other == null) return 1;
            if (Suit[0] < other.Suit[0]) return -1;
            if (Suit[0] > other.Suit[0]) return 1;
            if (Rank < other.Rank) return -1;
            if (Rank > other.Rank) return 1;
            return 0;
        }
    }
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
            string suits = "HDCS";
            for (int s = 0; s < 4; s++)
            {
                Cards.Add(new Card("A", suits[s].ToString()));
                for (int i = 2; i <= 10; i++)
                {
                    Cards.Add(new Card(i.ToString(), suits[s].ToString()));
                }
                Cards.Add(new Card("J", suits[s].ToString()));
                Cards.Add(new Card("Q", suits[s].ToString()));
                Cards.Add(new Card("K", suits[s].ToString()));
            }
        }

        public void Shuffle(int swaps = 500)
        {
            Random random = new Random();

            for (int i = 0; i < swaps; i++)
            {
                int index = random.Next(52);
                Card card = Cards[index];
                Cards[index] = Cards[0];
                Cards[0] = card;
            }
        }

        public Card Draw()
        {
            Card card = Cards.Last();
            Cards.Remove(card);
            return card;
        }

        public Card TopCard { get { return Cards.Last(); } }

        public override string ToString()
        {
            string s = string.Empty;
            foreach (var card in Cards)
            {
                s += card.ToString() + " ";
            }
            return s;
        }
    }

    public class Hand
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        public Hand Copy()
        {
            return new Hand { Cards = this.Cards.ToList<Card>() };
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
                elements.SelectMany((e, i) =>
                    elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int size)
        {
            var window = new T[size];
            int index = 0;

            foreach (var item in source)
            {
                window[index++] = item;

                if (index == size)
                {
                    yield return window;
                    window = new T[size];
                    index = 0;
                }
            }
        }
    }
}
