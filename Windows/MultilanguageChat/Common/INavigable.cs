using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MultilanguageChat.Common
{
    public interface INavigable
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode);

        void OnNavigatingFrom(NavigatingCancelEventArgs e);

        void OnNavigatedFrom();
    }
}
