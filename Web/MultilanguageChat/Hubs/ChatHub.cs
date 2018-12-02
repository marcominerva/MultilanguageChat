using Microsoft.AspNetCore.SignalR;
using MultilanguageChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultilanguageChat.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(ChatMessage message)
        {
            Clients.Others.SendAsync("SendMessage", message);
        }
    }
}
