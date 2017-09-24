using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPFChatClient
{
  public class LoggerClient
  {
    public ILog Logger { get; set; }

    private Func<Type, ILog> makeLogger;
    public Func<Type, ILog> MakeLogger
    {
      get
      {
        return makeLogger;
      }
      set
      {
        makeLogger = value;
        Logger = MakeLogger(this.GetType());
      }
    }
  }
}