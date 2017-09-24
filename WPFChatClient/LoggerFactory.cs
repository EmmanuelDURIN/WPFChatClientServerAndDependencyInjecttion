using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace WPFChatClient
{
  public class LoggerFactory
  {
    public Func<Type, ILog> MakeLogger
    {
      get{
        return BuildLogger;
      }
    }

    private ILog BuildLogger(Type type)
    {
      return LogManager.GetLogger(type); ;
    }
  }
}