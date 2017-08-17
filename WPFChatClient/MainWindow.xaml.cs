using ChatViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
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

namespace WPFChatClient
{
  public partial class MainWindow : Window
  {
    [Import(nameof(MainWindowViewModel))]
  //  [Import("MainWindowViewModel")]
    private MainWindowViewModel viewModel = null;

    public MainWindow()
    {
      InitializeComponent();
      CompositionContainer container = WPFContainer.Instance;
      //Lazy<MainWindowViewModel>  lazyVM = container.GetExport<MainWindowViewModel>(nameof(MainWindowViewModel));
      //viewModel = lazyVM.Value;
      //viewModel = new MainWindowViewModel();
      container.ComposeParts(this);
      DataContext = viewModel;
    }
    private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
      viewModel.PasswordChanged(passwordBox.Password);
    }
  }
}
