using GinRummyMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GinRummyMobile
{
    public enum TurnPhase
    {
        Start = 0,
        Draw = 1,
        Discard = 2,
        End = 3,
        Finished = 4,
    };

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GinRummyPage : ContentPage
    {
        Card activeCard;

        TurnPhase TurnPhase = TurnPhase.Start;
        string LastTurnResult = string.Empty;

        string cardSource = string.Empty;
        int activeHandIndex = -1;
        int activeCardIndex = -1;

        public GinRummyPage()
        {
            InitializeComponent();
            Game.StartNewGame();
            RefreshHandImages();
            RefreshLabels();
        }

        private void ClearActiveCard()
        {
            activeCard = null;
            activeCardIndex = -1;
            activeHandIndex = -1;
            cardSource = string.Empty;
        }

        private void RefreshLabels()
        {
            PlayerLabel.Text = $"{Game.Players[Game.ActivePlayer].Name}";
            LastTurnResultLabel.Text = LastTurnResult;
            if (GinRummy.HasGin(Game.Players[Game.ActivePlayer].Hand))
            {
                GameStateLabel.Text = "Gin Rummy!";
            }
            else
            {
                GameStateLabel.Text = string.Empty;
            }

            if (TurnPhase == TurnPhase.End)
            {
                TurnPhaseLabel.Text = "End Your Turn";
                StartTurnButton.IsVisible = false;
                EndTurnButton.IsVisible = true;
            }
            else if (TurnPhase == TurnPhase.Draw)
            {
                if (Game.Discard.Cards.Count > 0)
                {
                    TurnPhaseLabel.Text = "Draw from Stock or Discard Pile";
                }
                else
                {
                    TurnPhaseLabel.Text = "Draw from Stock";
                }
                StartTurnButton.IsVisible = false;
                EndTurnButton.IsVisible = false;
            }
            else if (TurnPhase == TurnPhase.Discard)
            {
                TurnPhaseLabel.Text = "Discard a card";
                StartTurnButton.IsVisible = false;
                EndTurnButton.IsVisible = false;
            }
            else if (TurnPhase == TurnPhase.Start)
            {
                TurnPhaseLabel.Text = $"{Game.Players[Game.ActivePlayer].Name} Start";
                StartTurnButton.IsVisible = true;
                EndTurnButton.IsVisible = false;
            }
            else if (TurnPhase == TurnPhase.Finished)
            {
                TurnPhaseLabel.Text = $"{Game.Players[Game.ActivePlayer].Name} Wins the Round";
                StartTurnButton.IsVisible = false;
                EndTurnButton.IsVisible = false;
            }
        }

        private void RefreshHandImages()
        {
            StockLabel.Text = $"Stock ({Game.Stock.Cards.Count})";

            if (TurnPhase != TurnPhase.Start)
            {
                HandImg0.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[0].Index}.png";
                HandImg1.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[1].Index}.png";
                HandImg2.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[2].Index}.png";
                HandImg3.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[3].Index}.png";
                HandImg4.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[4].Index}.png";
                HandImg5.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[5].Index}.png";
                HandImg6.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[6].Index}.png";
                HandImg7.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[7].Index}.png";
                HandImg8.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[8].Index}.png";
                HandImg9.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[9].Index}.png";

                if (Game.Players[Game.ActivePlayer].Hand.Cards.Count > 10)
                {
                    HandImg10.IsVisible = true;
                    HandImg10.Source = $"card{Game.Players[Game.ActivePlayer].Hand.Cards[10].Index}.png";
                }
                else
                {
                    HandImg10.IsVisible = false;
                    HandImg10.Source = $"empty.png";
                }
            }
            else
            {
                HandImg0.Source = $"back.png";
                HandImg1.Source = $"back.png";
                HandImg2.Source = $"back.png";
                HandImg3.Source = $"back.png";
                HandImg4.Source = $"back.png";
                HandImg5.Source = $"back.png";
                HandImg6.Source = $"back.png";
                HandImg7.Source = $"back.png";
                HandImg8.Source = $"back.png";
                HandImg9.Source = $"back.png";
                HandImg10.IsVisible = false;
                HandImg10.Source = $"back.png";
            }
        }

        private void DragDeck(object sender, DragStartingEventArgs e)
        {
            if (Game.Stock.Cards.Count == 0)
            {
                return;
            }
            cardSource = "deck";
            activeCard = Game.Stock.TopCard;
            e.Data.Text = activeCard.ToString();
        }

        private void DragDiscard(object sender, DragStartingEventArgs e)
        {
            if (Game.Discard.Cards.Count == 0)
            {
                return;
            }
            cardSource = "discard";
            activeCard = Game.Discard.TopCard;
            e.Data.Text = activeCard.ToString();
        }

        private void DropDiscard(object sender, DropEventArgs e)
        {
            e.Handled = true;
            if (cardSource == "deck")
            {
                ClearActiveCard();
            }
            else if (cardSource == "hand")
            {
                if (Game.Players[Game.ActivePlayer].Hand.Cards.Count > 10)
                {
                    Game.Players[Game.ActivePlayer].Hand.Cards.Remove(activeCard);
                    Game.Discard.Cards.Add(activeCard);
                    DiscardImg.Source = $"card{activeCard.Index}.png";
                    TurnPhase = TurnPhase.End;
                    ClearActiveCard();
                    RefreshHandImages();
                    if (GinRummy.HasGin(Game.Players[Game.ActivePlayer].Hand))
                    {
                        TurnPhase = TurnPhase.Finished;
                    }
                }
            }
            RefreshLabels();
        }

        private bool DragHand(int handIndex)
        {
            if (handIndex >= Game.Players[Game.ActivePlayer].Hand.Cards.Count) { return false; }
            cardSource = "hand";
            activeCard = Game.Players[Game.ActivePlayer].Hand.Cards[handIndex];
            activeHandIndex = handIndex;
            activeCardIndex = Game.Players[Game.ActivePlayer].Hand.Cards[handIndex].Index;
            return true;
        }

        private bool DropHand(int handIndex)
        {
            if (cardSource == "deck")
            {
                if (Game.Players[Game.ActivePlayer].Hand.Cards.Count > 10 || Game.Stock.Cards.Count <= 0 || TurnPhase != TurnPhase.Draw)
                {
                    ClearActiveCard();
                    return false;
                }
                Game.Players[Game.ActivePlayer].Hand.Cards.Insert(handIndex, Game.Stock.Draw());
                TurnPhase = TurnPhase.Discard;
                ClearActiveCard();
                RefreshHandImages();
                RefreshLabels();
                if (Game.Stock.Cards.Count == 0)
                {
                    DeckImg.Source = "empty.png";
                }
                LastTurnResult = $"{Game.Players[Game.ActivePlayer].Name} drew from Stock";
                return true;
            }
            else if (cardSource == "discard")
            {
                if (Game.Players[Game.ActivePlayer].Hand.Cards.Count > 10 || Game.Discard.Cards.Count <= 0 || TurnPhase != TurnPhase.Draw)
                {
                    ClearActiveCard();
                    return false;
                }
                Game.Players[Game.ActivePlayer].Hand.Cards.Insert(handIndex, Game.Discard.Draw());
                TurnPhase = TurnPhase.Discard;
                string cardString = activeCard.ToLongString();
                ClearActiveCard();
                RefreshHandImages();
                RefreshLabels();
                if (Game.Discard.Cards.Count == 0)
                {
                    DiscardImg.Source = "empty.png";
                }
                else
                {
                    DiscardImg.Source = $"card{Game.Discard.TopCard.Index}.png";
                }
                LastTurnResult = $"{Game.Players[Game.ActivePlayer].Name} drew the {cardString} from Discard";
                return true;
            }
            else if (cardSource == "hand")
            {
                int i = handIndex;
                if (i >= Game.Players[Game.ActivePlayer].Hand.Cards.Count)
                {
                    i = Game.Players[Game.ActivePlayer].Hand.Cards.Count - 1;
                }
                Game.Players[Game.ActivePlayer].Hand.Cards.Remove(activeCard);
                Game.Players[Game.ActivePlayer].Hand.Cards.Insert(i, activeCard);
                ClearActiveCard();
                RefreshHandImages();
                RefreshLabels();
                return true;
            }
            ClearActiveCard();
            return false;
        }

        async void OnStartTurnButtonClicked(object sender, EventArgs e)
        {
            TurnPhase = TurnPhase.Draw;
            RefreshHandImages();
            RefreshLabels();
        }

        async void OnEndTurnButtonClicked(object sender, EventArgs e)
        {
            if (Game.ActivePlayer == 0) Game.ActivePlayer = 1;
            else Game.ActivePlayer = 0;
            TurnPhase = TurnPhase.Start;
            RefreshHandImages();
            RefreshLabels();
        }

        async void OnNewGameButtonClicked(object sender, EventArgs e)
        {
            TurnPhase = TurnPhase.Start;
            Game.StartNewGame();
            DiscardImg.Source = "empty.png";
            RefreshLabels();
            RefreshHandImages();
        }

        #region HandEventHandlers
        private async void DragHand0(object sender, DragStartingEventArgs e)
        {
            DragHand(0);
        }

        private async void DropHand0(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(0);
        }

        private async void DragHand1(object sender, DragStartingEventArgs e)
        {
            DragHand(1);
        }

        private async void DropHand1(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(1);
        }

        private async void DragHand2(object sender, DragStartingEventArgs e)
        {
            DragHand(2);
        }

        private async void DropHand2(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(2);
        }

        private async void DragHand3(object sender, DragStartingEventArgs e)
        {
            DragHand(3);
        }

        private async void DropHand3(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(3);
        }

        private async void DragHand4(object sender, DragStartingEventArgs e)
        {
            DragHand(4);
        }

        private async void DropHand4(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(4);
        }

        private async void DragHand5(object sender, DragStartingEventArgs e)
        {
            DragHand(5);
        }

        private async void DropHand5(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(5);
        }

        private async void DragHand6(object sender, DragStartingEventArgs e)
        {
            DragHand(6);
        }

        private async void DropHand6(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(6);
        }

        private async void DragHand7(object sender, DragStartingEventArgs e)
        {
            DragHand(7);
        }

        private async void DropHand7(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(7);
        }

        private async void DragHand8(object sender, DragStartingEventArgs e)
        {
            DragHand(8);
        }

        private async void DropHand8(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(8);
        }

        private async void DragHand9(object sender, DragStartingEventArgs e)
        {
            DragHand(9);
        }

        private async void DropHand9(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(9);
        }

        private async void DragHand10(object sender, DragStartingEventArgs e)
        {
            DragHand(10);
        }

        private async void DropHand10(object sender, DropEventArgs e)
        {
            e.Handled = true;
            DropHand(10);
        }
        #endregion
    }
}