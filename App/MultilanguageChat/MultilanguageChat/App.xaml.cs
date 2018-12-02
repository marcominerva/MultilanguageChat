using MultilanguageChat.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MultilanguageChat
{
    public partial class App : Application
    {
        public static bool IsPausing { get; set; }

        public App()
        {
            InitializeComponent();

            var startPage = new MainPage();
            MainPage = new NavigationPage(startPage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
