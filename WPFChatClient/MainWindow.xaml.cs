using ChatViewModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

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
