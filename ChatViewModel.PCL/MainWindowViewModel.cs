using ChatBusinessLogic;
using ChatBusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Commands;

namespace ChatViewModel
{
  public class MainWindowViewModel : BindableBase, IPartImportsSatisfiedNotification
  {
    private ObservableCollection<ChatMessage> messages = new ObservableCollection<ChatMessage>();
    private User user = new User();
    private ChatMessage messageToSend = new ChatMessage();
    private IChatCommunication chatCommunication = null;
    private bool isSending;
    private bool isConnecting;
    private bool isConnected;

    public RelayCommand ConnectCmd { get; set; }
    public RelayCommand DisconnectCmd { get; set; }
    public RelayCommand SendMessageCmd { get; set; }

    public void PasswordChanged(string password)
    {
      User.Password = password;
      ConnectCmd.FireExecuteChanged();
    }

    public bool IsConnecting
    {
      get { return isConnecting; }
      set
      {
        bool hasChanged = SetProperty(ref isConnecting, value);
        if (hasChanged) { 
          OnPropertyChanged(nameof(AnyCommandRunning));
          ConnectCmd.FireExecuteChanged();
          DisconnectCmd.FireExecuteChanged();
          SendMessageCmd.FireExecuteChanged();
        }
      }
    }

    public bool IsSending
    {
      get { return isSending; }
      set
      {
        bool hasChanged = SetProperty(ref isSending, value);
        if (hasChanged)
        {
          OnPropertyChanged(nameof(AnyCommandRunning));
          ConnectCmd.FireExecuteChanged();
          DisconnectCmd.FireExecuteChanged();
          SendMessageCmd.FireExecuteChanged();
        }
      }
    }
    private bool AnyCommandRunning
    {
      get {
        return isSending || isConnecting;
      }
    }
    public ObservableCollection<ChatMessage> Messages
    {
      get { return messages; }
      set
      {
        SetProperty(ref messages, value);
      }
    }

    public User User
    {
      get { return user; }
      set
      {
        SetProperty(ref user, value);
      }
    }

    public ChatMessage MessageToSend
    {
      get { return messageToSend; }
      set
      {
        SetProperty(ref messageToSend, value);
      }
    }
    public bool IsConnected
    {
      get { return isConnected; }
      set
      {
        bool hasChanged = SetProperty(ref isConnected, value);
        if (hasChanged)
        {
          ConnectCmd.FireExecuteChanged();
          DisconnectCmd.FireExecuteChanged();
          OnPropertyChanged(nameof(IsDisconnected));
        }
      }
    }
    public bool IsDisconnected
    {
      get { return !isConnected; }
    }

    public IChatCommunication ChatCommunication { get => chatCommunication; set => chatCommunication = value; }

    public MainWindowViewModel()
    {
      LoadDummyData();

      ConnectCmd = new RelayCommand(execute: Connect, canExecute: o => !AnyCommandRunning && ! IsConnected && !String.IsNullOrWhiteSpace(User.Name) && !String.IsNullOrWhiteSpace(User.Password)); 
      DisconnectCmd = new RelayCommand(execute: Disconnect, canExecute: o => !AnyCommandRunning && IsConnected);
      SendMessageCmd = new RelayCommand(execute: SendMessage, canExecute: o => !AnyCommandRunning && !String.IsNullOrWhiteSpace(MessageToSend.Content));

      User.PropertyChanged += (o, args) => 
        {
            if ( args.PropertyName == nameof(User.Name) || args.PropertyName == nameof(User.Password))
              ConnectCmd.FireExecuteChanged();
        };
      MessageToSend.PropertyChanged += (o, args) =>
      {
          if (args.PropertyName == nameof(ChatMessage.Content) )
          SendMessageCmd.FireExecuteChanged();
      };
    }
    private async void SendMessage(object obj)
    {
      IsSending = true;
      MessageToSend.EmissionDate = DateTime.Now;
      MessageToSend.Speaker = User.Name;
      await chatCommunication.SendMessage(MessageToSend);
      MessageToSend.Content = "";
      IsSending = false;
    }
    private async void Connect(object obj)
    {
      IsConnecting = true;
      await chatCommunication.Connect(User.Name, User.Password);
      IsConnected = true;
      IsConnecting = false;
    }
    private async void Disconnect(object obj)
    {
      IsConnecting = true;
      await chatCommunication.Disconnect();
      IsConnected = false;
      IsConnecting = false;
    }	
    private void LoadDummyData()
    {
      var newMessages = Enumerable.Range(1, 10)
        .Select(i => new ChatMessage
        {
          Speaker = "Emmanuel",
          Content = "Hello" + i,
          EmissionDate = DateTime.Now.AddDays(-10).AddMinutes(i)
        }
        );
      foreach (var message in newMessages)
      {
        messages.Add(message);
      }
    }

    public void OnImportsSatisfied()
    {
    }
  }
}
