using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace AutoSitecore
{
  public class AutoSitecoreFactory
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
      ItemData data = _fixture.Create<ItemData>();
      Database db = _fixture.Create<Database>();
      var item = Substitute.For<Item>(data.Definition.ID, data, db);

      item.Name.Returns(item.InnerData.Definition.Name);

      item.Paths.Returns(
        _fixture.Build<ItemPath>().FromFactory(() => Substitute.For<ItemPath>(item))
          .Create());

      return item;
    }
  }
}