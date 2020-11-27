using System;
using System.Threading;
using ChatBusinessLogic;
using ChatViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestViewModels
{
  [TestClass]
  public class UnitTestMainWindowViewModel
  {


    [TestMethod]
    public void TestIsConnectedAfterConnect()
    {

      // 1 Arrange : mise en place du Mock
      Mock<IChatCommunication> mockObject = new Mock<IChatCommunication>();
      IChatCommunication chatCommunication = mockObject.Object;
      MainWindowViewModel viewModel = new MainWindowViewModel(chatCommunication);

      var expectedName = "toto";
      var expectedPassword = "1234";
      viewModel.User.Name = expectedName;
      viewModel.User.Password = expectedPassword;
      // 2 Act : Scénario
      viewModel.ConnectCmd.Execute(null);

      // 3 : Assert
      // Avec le mock on vérifie que l'objet englobant (MainWindowViewModel)
      // utilise bien le ChatCommunication 
      Assert.IsTrue(viewModel.IsConnected);
      mockObject.Verify(
        vm => vm.Connect(
          It.Is<String>( n => n == expectedName),
          It.Is<String>(n => n == expectedPassword),
          It.IsAny<CancellationToken>()
          )
        );
    }
  }
}
