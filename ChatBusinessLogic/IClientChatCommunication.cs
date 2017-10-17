using System;
using ChatBusinessObjects;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ChatBusinessLogic
{
  // Même interface que IChatCommunication sauf que émission d'événements
  public interface IClientChatCommunication : IChatCommunication
  {
    Action<ChatMessage> MessageReceived { get; set; }
    event Action<StateChange> StateChanged;
  }
}