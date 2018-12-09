using MultilanguageChat.Common;
using MultilanguageChat.Models;
using MultilanguageChat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorService;
using TranslatorService.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MultilanguageChat.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ITranslatorClient translatorClient;
        private readonly IMessageService messageService;

        private string userName;
        public string UserName
        {
            get => userName;
            set => Set(ref userName, value, broadcast: true);
        }

        private IEnumerable<ServiceLanguage> languages;
        public IEnumerable<ServiceLanguage> Languages
        {
            get => languages;
            set => Set(ref languages, value);
        }

        private ServiceLanguage selectedLanguage;
        public ServiceLanguage SelectedLanguage
        {
            get => selectedLanguage;
            set => Set(ref selectedLanguage, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => Set(ref message, value, broadcast: true);
        }

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public AutoRelayCommand SendMessageCommand { get; private set; }

        public MainViewModel(ITranslatorClient translatorClient, IMessageService messageService)
        {
            this.translatorClient = translatorClient;

            this.messageService = messageService;
            this.messageService.OnMessageReceivedAction(OnMessageReceived);

            UserName = DeviceInfo.Name;

            CreateCommands();
        }

        private void CreateCommands()
        {
            SendMessageCommand = new AutoRelayCommand(async () => await SendMessageAsync(),
                () => !IsBusy && !string.IsNullOrWhiteSpace(message) && !string.IsNullOrWhiteSpace(userName))
                .DependsOn(nameof(IsBusy)).DependsOn(nameof(Message)).DependsOn(nameof(UserName));
        }

        private async Task SendMessageAsync()
        {
            try
            {
                var chatMessage = new ChatMessage
                {
                    Sender = userName,
                    Language = SelectedLanguage.Code,
                    Text = message,
                    IsMine = true
                };

                Messages.Add(chatMessage);
                await messageService.SendMessageAsync(chatMessage);

                Message = null;
            }
            catch (Exception ex)
            {
                await ShowErrorAsync("Error while sending message", ex);
            }
        }

        private async void OnMessageReceived(ChatMessage message)
        {
            try
            {
                // Translates the message in the user language.
                var translationResponse = await translatorClient.TranslateAsync(message.Text, message.Language, selectedLanguage.Code);
                message.Text = translationResponse.Translation.Text;

                Device.BeginInvokeOnMainThread(() => Messages.Add(message));
            }
            catch (Exception ex)
            {
                await ShowErrorAsync("Error while translating message", ex);
            }
        }

        public override async void Activate(object parameter)
        {
            IsBusy = true;

            try
            {
                Languages = await translatorClient.GetLanguagesAsync();
                SelectedLanguage = Languages.FirstOrDefault(l => l.Code == new CultureInfo(translatorClient.Language).TwoLetterISOLanguageName) ?? Languages.FirstOrDefault();

                await messageService.ConnectAsync();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync("Error while initializing", ex);
            }
            finally
            {
                IsBusy = false;
            }

            base.Activate(parameter);
        }
    }
}
