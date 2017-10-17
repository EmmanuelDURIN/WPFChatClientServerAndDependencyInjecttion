using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBusinessObjects
{
  public class User : BindableBase
  {
    private String name;
    private String password;

    public String Name
    {
      get { return name; }
      set
      {
        SetProperty(ref name, value);
      }
    }
    public String Password
    {
      get { return password; }
      set
      {
        SetProperty(ref password, value);
      }
    }
    public override bool Equals(object obj)
    {
      var user = obj as User;
      if (user == null)
        return false;
      return Name == user.Name;
    }
  }
}
