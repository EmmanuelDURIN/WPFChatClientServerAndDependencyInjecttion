using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TechnicalService
{
  public class LoggerFactory
  {
    static LoggerFactory()
    {
      log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
    }
    public Func<Type, ILog> MakeLogger
    {
      get
      {
        return BuildLogger;
      }
    }

    private ILog BuildLogger(Type type)
    {
      return LogManager.GetLogger(type);
    }
  }
}