using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultilanguageChat.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; }

        public string Text { get; set; }

        public string Language { get; set; }
    }
}