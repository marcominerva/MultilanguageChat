using Microsoft.AspNetCore.Mvc;
using MultilanguageChat.Models;
using System.Threading.Tasks;
using TranslatorService;

namespace MultilanguageChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslatorController : ControllerBase
    {
        private readonly ITranslatorClient translatorClient;

        public TranslatorController(ITranslatorClient translatorClient)
        {
            this.translatorClient = translatorClient;
        }

        [HttpPost]
        public async Task<ActionResult<TranslationMessage>> Translate(TranslationMessage message)
        {
            string translatedText = null;

            if (!string.IsNullOrWhiteSpace(message.DestinationLanguage) && message.SourceLanguage != message.DestinationLanguage)
            {
                // Translates the text.
                var response = await translatorClient.TranslateAsync(message.Text, message.SourceLanguage, message.DestinationLanguage);
                translatedText = response.Translation.Text;
            }
            else
            {
                // No destination language specified, use the original text.
                translatedText = message.Text;
            }

            var translationMessage = new TranslationMessage
            {
                Text = message.Text,
                SourceLanguage = message.SourceLanguage,
                TranslatedText = translatedText,
                DestinationLanguage = message.DestinationLanguage
            };

            return translationMessage;
        }
    }
}
