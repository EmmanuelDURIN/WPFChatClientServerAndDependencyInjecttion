using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFChatClient
{
  /// <summary>
  /// Logique d'interaction pour App.xaml
  /// </summary>
  public partial class App : Application
  {
    static App()
    {
      log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
    }
  }
}
