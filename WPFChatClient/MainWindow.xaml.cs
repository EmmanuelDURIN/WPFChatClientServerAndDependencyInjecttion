﻿using Autofac;
using ChatViewModel;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using System.Windows;

namespace WPFChatClient
{
  public partial class MainWindow : Window
  {
    private MainWindowViewModel viewModel = null;
    public MainWindowViewModel ViewModel { get => viewModel; set => viewModel = value; }
    public MainWindow(MainWindowViewModel viewModel)
    {
      InitializeComponent();
      this.viewModel = viewModel;
      DataContext = viewModel;
    }
    private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
      viewModel.PasswordChanged(passwordBox.Password);
    }
  }
}

