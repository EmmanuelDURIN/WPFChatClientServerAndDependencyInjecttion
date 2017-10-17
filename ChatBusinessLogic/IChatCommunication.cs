using System;
using ChatBusinessObjects;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ChatBusinessLogic
{
  public interface IChatCommunication
  {
    Task Connect(string userName, string password);
    Task Disconnect();
    Task SendMessage(ChatMessage message);
    Task<List<User>> GetConnectedUsers();
  }
}