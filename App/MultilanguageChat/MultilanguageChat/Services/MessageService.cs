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
        private readonly HubConnection connection;
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
            connection = new HubConnectionBuilder()
                .WithUrl(serverUrl)
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<ChatMessage>(SEND_MESSAGE, (message) => messagedReceivedAction?.Invoke(message));
        }

        public Task ConnectAsync() => connection.StartAsync();

        public Task DisconnectAsync() => connection.StopAsync();

        public Task SendMessageAsync(ChatMessage message) => connection.InvokeAsync(SEND_MESSAGE, message);
    }
}
