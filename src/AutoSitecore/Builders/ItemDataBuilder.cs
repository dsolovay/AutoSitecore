using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;
using Sitecore.Globalization;
using System.Collections.Generic;
using System.Linq;


namespace AutoSitecore.Builders
{
  internal class ItemDataBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      ItemDataRequest itemDataRequest = request as ItemDataRequest;

      if (itemDataRequest == null)
      {
        return new NoSpecimen();
      }

      ID itemID = ID.IsNullOrEmpty(itemDataRequest.ItemId) ?
        context.Create<ID>() : itemDataRequest.ItemId;

      string itemName = string.IsNullOrEmpty(itemDataRequest.Name) ?
        context.Create("itemName") as string : itemDataRequest.Name;

      ID templateID = ID.IsNullOrEmpty(itemDataRequest.TemplateId) ?
        context.Create<ID>(): itemDataRequest.TemplateId;

      ItemDefinition defintion = new ItemDefinition(itemID, itemName, templateID, ID.Null);
      return new ItemData(defintion,
        Language.Current,
        Sitecore.Data.Version.Latest,
        GetFieldList(itemDataRequest, context));
    }

    private FieldList GetFieldList(ItemDataRequest itemDataRequest, ISpecimenContext context)
    {
      var list = new FieldList();

      List<ID> ids = new List<ID>();
      List<string> values = new List<string>();

      if (itemDataRequest.HasFields)
      {
        ids = context.CreateMany<ID>().ToList();
        values = context.CreateMany<string>("value").ToList();
      }

      foreach (var field in itemDataRequest.CustomFields ?? new List<System.Attribute>())
      {
        FieldDataAttribute fieldData = field as FieldDataAttribute;
        if (fieldData == null) { continue; }
        ids.Add(fieldData.ID);
        string value = fieldData.Value ?? context.Create("value");
        values.Add(value);
      }

      for (int i = 0; i < ids.Count; i++)
      {
        list.Add(ids[i], values[i]);
      }

      return list;
    }
  }
}