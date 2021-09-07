using AutoSitecore.Builders;
using Ploeh.AutoFixture;
using Sitecore.Data;

namespace AutoSitecore
{
  public class AutoSitecoreCustomization: ICustomization
  {
    public void Customize(IFixture fixture)
    {
    
      fixture.Inject(NSubstitute.Substitute.For<Database>());

      fixture.Customizations.Insert(0, new ItemDataBuilder());

      fixture.Customizations.Insert(0, new ItemBuilder());
      
    }
  }
}