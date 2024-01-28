using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GinRummyMobile.Models
{
    public class Player
    {
        public Player(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; } = string.Empty;

        public Hand Hand { get; set; } = new Hand();
    }
    public class Game
    {
        public static Player[] Players { get; set; } =
        {
            new Player("Player 1"),
            new Player("Player 2")
        };

        public static int ActivePlayer { get; set; } = 0;

        public static Deck Stock { get; set; } = new Deck();

        public static Deck Discard { get; set; } = new Deck();

        static Game() { }

        public static void StartNewGame()
        {
            Stock = new Deck();
            Stock.Shuffle();
            Discard.Cards.Clear();
            Players[0].Hand.Cards.Clear();
            Players[1].Hand.Cards.Clear();
            for (int i = 0; i < 10; i++)
            {
                Players[0].Hand.Cards.Add(Stock.Draw());
                Players[1].Hand.Cards.Add(Stock.Draw());
            }
        }

    }
}
