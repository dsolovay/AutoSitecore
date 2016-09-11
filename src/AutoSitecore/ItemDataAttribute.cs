using System;
using Sitecore.Data;

namespace AutoSitecore
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  public class ItemDataAttribute : Attribute
  {
    public static readonly ItemDataAttribute Null = new ItemDataAttribute();

    public ItemDataAttribute(string name=null, string itemId = null, string templateId= null)
    {
      Name = name;
      ItemId = ID.Parse(itemId, ID.Undefined);
      TemplateId = ID.Parse(templateId, ID.Undefined);
    }

    public string Name { get; }
    public ID TemplateId { get; }
    public ID ItemId { get; }
  }
}