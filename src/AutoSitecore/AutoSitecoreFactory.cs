using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoSitecore.Builders;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace AutoSitecore
{
  internal class AutoSitecoreFactory
  {
    private IFixture _fixture;

    public AutoSitecoreFactory(IFixture fixture)
    {
      this._fixture = fixture;
    }

    public Item MakeItem(ItemDataAttribute itemData)
    {
      string itemName = itemData.Name ?? _fixture.Create<string>("itemName");

      _fixture.Customizations.Insert(0, new ItemNameBuilder(itemName));
      _fixture.Customizations.Insert(0, new TemplateIdBuilder(itemData.TemplateId));
      _fixture.Customizations.Insert(0, new ItemIdBuilder(itemData.ItemId));

      if (itemData.HasFields)
      {
        _fixture.Customizations.Insert(0, new ItemFieldBuilder(_fixture));
      }
      
      ItemData data = _fixture.Create<ItemData>();
      Database db = _fixture.Create<Database>();
      var item = Substitute.For<Item>(data.Definition.ID, data, db);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);

      SetItemFields(item);

      item.Paths.Returns(
        _fixture.Build<ItemPath>().FromFactory(() => Substitute.For<ItemPath>(item))
          .Create());

      return item;
    }

    private void SetItemFields(Item item)
    {
      var fields = item.InnerData.Fields;
      item.Fields.Returns(Substitute.For<FieldCollection>(item));
      item.Fields.Count.Returns(fields.Count);
      int i = 0;
      foreach (var id in fields.GetFieldIDs())
      {
        string value = fields[id];
        item[id].Returns(value);
        var field = Substitute.For<Field>(id, item);
        field.Value.Returns(value);
        item.Fields[id].Returns(field);
        item.Fields[i++].Returns(field);
      }
    }
  }

  internal class ItemFieldBuilder : ISpecimenBuilder
  {
    private readonly IFixture _fixture;

    public ItemFieldBuilder(IFixture fixture)
    {
      _fixture = fixture;
    }

    public object Create(object request, ISpecimenContext context)
    {
      var info = request as ParameterInfo;

      if (info == null || info.ParameterType != typeof (FieldList) || info.Name != "fields")
      {
        return new NoSpecimen();
      }

      var list = new FieldList();
      List<ID> ids = _fixture.CreateMany<ID>().ToList();
      List<string> values  = _fixture.CreateMany<string>("value").ToList();
      for (int i = 0; i < ids.Count; i++)
      {
        list.Add(ids[i], values[i]);
      }
      return list;


    }
  }
}