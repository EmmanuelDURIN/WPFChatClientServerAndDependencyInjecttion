using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatBusinessObjects
{
  public class ChatMessage : BindableBase
  {
    private int id;
    private String speaker;
    private DateTime emissionDate;
    private String content;

    public int Id
    {
      get { return id; }
      set
      {
        SetProperty(ref id, value);
      }
    }
    public String Speaker
    {
      get { return speaker; }
      set
      {
        SetProperty(ref speaker, value);
      }
    }
    public DateTime EmissionDate
    {
      get { return emissionDate; }
      set
      {
        SetProperty(ref emissionDate, value);
      }
    }
    public String Content
    {
      get { return content; }
      set
      {
        SetProperty(ref content, value);
      }
    }
  }
}
