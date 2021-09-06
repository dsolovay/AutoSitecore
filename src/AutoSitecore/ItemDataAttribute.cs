using System;
using Sitecore;
using Sitecore.Data;
using Sitecore.Pipelines.GetContentEditorWarnings;

namespace AutoSitecore
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  public class ItemDataAttribute : Attribute
  {
    public static readonly ItemDataAttribute Null = new ItemDataAttribute();

    public ItemDataAttribute(string name = null, string itemId = null, string templateId = null, bool fields = false)
    {
      Name = name;

      ItemId = itemId != null ? ParseIdWithError(itemId, nameof(itemId)) : ID.Undefined;
      TemplateId = templateId != null ? ParseIdWithError(templateId, nameof(templateId)) : ID.Undefined;

      HasFields = fields;
    }

    public bool HasFields { get; }

    private ID ParseIdWithError(string value, string fieldName)
    {
      if (MainUtil.IsID(value))
      {
        return ID.Parse(value);
      }
      throw new ArgumentException(fieldName);
    }

    public string Name { get; }
    public ID TemplateId { get; }
    public ID ItemId { get; }
  }

}