using MultilanguageChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultilanguageChat.Services
{
    public interface IMessageService
    {
        Task ConnectAsync();

        Task DisconnectAsync();

        Task SendMessageAsync(ChatMessage message);

        IMessageService OnMessageReceivedAction(Action<ChatMessage> action);
    }
}
