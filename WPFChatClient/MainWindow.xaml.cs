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
  public partial class MainWindow : Window, IPartImportsSatisfiedNotification
  {
    private MainWindowViewModel viewModel = null;
    public MainWindowViewModel ViewModel { get => viewModel; set => viewModel = value; }

    public MainWindow()
    {
      InitializeComponent();
      CompositionContainer container = WPFContainer.Instance;
      Lazy<MainWindowViewModel> lazyVM = container.GetExport<MainWindowViewModel>();
      viewModel = lazyVM.Value;

      // ComposeParts et SatisfyImportsOnce ne marchent pas sans [Import] sur MainWindowViewModel
      // sauf si on instancie laisse MEF instancier la Window :
      //CompositionContainer container = WPFContainer.Instance;
      //Lazy<MainWindow> lazyWindow = container.GetExport<MainWindow>();
      //var mainWindow = lazyWindow.Value;
      //container.ComposeParts(this);
      //container.SatisfyImportsOnce(this);

      DataContext = viewModel;
    }
    private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
      viewModel.PasswordChanged(passwordBox.Password);
    }
    public void OnImportsSatisfied()
    {
      System.Diagnostics.Debug.WriteLine($"ViewModel : {ViewModel}");
    }
  }
}
