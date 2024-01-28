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
    public partial class StartPage : ContentPage
    {
        private Game Game { get; set; }
        public StartPage()
        {
            InitializeComponent();
            Game = new Game();
        }

        async void OnStartGameButtonClicked(object sender, EventArgs e)
        {
            Game.Players[0].Name = Player1Name.Text;
            Game.Players[1].Name = Player2Name.Text;
            await Navigation.PushAsync(new GinRummyPage());
        }
    }
}