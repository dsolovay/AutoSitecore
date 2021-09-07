using AutoSitecore.Builders;
using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore.Data;

namespace AutoSitecore
{
  public class AutoSitecoreCustomization: ICustomization
  {
    public void Customize(IFixture fixture)
    {
    
      fixture.Inject(Substitute.For<Database>());

      fixture.Customizations.Insert(0, new ItemBuilder(fixture));
      
    }
  }
}