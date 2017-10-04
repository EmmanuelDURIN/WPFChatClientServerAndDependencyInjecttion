using ChatBusinessObjects;

namespace ChatViewModel
{
  internal class ChatMessageViewModel  : ChatMessage
  {
    public bool IsMyMessage { get; set; }

    public ChatMessageViewModel(ChatMessage msg, bool isMyMessage = false)
    {
      IsMyMessage = isMyMessage;
      this.Speaker = msg.Speaker;
      this.Content = msg.Content;
      this.EmissionDate = msg.EmissionDate;
    }
  }
}