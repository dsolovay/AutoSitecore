using System;

namespace AutoSitecore
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  public class ItemDataAttribute : Attribute
  {
    private readonly string _name;
 

    public ItemDataAttribute(string name=null)
    {
      this._name = name;
    }
  }
}