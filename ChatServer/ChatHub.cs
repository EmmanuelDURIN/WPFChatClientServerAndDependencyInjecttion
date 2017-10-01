using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ChatBusinessLogic;
using ChatBusinessObjects;
using System.Threading.Tasks;

namespace ChatServer
{
  public class ChatHub : Hub<IChatCommunication>, IChatCommunication
  {
    public event Action<ChatMessage> MessageReceived;

    public Task Connect(string userName, string password)
    {
      // TODO autre exercice
      return Task.FromResult(0);
    }

    public Task Disconnect()
    {
      // TODO autre exercice
      return Task.FromResult(0);
    }
    public Task SendMessage(ChatMessage message)
    {
      Clients.All.SendMessage(message);
      // Pour appeler tous sauf l'appelant :
      //Clients.Others.SendMessage(message);
      return Task.FromResult(0);
    }
  }
}