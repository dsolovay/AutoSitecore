using AutoSitecore.Builders;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoSitecore
{
  public class ItemBuilder : ISpecimenBuilder
  {
    private IFixture _fixture;

    public ItemBuilder(IFixture fixture)
    {
      _fixture = fixture;
    }

    public object Create(object request, ISpecimenContext context)
    {

      var typeInfo = request as Type;
      var paramInfo = request as ParameterInfo;

      if ((typeInfo == null && paramInfo == null))
      {
        return new NoSpecimen();
      }

      if (typeInfo != null && typeInfo != typeof(Item))
      {
        return new NoSpecimen();
      }

      if (paramInfo != null && paramInfo.ParameterType != typeof(Item))
      {
        return new NoSpecimen();
      }

      ItemDataAttribute itemDataAttribute = paramInfo?.GetCustomAttributes(typeof(ItemDataAttribute))?.FirstOrDefault() as ItemDataAttribute;


      if (itemDataAttribute != null)
      {
        string itemName = itemDataAttribute?.Name ?? _fixture.Create("itemName");
        _fixture.Customizations.Insert(0, new ItemNameBuilder(itemName));
        _fixture.Customizations.Insert(0, new TemplateIdBuilder(itemDataAttribute.TemplateId));
        _fixture.Customizations.Insert(0, new ItemIdBuilder(itemDataAttribute.ItemId));

      }

      List<System.Attribute> fields = paramInfo?.GetCustomAttributes(typeof(FieldDataAttribute)).ToList();


      if (((itemDataAttribute?.HasFields) ?? false) || ((fields?.Count ?? 0) > 0))
      {
        _fixture.Customizations.Insert(0, new ItemFieldBuilder(_fixture, itemDataAttribute?.HasFields ?? false, fields ?? new List<System.Attribute>()));
      }

      ItemData data = _fixture.Create<ItemData>();
      Database db = _fixture.Create<Database>();

      var item = Substitute.For<Item>(data.Definition.ID, data, db);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);
      item.Paths.Returns(Substitute.For<ItemPath>(item));



      SetItemFields(item, fields);


      return item;

      //TODO migrate field creation logic


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