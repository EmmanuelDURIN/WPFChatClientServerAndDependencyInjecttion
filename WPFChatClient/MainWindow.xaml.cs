using Autofac;
using ChatViewModel;
using System.Windows;

namespace WPFChatClient
{
  public partial class MainWindow : Window
  {
    private MainWindowViewModel viewModel = null;
    public MainWindowViewModel ViewModel { get => viewModel; set => viewModel = value; }

    public MainWindow()
    {
      InitializeComponent();
      IContainer container = WPFContainer.CreateContainer();
      viewModel = container.Resolve<MainWindowViewModel>();
      DataContext = viewModel;
    }
    private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
      viewModel.PasswordChanged(passwordBox.Password);
    }
  }
}

