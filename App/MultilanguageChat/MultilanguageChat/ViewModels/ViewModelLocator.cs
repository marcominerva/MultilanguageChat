using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using MultilanguageChat.Common;
using MultilanguageChat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorService;

namespace MultilanguageChat.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.Default.Register<IUserDialogs>(() => UserDialogs.Instance);
            SimpleIoc.Default.Register<IAudioService, AudioService>();

            SimpleIoc.Default.Register<ITranslatorClient>(() =>
            {
                var client = new TranslatorClient(Constants.TranslatorSubscriptionKey);
                return client;
            });

            SimpleIoc.Default.Register<ISpeechClient>(()=>
            {
                var client = new SpeechClient(Constants.SpeechRegion, Constants.SpeechSubscriptionKey);
                return client;
            });

            SimpleIoc.Default.Register<IMessageService>(() =>
            {
                var service = new MessageService(Constants.ServerUrl);
                return service;
            });

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}
