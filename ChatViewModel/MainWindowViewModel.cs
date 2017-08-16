using ChatBusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatViewModel
{
  public class MainWindowViewModel : BindableBase
  {
    private ObservableCollection<ChatMessage> messages = new ObservableCollection<ChatMessage>();
    private User user;
    private ChatMessage messageToSend;

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
    public MainWindowViewModel()
    {
      LoadDummyData();
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
  }
}
