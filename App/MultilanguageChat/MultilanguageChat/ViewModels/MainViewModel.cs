using MultilanguageChat.Common;
using MultilanguageChat.Models;
using MultilanguageChat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
        private readonly ISpeechClient speechClient;
        private readonly IMessageService messageService;
        private readonly IAudioService audioService;

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

        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => Set(ref isRecording, value);
        }

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public AutoRelayCommand SendMessageCommand { get; private set; }

        public AutoRelayCommand StartRecordingCommand { get; private set; }

        public AutoRelayCommand StopRecordingCommand { get; private set; }

        public MainViewModel(ITranslatorClient translatorClient, ISpeechClient speechClient, IMessageService messageService, IAudioService audioService)
        {
            this.translatorClient = translatorClient;
            this.speechClient = speechClient;
            this.audioService = audioService;

            this.messageService = messageService;
            this.messageService.OnMessageReceived(OnMessageReceived);

            UserName = DeviceInfo.Name;

            CreateCommands();
        }

        private void CreateCommands()
        {
            SendMessageCommand = new AutoRelayCommand(async () => await SendMessageAsync(),
                () => !IsBusy && !string.IsNullOrWhiteSpace(message) && !string.IsNullOrWhiteSpace(userName))
                .DependsOn(nameof(IsBusy)).DependsOn(nameof(Message)).DependsOn(nameof(UserName));

            StartRecordingCommand = new AutoRelayCommand(async () => await StartRecordingAsync());
            StopRecordingCommand = new AutoRelayCommand(async () => await StopRecordingAsync());
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

                // Sends the message using SignalR.
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
                // Translates the message to the user language with Translator Cognitive Service.
                var translationResponse = await translatorClient.TranslateAsync(message.Text, message.Language, selectedLanguage.Code);
                message.Text = translationResponse.Translation.Text;

                Device.BeginInvokeOnMainThread(() => Messages.Add(message));
                await audioService.SpeakAsync($"{message.Sender}. {message.Text}", selectedLanguage.Code);
            }
            catch (Exception ex)
            {
                await ShowErrorAsync("Error while translating message", ex);
            }
        }

        private async Task StartRecordingAsync()
        {
            string audioFile = null;
            IsRecording = true;

            try
            {
                audioFile = await audioService.StartRecordingAsync();
            }
            catch (Exception ex)
            {
                IsRecording = false;
                await ShowErrorAsync("Error while start recording", ex);
            }

            if (audioFile != null)
            {
                IsBusy = true;

                try
                {
                    using (var file = File.OpenRead(audioFile))
                    {
                        // Tries to recognize speech using the Speech Cognitive Service.
                        var cultureInfo = CultureInfo.CreateSpecificCulture(selectedLanguage.Code);
                        var result = await speechClient.RecognizeAsync(file, cultureInfo.IetfLanguageTag);

                        if (result.RecognitionStatus == RecognitionStatus.Success)
                        {
                            // Speech has been recognized, sends it.
                            Message = result.DisplayText;
                            await SendMessageAsync();
                        }
                        else
                        {
                            await DialogService.AlertAsync("Unable to recognize speech");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await ShowErrorAsync("Error while recognizing speech", ex);
                }
                finally
                {
                    IsBusy = false;
                    IsRecording = false;
                }
            }
            else
            {
                IsRecording = false;
            }
        }

        private Task StopRecordingAsync() => audioService.StopRecordingAsync();

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
