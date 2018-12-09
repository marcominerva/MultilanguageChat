using MultilanguageChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MultilanguageChat.Controls
{
    public class ChatMessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyMessage { get; set; }

        public DataTemplate OtherMessage { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            => ((ChatMessage)item).IsMine ? MyMessage : OtherMessage;
    }
}
