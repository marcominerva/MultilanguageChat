using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MultilanguageChat.Services
{
    public class AudioService : IAudioService
    {
        private readonly AudioRecorderService recorder;
        private IEnumerable<Locale> locales;

        public AudioService()
        {
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true,
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(15)
            };
        }

        public async Task<string> StartRecordingAsync()
        {
            if (!recorder.IsRecording)
            {
                // Start recording audio.
                var audioRecordTask = await recorder.StartRecording();
                var audioFile = await audioRecordTask;
                return audioFile;
            }

            return null;
        }

        public Task StopRecordingAsync() => recorder.StopRecording();

        public async Task SpeakAsync(string text, string language)
        {
            if (locales == null)
            {
                locales = await TextToSpeech.GetLocalesAsync();
            }

            var settings = new SpeechOptions()
            {
                Volume = 1,
                Pitch = 1,
                Locale = locales.FirstOrDefault(l => l.Language == language)
            };

            await TextToSpeech.SpeakAsync(text, settings);
        }
    }
}
