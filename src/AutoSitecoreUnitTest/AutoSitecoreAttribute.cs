using System;
using AutoSitecore;
using Ploeh.AutoFixture.Xunit2;

namespace AutoSitecoreUnitTest
{
  internal class AutoSitecoreAttribute : AutoDataAttribute
  {
    public AutoSitecoreAttribute()
    {
      Fixture.Customize(new AutoSitecoreCustomization());
    }
  }
}