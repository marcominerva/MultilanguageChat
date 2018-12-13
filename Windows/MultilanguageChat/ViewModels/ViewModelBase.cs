using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using MultilanguageChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MultilanguageChat.ViewModels
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, INavigable
    {
        protected IUserDialogs DialogService { get; }

        public ViewModelBase()
        {
            DialogService = SimpleIoc.Default.GetInstance<IUserDialogs>();
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetBusy(value) && !isBusy)
                {
                    BusyMessage = null;
                }
            }
        }

        private string busyMessage;
        public string BusyMessage
        {
            get => busyMessage;
            set => Set(ref busyMessage, value, broadcast: true);
        }

        public bool SetBusy(bool value, string message = null)
        {
            BusyMessage = message;

            var isSet = Set(() => IsBusy, ref isBusy, value, broadcast: true);
            if (isSet)
            {
                OnIsBusyChanged();
            }

            return isSet;
        }

        protected virtual void OnIsBusyChanged()
        {
        }

        public virtual void OnNavigatedFrom()
        {
        }

        public virtual Task OnNavigatedToAsync(object parameter, NavigationMode mode) => Task.CompletedTask;

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        protected Task ShowErrorAsync(string message, Exception error = null) => ShowErrorAsync(message, null, error);

        protected async Task ShowErrorAsync(string message, string title, Exception error = null)
        {
            DialogService.HideLoading();

            var alert = message;
            if (error != null && error.Message != message)
            {
                alert += $" - ({error.Message})";
            }

            await DialogService.AlertAsync(alert, title);
        }
    }
}
