using System;
using ChatBusinessObjects;

namespace ChatBusinessLogic
{
  public interface IChatCommunication
  {
    event Action<ChatMessage> MessageReceived;

    void Connect(string userName, string password);
    void Disconnect();
    void SendMessage(ChatMessage message);
  }
}