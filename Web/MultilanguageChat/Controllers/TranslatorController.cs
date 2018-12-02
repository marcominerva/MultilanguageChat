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

        [HttpGet]
        public async Task<ActionResult<TranslationMessage>> Translate(string text, [FromQuery(Name = "src")] string sourceLanguage, [FromQuery(Name = "dst")] string destinationLanguage)
        {
            string translatedText = null;

            if (!string.IsNullOrWhiteSpace(destinationLanguage))
            {
                // Translates the text.
                var translatorService = new TranslatorClient(settings.TranslatorSubscriptionKey);
                var response = await translatorService.TranslateAsync(text, sourceLanguage, destinationLanguage);
                translatedText = response.Translation.Text;
            }
            else
            {
                // No destination language specified, use the original message.
                translatedText = text;
            }

            var translationMessage = new TranslationMessage
            {
                Text = text,
                SourceLanguage = sourceLanguage,
                TranslatedText = translatedText,
                DestinationLanguage = destinationLanguage
            };

            return translationMessage;
        }
    }
}
