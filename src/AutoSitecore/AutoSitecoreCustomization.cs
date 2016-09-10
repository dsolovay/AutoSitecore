using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace AutoSitecore
{
  public class AutoSitecoreCustomization: ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Inject(Substitute.For<Database>());


      fixture.Register<Item>(() =>
      {
        ItemData data = fixture.Create<ItemData>();
         
        Database db = fixture.Create<Database>();
        return  Substitute.For<Item>(data.Definition.ID, data, db);
      });
    }
  }
}