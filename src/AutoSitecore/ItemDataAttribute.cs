using System;
using System.Collections.Generic;
using Sitecore;
using Sitecore.Data;
using Sitecore.Pipelines.GetContentEditorWarnings;

namespace AutoSitecore
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
  public class ItemDataAttribute : Attribute
  {
    public ItemDataAttribute(string name = null, string itemId = null, string templateId = null, bool fields = false)
    {
      Name = name;

      ItemId = itemId != null ? ParseIdWithError(itemId, nameof(itemId)) : null;
      TemplateId = templateId != null ? ParseIdWithError(templateId, nameof(templateId)) : null;

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
    public List<Attribute> CustomFields { get; internal set; } = new List<Attribute>();

    internal ItemDataRequest ToItemRequest()
    {
      return new ItemDataRequest
      {
        Name = this.Name,
        ItemId = this.ItemId,
        TemplateId = this.TemplateId,
        CustomFields = this.CustomFields,
        HasFields = this.HasFields

      };
    }
  }

}