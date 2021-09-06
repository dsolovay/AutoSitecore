using AutoSitecore.Builders;
using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace AutoSitecore
{
  internal class AutoSitecoreFactory
  {
    private IFixture _fixture;

    public AutoSitecoreFactory(IFixture fixture)
    {
      this._fixture = fixture;
    }

   
    // TODO Refactor to use specimen builder, so that customizations are accessible.

    public Item MakeItem(ItemDataAttribute itemData)
    {
      return MakeItem(itemData, new List<System.Attribute>());
    }
      public Item MakeItem(ItemDataAttribute itemData, List<System.Attribute> fields)
    {

    

      if (itemData == null)
      {
        itemData = ItemDataAttribute.Null;
      }
 
      string itemName = itemData.Name ?? _fixture.Create<string>("itemName");

      _fixture.Customizations.Insert(0, new ItemNameBuilder(itemName));
      _fixture.Customizations.Insert(0, new TemplateIdBuilder(itemData.TemplateId));
      _fixture.Customizations.Insert(0, new ItemIdBuilder(itemData.ItemId));

      if (itemData.HasFields || fields.Count > 0)
      {
        _fixture.Customizations.Insert(0, new ItemFieldBuilder(_fixture, itemData.HasFields, fields));
      }

      _fixture.Customizations.Insert(0, new ItemBuilder());

      //ItemData data = _fixture.Create<ItemData>();
      //Database db = _fixture.Create<Database>();
      //var item = Substitute.For<Item>(data.Definition.ID, data, db);

      var item = _fixture.Create<Item>();

      SetItemFields(item, fields);

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

      foreach (var attr in attributeFields)
      {
        FieldDataAttribute fieldData = attr as FieldDataAttribute;
        if (fieldData == null) { continue; }

        ID id = fieldData.ID;
        string name  = fieldData.Name;
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