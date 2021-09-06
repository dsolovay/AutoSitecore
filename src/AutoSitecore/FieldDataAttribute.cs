using System;
using Sitecore.Data;

namespace AutoSitecore
{
  [AttributeUsage(System.AttributeTargets.Parameter,AllowMultiple = true)]
  public class FieldDataAttribute : Attribute {

    public FieldDataAttribute(string name, string value)
    {
      Name = name;
      Value = value;
    }
    public string Name { get; set; }
    public string Value { get; set; }

    internal ID GeneratedID { get; set; }
  }

}