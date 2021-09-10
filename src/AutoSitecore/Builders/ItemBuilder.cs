using AutoSitecore.Builders;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoSitecore.Builders
{
  internal class ItemBuilder : ISpecimenBuilder
  {

    public object Create(object request, ISpecimenContext context)
    {

      var typeInfo = request as Type;
      var paramInfo = request as ParameterInfo;
      var itemRequest = request as ItemRequest;
      
      if ((typeInfo == null && paramInfo == null && itemRequest == null))
      {
        return new NoSpecimen();
      } else if (typeInfo != null && typeInfo != typeof(Item))
      {
        return new NoSpecimen();
      } else if (paramInfo != null && paramInfo.ParameterType != typeof(Item))
      {
        return new NoSpecimen();
      }      

      ItemDataAttribute itemDataAttribute = paramInfo?.GetCustomAttributes(typeof(ItemDataAttribute))?.FirstOrDefault() as ItemDataAttribute ?? new ItemDataAttribute();

      List<System.Attribute> fields = paramInfo?.GetCustomAttributes(typeof(FieldDataAttribute)).ToList();
      itemDataAttribute.CustomFields = fields;
  
      Database db = context.Create<Database>();

      ItemData data;
      if (itemRequest != null)
      {
        data = itemRequest.ItemData;
        fields = Convert(itemRequest.Fields);
      }
      else
      {
        data = context.Resolve(itemDataAttribute) as ItemData;
      }

      return new ItemFactory(data, db, fields).Make();
    }

    private List<System.Attribute> Convert(FieldRequestCollection fields)
    {
      var list = new List<System.Attribute>();
      foreach(var field in fields)
      {
        list.Add(new FieldDataAttribute(field.Name, field.Value, field.ID?.ToString()));
      }
      return list;
    }
  }

  internal class ItemFactory
  {
    private readonly ItemData data;
    private readonly Database db;
    private readonly List<System.Attribute> attributeFields;

    public ItemFactory(ItemData data, Database db, List<System.Attribute> attributeFields)
    {
      this.data = data;
      this.db = db;
      this.attributeFields = attributeFields;
    }

    public Item Make()
    {
      var item = Substitute.For<Item>(data.Definition.ID, data, db);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);
      item.Paths.Returns(Substitute.For<ItemPath>(item));
      item.Access.Returns(Substitute.For<ItemAccess>(item));
      item.Appearance.Returns(Substitute.For<ItemAppearance>(item));
      //TODO Map remaining properties to substitutes.

      SetItemFields(item, attributeFields);

      return item;
    }
 


    private void SetItemFields(Item item, List<System.Attribute> attributeFields)
    {
      var innerFields = item.InnerData.Fields;
      item.Fields.Returns(Substitute.For<FieldCollection>(item));
      item.Fields.Count.Returns(innerFields.Count);
      int i = 0;
      foreach (var id in innerFields.GetFieldIDs())
      {
        string value = innerFields[id];
        item[id].Returns(value);
        var field = Substitute.For<Field>(id, item);
        field.Value.Returns(value);
        item.Fields[id].Returns(field);
        item.Fields[i++].Returns(field);
      }

      foreach (var attr in attributeFields ?? new List<System.Attribute>())
      {
        FieldDataAttribute fieldData = attr as FieldDataAttribute;
        if (fieldData == null) { continue; }

        ID id = fieldData.ID;
        string name = fieldData.Name;
        string value = fieldData.Value;

        if (string.IsNullOrEmpty(name)) { continue; }

        string stringForId = item[id];
        item[name].Returns(stringForId);
        Field fieldForId = item.Fields[id];
        item.Fields[name].Returns(fieldForId);

      }

    }


  }
}