using Microsoft.AspNetCore.SignalR.Client;
using MultilanguageChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultilanguageChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly HubConnection hub;
        private const string SEND_MESSAGE = "SendMessage";

        private Action<ChatMessage> messagedReceivedAction;
        public IMessageService OnMessageReceivedAction(Action<ChatMessage> action)
        {
            messagedReceivedAction = action;
            return this;
        }

        public MessageService(string serverUrl)
        {
            // Creates the hub for the connection via SignalR.
            hub = new HubConnectionBuilder()
                .WithUrl(serverUrl)
                .Build();

            hub.On<ChatMessage>(SEND_MESSAGE, (message) => messagedReceivedAction?.Invoke(message));
        }

        public Task ConnectAsync() => hub.StartAsync();

        public Task DisconnectAsync() => hub.StopAsync();

        public Task SendMessageAsync(ChatMessage message) => hub.InvokeAsync(SEND_MESSAGE, message);
    }
}
