using System;
using ChatBusinessObjects;
using System.Threading.Tasks;
using System.Threading;

namespace ChatBusinessLogic
{
  public interface IChatCommunication
  {
    event Action<ChatMessage> MessageReceived;

    Task Connect(string userName, string password, CancellationToken token );
    Task Disconnect(CancellationToken token);
    Task SendMessage(ChatMessage message);
  }
}