using System;
using ChatBusinessObjects;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ChatBusinessLogic
{
  // Définit ce que le serveur peut dire au client
  public interface IClientChatCommunication 
  {
    Task BroadcastMessage(ChatMessage message);
    void UserConnected(string userName);
    void UserDisconnected(string userName);
  }
}