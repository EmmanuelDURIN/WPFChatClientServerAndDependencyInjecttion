using System;
using ChatBusinessObjects;
using System.Threading.Tasks;

namespace ChatBusinessLogic
{
  public interface IChatCommunication
  {
    event Action<ChatMessage> MessageReceived;

    Task Connect(string userName, string password);
    Task Disconnect();
    Task SendMessage(ChatMessage message);
  }
}