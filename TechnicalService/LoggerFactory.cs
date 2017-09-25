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