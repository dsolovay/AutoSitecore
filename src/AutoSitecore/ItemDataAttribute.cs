using System;
using Sitecore.Data;

namespace AutoSitecore
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  public class ItemDataAttribute : Attribute
  {
    public static readonly ItemDataAttribute Null = new ItemDataAttribute();

    public ItemDataAttribute(string name=null, string id = null, string templateId= null)
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
      ID result2;
     
      if (ID.TryParse(id, out result2))
      {
        ItemId = result2;
      }
      else
      {
        ItemId = ID.Undefined;
      }

    }

    public string Name { get; }
    public ID TemplateId { get; }
    public ID ItemId { get; }
  }
}