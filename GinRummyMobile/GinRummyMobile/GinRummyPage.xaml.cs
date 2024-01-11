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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GinRummyPage : ContentPage
    {
        Deck stock = new Deck();
        Deck discard = new Deck();
        Card activeCard;

        Hand hand = new Hand();

        string cardSource = "";
        int activeHandIndex = -1;
        int activeCardIndex = -1;

        public GinRummyPage()
        {
            InitializeComponent();
            stock.Shuffle();
            discard.Cards.Clear();
            for (int i = 0; i < 10; i++)
            {
                hand.Cards.Add(stock.Draw());
            }
            RefreshHandImages();
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
            if (GinRummy.HasGin(hand))
            {
                GameStateLabel.Text = "Gin Rummy!";
            }
        }

        private void RefreshHandImages()
        {

            HandImg0.Source = $"card{hand.Cards[0].Index}.png";
            HandImg1.Source = $"card{hand.Cards[1].Index}.png";
            HandImg2.Source = $"card{hand.Cards[2].Index}.png";
            HandImg3.Source = $"card{hand.Cards[3].Index}.png";
            HandImg4.Source = $"card{hand.Cards[4].Index}.png";
            HandImg5.Source = $"card{hand.Cards[5].Index}.png";
            HandImg6.Source = $"card{hand.Cards[6].Index}.png";
            HandImg7.Source = $"card{hand.Cards[7].Index}.png";
            HandImg8.Source = $"card{hand.Cards[8].Index}.png";
            HandImg9.Source = $"card{hand.Cards[9].Index}.png";

            if (hand.Cards.Count > 10)
            {
                HandImg10.IsVisible = true;
                HandImg10.Source = $"card{hand.Cards[10].Index}.png";
            }
            else
            {
                HandImg10.IsVisible = false;
                HandImg10.Source = $"empty.png";
            }
        }

        private void DragDeck(object sender, DragStartingEventArgs e)
        {
            if (stock.Cards.Count == 0)
            {
                return;
            }
            cardSource = "deck";
            activeCard = stock.TopCard;
            e.Data.Text = activeCard.ToString();
        }

        private void DragDiscard(object sender, DragStartingEventArgs e)
        {
            if (discard.Cards.Count == 0)
            {
                return;
            }
            cardSource = "discard";
            activeCard = discard.TopCard;
            e.Data.Text = activeCard.ToString();
        }

        private async void DropDiscard(object sender, DropEventArgs e)
        {
            e.Handled = true;
            if (cardSource == "deck")
            {
                ClearActiveCard();
            }
            else if (cardSource == "hand")
            {
                if (hand.Cards.Count > 10)
                {
                    hand.Cards.Remove(activeCard);
                    discard.Cards.Add(activeCard);
                    DiscardImg.Source = $"card{activeCard.Index}.png";
                    ClearActiveCard();
                    RefreshHandImages();
                }
            }
            RefreshLabels();
        }

        private bool DragHand(int handIndex)
        {
            if (handIndex >= hand.Cards.Count) { return false; }
            cardSource = "hand";
            activeCard = hand.Cards[handIndex];
            activeHandIndex = handIndex;
            activeCardIndex = hand.Cards[handIndex].Index;
            return true;
        }

        private bool DropHand(int handIndex)
        {
            if (cardSource == "deck")
            {
                if (hand.Cards.Count > 10 || stock.Cards.Count <= 0)
                {
                    ClearActiveCard();
                    return false;
                }
                hand.Cards.Insert(handIndex, stock.Draw());
                ClearActiveCard();
                RefreshHandImages();
                RefreshLabels();
                if (stock.Cards.Count == 0)
                {
                    DeckImg.Source = "empty.png";
                }
                return true;
            }
            else if (cardSource == "discard")
            {
                if (hand.Cards.Count > 10 || discard.Cards.Count <= 0)
                {
                    ClearActiveCard();
                    return false;
                }
                hand.Cards.Insert(handIndex, discard.Draw());
                ClearActiveCard();
                RefreshHandImages();
                RefreshLabels();
                if (discard.Cards.Count == 0)
                {
                    DiscardImg.Source = "empty.png";
                }
                else
                {
                    DiscardImg.Source = $"card{discard.TopCard.Index}.png";
                }
                return true;
            }
            else if (cardSource == "hand")
            {
                int i = handIndex;
                if (i >= hand.Cards.Count)
                {
                    i = hand.Cards.Count - 1;
                }
                hand.Cards.Remove(activeCard);
                hand.Cards.Insert(i, activeCard);
                ClearActiveCard();
                RefreshHandImages();
                RefreshLabels();
                return true;
            }
            ClearActiveCard();
            return false;
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