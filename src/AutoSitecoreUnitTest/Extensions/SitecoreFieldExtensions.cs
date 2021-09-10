using AutoSitecoreUnitTest.Asssertions;

namespace AutoSitecoreUnitTest.Extensions
{
  public static class SitecoreFieldExtensions
  {
    public static SitecoreFieldAssertions Should(this Sitecore.Data.Fields.Field instance)
    {
      return new SitecoreFieldAssertions(instance);
    }
  }
}
