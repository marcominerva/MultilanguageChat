using MultilanguageChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultilanguageChat.Controls
{
    public class ChatMessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyMessage { get; set; }

        public DataTemplate OtherMessage { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
            => ((ChatMessage)item).IsMine ? MyMessage : OtherMessage;
    }
}
