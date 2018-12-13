using System.IO;
using System.Threading.Tasks;

namespace MultilanguageChat.Services
{
    public interface IAudioService
    {
        Task<string> StartRecordingAsync();

        Task StopRecordingAsync();

        Task SpeakAsync(string text, string language);
    }
}