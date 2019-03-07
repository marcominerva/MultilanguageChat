using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MultilanguageChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorService;

namespace MultilanguageChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslatorController : ControllerBase
    {
        private readonly AppSettings settings;

        public TranslatorController(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        [HttpPost]
        public async Task<ActionResult<TranslationMessage>> Translate(TranslationMessage message)
        {
            string translatedText = null;

            if (!string.IsNullOrWhiteSpace(message.DestinationLanguage) &&  message.SourceLanguage != message.DestinationLanguage)
            {
                // Translates the text.
                var translatorService = new TranslatorClient(settings.TranslatorSubscriptionKey);
                var response = await translatorService.TranslateAsync(message.Text, message.SourceLanguage, message.DestinationLanguage);
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
