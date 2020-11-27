using ChatBusinessLogic;
using ChatBusinessObjects;
using Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ViewModel.Commands;

namespace ChatViewModel
{
    public class MainWindowViewModel : ReactiveObject//: BindableBase
    {
        private ObservableCollection<ChatMessage> messages = new ObservableCollection<ChatMessage>();
        private User user = new User();
        private ChatMessage messageToSend = new ChatMessage();
        private IChatCommunication chatCommunication = new NullChatCommunication();
        private bool isSending;
        private bool isConnecting;
        private bool isConnected;
        private CancellationTokenSource connectionCts;

        public ReactiveCommand<Unit, Unit> ConnectCmd { get; set; }
        public ReactiveCommand<Unit, Unit> DisconnectCmd { get; set; }
        public ReactiveCommand<Unit, Unit> SendMessageCmd { get; set; }
        public ReactiveCommand<Unit, Unit> CancelConnectCmd { get; set; }

        public void PasswordChanged(string password)
        {
            User.Password = password;
        }

        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                bool hasChanged = SetProperty(ref isConnecting, value);
                //if (hasChanged)
                //{
                //  OnPropertyChanged(nameof(AnyCommandRunning));
                //  ConnectCmd.FireExecuteChanged();
                //  DisconnectCmd.FireExecuteChanged();
                //  SendMessageCmd.FireExecuteChanged();
                //  CancelConnectCmd.FireExecuteChanged();
                //}
            }
        }
        public bool IsSending
        {
            get { return isSending; }
            set
            {
                //isSending = value;
                bool hasChanged = SetProperty(ref isSending, value);
                //if (hasChanged)
                //{
                //  OnPropertyChanged(nameof(AnyCommandRunning));
                //  ConnectCmd.FireExecuteChanged();
                //  DisconnectCmd.FireExecuteChanged();
                //  SendMessageCmd.FireExecuteChanged();
                //}
            }
        }
        private bool anyCommandRunning;
        public bool AnyCommandRunning
        {
            set
            {
                anyCommandRunning = value;
            }
            get
            {
                return anyCommandRunning;
            }
        }
        public ObservableCollection<ChatMessage> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
            }
        }

        public User User
        {
            get { return user; }
            set
            {
                user = value;
            }
        }

        public ChatMessage MessageToSend
        {
            get { return messageToSend; }
            set
            {
                messageToSend = value;
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
                    this.RaisePropertyChanged(nameof(IsDisconnected));
                }
            }
        }
        public bool IsDisconnected
        {
            get { return !isConnected; }
        }
        public MainWindowViewModel()
        {
            LoadDummyData();

      var canConnect = this.WhenAnyValue(
                          // les propriétés dont dépend la commande
                          // ReactiveUI s'abonne à ces propriétés pour émettre le signal de chgt d'état de la commande
                              x => x.AnyCommandRunning,
                              x => x.IsConnected,
                              x => x.User.Name,
                              x => x.User.Password,
                              (anyCommandRunning, isConnected, name, password) =>
                                !anyCommandRunning && !isConnected && !String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(password)
                          );

      ConnectCmd = ReactiveCommand.CreateFromTask(Connect, canConnect);
      var canDisconnect = this.WhenAnyValue(
                                    x => x.AnyCommandRunning,
                                    x => x.IsConnected,
                                    (anyCommandRunning, isConnected) => !anyCommandRunning && isConnected
                                );

      DisconnectCmd = ReactiveCommand.Create(Disconnect, canDisconnect);
      var canSendMessage = this.WhenAnyValue(
                                 x => x.AnyCommandRunning,
                                 x => x.IsConnected,
                                 x => x.MessageToSend.Content,
                                 (anyCommandRunning, isConnected, messageContent) =>
                                 !anyCommandRunning && isConnected && !String.IsNullOrWhiteSpace(messageContent)
                                );
      SendMessageCmd = ReactiveCommand.CreateFromTask(SendMessage, canSendMessage);
      var canCancelConnect = this.WhenAnyValue(
             x => x.IsConnecting,
             selector: isConnecting => isConnecting
            );
            CancelConnectCmd = ReactiveCommand.CreateFromTask(CancelConnect, canCancelConnect);

      // Ce qu'on écoute et ce qu'on produit en sortie de l'écoute
      IObservable<bool> observableAnyCommandRunning = this.WhenAnyValue
          (
              x => x.IsConnecting,
              x => x.IsSending,
              (isConnecting, isSending) => isSending || isConnecting
          );

      // Dans quelle propriété on écrit 
      ObservableAsPropertyHelper<bool> helper = observableAnyCommandRunning
        .ToProperty(this, x => x.AnyCommandRunning, initialValue:false,deferSubscription:false);

            this
                .WhenAny(x => x.IsConnected, observedChange => observedChange.Value)
                // Raise event that IsDisconnected has changed
                .Subscribe(v =>
                {
                    this.RaisePropertyChanging(nameof(IsDisconnected));     
                });
        }
        private async Task SendMessage()
        {
            IsSending = true;
            MessageToSend.EmissionDate = DateTime.Now;
            MessageToSend.Speaker = User.Name;
            await chatCommunication.SendMessage(MessageToSend);
            MessageToSend.Content = "";
            IsSending = false;
        }
        private async Task Connect()
        {
            IsConnecting = true;
            //Task task = Task.Delay(2000);
            //await task;
            connectionCts = new CancellationTokenSource();
            try
            {
                await chatCommunication.Connect(User.Name, User.Password, connectionCts.Token);
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
        private async void Disconnect()
        {
            IsConnecting = true;
            connectionCts = new CancellationTokenSource();
            try
            {
                await chatCommunication.Disconnect(connectionCts.Token);
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
        private Task CancelConnect()
        {
            connectionCts.Cancel(throwOnFirstException: false);
            return Task.FromResult(0);
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
        private bool SetProperty<T>(ref T target, T value, [CallerMemberName] string propertyName = null)
        {
            if (target.Equals(value))
                return false;
            target = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
