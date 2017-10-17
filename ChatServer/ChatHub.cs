using ChatBusinessLogic;
using ChatBusinessObjects;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ChatServer
{
  public class ChatHub : Hub<IClientChatCommunication>, IChatCommunication
  {
    public Task Connect(string userName, string password)
    {
      Clients.Others.UserConnected(userName);

      return Task.FromResult(0);
    }
    [Authorize]
    public Task Disconnect()
    {
      Clients.Others.UserDisconnected(this.Context.User.Identity.Name);
      System.Diagnostics.Debug.WriteLine($"Disconnect Caller is {this.Context.User.Identity.Name}");
      return Task.FromResult(0);
    }
    [Authorize]
    public Task SendMessage(ChatMessage message)
    {
      System.Diagnostics.Debug.WriteLine($"SendMessage Caller is {this.Context.User.Identity.Name}");
      //Clients.All.SendMessage(message);
      // Pour appeler tous sauf l'appelant :
      Clients.Others.BroadcastMessage(message);
      return Task.FromResult(0);
    }
    [Authorize]
    public Task<List<User>> GetConnectedUsers()
    {
      return Task.FromResult<List<User>>(
      Enumerable.Range(1, 4)
        .Select(i =>
         new User { Name = "User" + i, Password = "Password" + i }
      )
      .ToList());
    }
  }
}