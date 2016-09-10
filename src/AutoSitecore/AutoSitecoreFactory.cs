using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;
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
      ItemData data = _fixture.Create<ItemData>();
      Database db = _fixture.Create<Database>();
      var item = Substitute.For<Item>(data.Definition.ID, data, db);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);

      item.Paths.Returns(
        _fixture.Build<ItemPath>().FromFactory(() => Substitute.For<ItemPath>(item))
          .Create());

      return item;
    }
  }

  internal class TemplateIdBuilder : ISpecimenBuilder
  {
    private readonly ID _templateId;

    public TemplateIdBuilder(ID templateId)
    {
      _templateId = templateId;
    }

    public object Create(object request, ISpecimenContext context)
    {
      var info = request as ParameterInfo;

      if (info == null || info.ParameterType != typeof (ID) || info.Name != "templateID")
      {
        return new NoSpecimen();
      }

      if (_templateId == ID.Undefined)
      {
        return new ID();
      }

      return _templateId;
    }
  }
}