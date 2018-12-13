using System;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using MultilanguageChat.Common;
using MultilanguageChat.Services;
using MultilanguageChat.Views;
using TranslatorService;

namespace MultilanguageChat.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        static ViewModelLocator()
        {
            CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<IUserDialogs>(() => UserDialogs.Instance);

            SimpleIoc.Default.Register<IAudioService, AudioService>();

            SimpleIoc.Default.Register<ITranslatorClient>(() =>
            {
                var client = new TranslatorClient(Constants.TranslatorSubscriptionKey);
                return client;
            });

            SimpleIoc.Default.Register<ISpeechClient>(() =>
            {
                var client = new SpeechClient(Constants.SpeechRegion, Constants.SpeechSubscriptionKey);
                return client;
            });

            SimpleIoc.Default.Register<IMessageService>(() =>
            {
                var service = new MessageService(Constants.ServerUrl);
                return service;
            });

            Register<MainViewModel, MainPage>();
        }

        public MainViewModel MainViewModel => CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();

        public NavigationServiceEx NavigationService => CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public static void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();
            CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>().Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
