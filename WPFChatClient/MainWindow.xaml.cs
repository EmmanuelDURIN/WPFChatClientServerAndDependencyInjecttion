using Autofac;
using ChatViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfChatClient;

namespace WPFChatClient
{
  /// <summary>
  /// Logique d'interaction pour MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private MainWindowViewModel viewModel;

    public MainWindow()
    {
      InitializeComponent();
      // on n'instancie plus le ViewModel nous même
      // C'est le Fx de DI qui s'en charge
      // C'est lui qui injecte les dépendances
      // Donc quand on voudra exécuter dans un contexte d'exécution différent (ex : test )
      // on pourra avoir d'autres dépendances ( ex : remplacer la vraie classe de communication par un stub(bouchon)/mock )
      viewModel = WpfContainer.CreateContainer().Resolve<MainWindowViewModel>();
      DataContext = viewModel;
    }

    private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
      viewModel.PasswordChanged(passwordBox.Password);
    }
  }
}
