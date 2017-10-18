using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatViewModel;
using ChatBusinessLogic;
using Moq;
using ChatBusinessObjects;
using System.Threading.Tasks;

namespace ChatViewModelTest
{
  [TestClass]
  public class MainWindowViewModelTest
  {
    private Mock<IClientChatCommunication> mockObject;
    private IClientChatCommunication chatCommunication;
    private MainWindowViewModel viewModel;

    [TestInitialize]
    public void Setup()
    {
      mockObject = new Mock<IClientChatCommunication>();
      chatCommunication = mockObject.Object;
      viewModel = new MainWindowViewModel(chatCommunication);
    }
    [TestMethod]
    public void TestSendMessageWhenConnected()
    {
      const string expectedUsername = "Toto";

      viewModel.ConnectCmd.Execute(null);
      viewModel.User.Name = expectedUsername;
      Assert.IsTrue(viewModel.IsConnected);
      viewModel.SendMessageCmd.Execute(null);
      Assert.IsTrue(viewModel.IsConnected);
      Assert.IsFalse(viewModel.IsSending);

      mockObject.Verify(m => m.Connect(It.IsAny<String>(), It.IsAny<String>()));
      mockObject.Verify(
        m => m.SendMessage(
          It.Is<ChatMessage>(
            msg => msg.Speaker == expectedUsername
            // ne pas faire le message est remis à "" à la fin de l'envoi
            // && msg.Content == expectedContent  
            )));
    }
    [TestMethod]
    public void TestSendMessageWhenNotConnected()
    {
      Assert.IsFalse(viewModel.IsConnected);
      viewModel.SendMessageCmd.Execute(null);
      Assert.IsFalse(viewModel.IsSending);

      mockObject.Verify(m => m.SendMessage(It.IsAny<ChatMessage>()));
    }
    [TestMethod]
    public void TestSendMessageWithException()
    {
      viewModel.ConnectCmd.Execute(null);
      Assert.IsTrue(viewModel.IsConnected);
      try
      {
        viewModel.SendMessageCmd.Execute(null);
        Assert.Fail("on ne devrait pas arriver là à cause d'une exception");
      }
      catch (Exception)
      {
        System.Diagnostics.Debug.WriteLine("Exception attendue, normal");
      }
      Assert.IsFalse(viewModel.IsSending);

      mockObject.Verify(m => m.Connect(It.IsAny<String>(), It.IsAny<String>()));
      mockObject.Verify(
        m => m.SendMessage(
          It.IsAny<ChatMessage>()));
    }
    [TestMethod]
    public void TestConnectionFailureShouldNotCauseException()
    {
      mockObject = new Mock<IClientChatCommunication>();
      mockObject.Setup(m => m.Connect(It.IsAny<String>(), It.IsAny<String>())).Throws(new Exception("Connection failed"));
      chatCommunication = mockObject.Object;
      viewModel = new MainWindowViewModel(chatCommunication);

      try
      {
        viewModel.ConnectCmd.Execute(null);
        viewModel.SendMessageCmd.Execute(null);
      }
      catch (Exception)
      {
        Assert.Fail("on ne devrait pas arriver là à cause d'une exception");
      }
      Assert.IsFalse(viewModel.IsConnected);

      mockObject.Verify(m => m.Connect(It.IsAny<String>(), It.IsAny<String>()));
    }
  }
}
