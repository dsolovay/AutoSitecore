using System;
using Sitecore.Data;

namespace AutoSitecore
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  public class ItemDataAttribute : Attribute
  {
    public static readonly ItemDataAttribute Null = new ItemDataAttribute();

    public ItemDataAttribute(string name=null, string templateId= null)
    {
      this.Name = name;
      ID result;
      if (ID.TryParse(templateId, out result))
      {
        TemplateId = result;
      }
      else
      {
        TemplateId = ID.Null;
      }
      
    }

    public string Name { get; }
    public ID TemplateId { get; }
  }
}