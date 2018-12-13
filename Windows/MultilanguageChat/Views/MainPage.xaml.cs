using System;
using MultilanguageChat.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MultilanguageChat.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
