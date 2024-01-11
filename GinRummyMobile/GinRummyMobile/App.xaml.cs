using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GinRummyMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new GinRummyPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
