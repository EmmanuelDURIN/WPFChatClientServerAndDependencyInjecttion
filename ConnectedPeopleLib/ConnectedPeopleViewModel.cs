using ChatBusinessLogic;
using ChatBusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ConnectedPeopleLib
{
  public class ConnectedPeopleViewModel : BindableBase
  {
    private TaskScheduler uiTaskScheduler;

    private ObservableCollection<User> connectedPeople;
    public ObservableCollection<User> ConnectedPeople
    {
      get { return connectedPeople; }
      set
      {
        SetProperty(ref connectedPeople, value);
      }
    }
    private INotifyingClientChatCommunication chatCommunication;
    public ConnectedPeopleViewModel(INotifyingClientChatCommunication chatCommunication)
    {
      this.chatCommunication = chatCommunication;
      if ( chatCommunication != null) { 
        chatCommunication.UserConnected += ChatCommunicationUserConnected;
        chatCommunication.UserDisconnected += ChatCommunicationUserDisconnected;
      }
      try
      {
        uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      }
      catch (Exception)
      {
        System.Diagnostics.Debug.WriteLine("Couldn't get uiTaskScheduler");
      }
    }
    private void ChatCommunicationUserDisconnected(string userName)
    {
        Task.Factory.StartNew
        (
          state =>
          {
            var user = connectedPeople.FirstOrDefault(u => u.Name == userName);
            if (user != null)
            {
              connectedPeople.Remove(user);
            }
          },
          state: null,
          cancellationToken: CancellationToken.None,
          creationOptions: TaskCreationOptions.None,
          scheduler: uiTaskScheduler
         );
    }
    private void ChatCommunicationUserConnected(string userName)
    {
      Task.Factory.StartNew
       (
         state =>
         {
           connectedPeople.Add(new User { Name = userName });
         },
         state: null,
         cancellationToken: CancellationToken.None,
         creationOptions: TaskCreationOptions.None,
         scheduler: uiTaskScheduler
        );
    }
    public void Init()
    {
      chatCommunication.StateChanged += this.ChatCommunicationStateChanged;
    }
    private async void ChatCommunicationStateChanged(Microsoft.AspNet.SignalR.Client.StateChange stateChange)
    {
      if (stateChange.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
      {
        try
        {
          var users = await chatCommunication.GetConnectedUsers();
          ConnectedPeople = new ObservableCollection<User>(users);
        }
        catch (Exception)
        {
          System.Diagnostics.Debug.WriteLine("Exception during getting connected people");
        }
      }
    }
    internal void Close()
    {
      chatCommunication.StateChanged -= this.ChatCommunicationStateChanged;
    }
  }
}