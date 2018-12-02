using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultilanguageChat.Models
{
    public class TranslationMessage
    {
        public string Text { get; set; }

        public string SourceLanguage { get; set; }

        public string DestinationLanguage { get; set; }

        public string TranslatedText { get; set; }
    }
}
