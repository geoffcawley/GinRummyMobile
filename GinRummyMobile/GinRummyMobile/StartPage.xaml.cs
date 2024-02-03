using GinRummyMobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GinRummyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        private Game Game { get; set; }
        private GameSettings Settings { get; set; }

        private string fileName { get; set; }

        public StartPage()
        {
            InitializeComponent();
            Game = new Game();
            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ginrummysettings.txt");
            try
            {
                string settingsstr = File.ReadAllText(fileName);
                Settings = JsonSerializer.Deserialize<GameSettings>(settingsstr);
            }
            catch
            {
                Settings = new GameSettings();
                File.WriteAllText(fileName, JsonSerializer.Serialize(Settings));
            }
            Player1Name.Text = Settings.Player1Name;
            Player2Name.Text = Settings.Player2Name;
        }

        async void OnStartGameButtonClicked(object sender, EventArgs e)
        {
            Settings.Player1Name = Player1Name.Text;
            Settings.Player2Name = Player2Name.Text;
            File.WriteAllText(fileName, JsonSerializer.Serialize(Settings));
            Game.Players[0].Name = Player1Name.Text;
            Game.Players[1].Name = Player2Name.Text;
            await Navigation.PushAsync(new GinRummyPage());
        }
    }
}