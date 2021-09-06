using System;
using Sitecore.Data;

namespace AutoSitecore
{
  [AttributeUsage(System.AttributeTargets.Parameter,AllowMultiple = true)]
  public class FieldDataAttribute : Attribute {

    public FieldDataAttribute(string name=null, string value=null, string id=null)
    {
      Name = name;
      Value = value;
      ID = id != null ? new ID(id) : ID.NewID;
    }

 
    public string Name { get; set; }
    public string Value { get; set; }
    public ID ID { get; set; }
  }

}