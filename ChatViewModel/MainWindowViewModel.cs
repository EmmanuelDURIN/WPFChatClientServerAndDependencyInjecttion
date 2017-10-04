using ChatBusinessLogic;
using ChatBusinessObjects;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModel.Commands;

namespace ChatViewModel
{
  public class MainWindowViewModel : BindableBase
  {
    private User user = new User();
    private ChatMessage messageToSend = new ChatMessage();
    private IClientChatCommunication chatCommunication = null;
    private bool isSending;
    private bool isConnecting;
    private bool isConnected;
    private ObservableCollection<ChatMessage> messages = new ObservableCollection<ChatMessage>();
    private TaskScheduler uiTaskScheduler;
    public ILog Logger { get; set; }
   
    public RelayCommand ConnectCmd { get; set; }
    public RelayCommand DisconnectCmd { get; set; }
    public RelayCommand SendMessageCmd { get; set; }

    public MainWindowViewModel(IClientChatCommunication chatCommunication)
    {
      uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      // Code préexistant
      this.ChatCommunication = chatCommunication;

      ConnectCmd = new RelayCommand(execute: Connect, canExecute: o => !AnyCommandRunning && !IsConnected && !String.IsNullOrWhiteSpace(User.Name) && !String.IsNullOrWhiteSpace(User.Password));
      DisconnectCmd = new RelayCommand(execute: Disconnect, canExecute: o => !AnyCommandRunning && IsConnected);
      SendMessageCmd = new RelayCommand(execute: SendMessage, canExecute: o => !AnyCommandRunning && !String.IsNullOrWhiteSpace(MessageToSend.Content) && IsConnected );
     
      User.Name = "X";
      User.Password = "Y";
      User.PropertyChanged += (o, args) =>
      {
        if (args.PropertyName == nameof(User.Name) || args.PropertyName == nameof(User.Password))
          ConnectCmd.FireExecuteChanged();
      };
      MessageToSend.PropertyChanged += (o, args) =>
      {
        if (args.PropertyName == nameof(ChatMessage.Content))
          SendMessageCmd.FireExecuteChanged();
      };
    }
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
        if (hasChanged)
        {
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
      get
      {
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
          SendMessageCmd.FireExecuteChanged();
          OnPropertyChanged(nameof(IsDisconnected));
        }
      }
    }
    public bool IsDisconnected
    {
      get { return !isConnected; }
    }

    public IClientChatCommunication ChatCommunication
    {
      get => chatCommunication;
      set
      {
        chatCommunication = value;
        // Abonnement  à l'événement MessageReceived 
        // et retour sur le thread graphique en cas de réception de message
        chatCommunication.MessageReceived = msg =>
        {
          Task.Factory.StartNew( state => {
            messages.Insert(0, msg);
          },
          state : null, 
          cancellationToken : CancellationToken.None, 
          creationOptions : TaskCreationOptions.None, 
          scheduler:uiTaskScheduler);
        };
      }
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
      try
      {
        await chatCommunication.Connect(User.Name, User.Password);
        IsConnected = true;
      }
      catch (TaskCanceledException)
      {
        System.Diagnostics.Debug.WriteLine("Cancellation");
      }
      finally
      {
        IsConnecting = false;
      }
    }
    private async void Disconnect(object obj)
    {
      IsConnecting = true;
      try
      {
        await chatCommunication.Disconnect();
		Logger.Info("User disconnected");
		IsConnected = false;
      }
      catch (TaskCanceledException)
      {
        System.Diagnostics.Debug.WriteLine("Cancellation");
      }
      finally
      {
        IsConnecting = false;
      }
    }
  }
}
