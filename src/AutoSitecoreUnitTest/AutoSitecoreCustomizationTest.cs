using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoSitecore;
using FluentAssertions;
using Ploeh.AutoFixture;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace AutoSitecoreUnitTest
{
  public class AutoSitecoreCustomizationTest
  {
    [Fact]
    public void IsAutoFixtureCustomization()
    {
      Fixture fixture = new Fixture();
      ICustomization sut = new AutoSitecoreCustomization();

      fixture.Customize(sut);

    }

    [Fact]
    public void CreatesAnonymousIds()
    {
      Fixture fixture = new Fixture();
      fixture.Customize(new AutoSitecoreCustomization());

      ID anonyousId = fixture.Create<ID>();

      anonyousId.Should().NotBe(ID.Null);
    }

    [Fact]
    public void CreatesItemDefinitions()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var definition = fixture.Create<ItemDefinition>();

    }

    [Fact]
    public void CreatesItemData()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var data = fixture.Create<ItemData>();
    }

    [Fact]
    public void CreatesSubstituteDatabase()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var db = fixture.Create<Database>();
    }

    [Fact]
    public void CreatesItemAsSubstitute()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var item = fixture.Create<Item>();

      item.GetType().Should().NotBe<Item>();
      item.GetType().Should().BeDerivedFrom<Item>();
      item.GetType().FullName.Should().Be("Castle.Proxies.ItemProxy");
    }

    [Fact]
    public void KeyIsEmpty()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var item = fixture.Create<Item>();

      item.Key.Should().BeEmpty();
    }

    [Fact]
    public void ItemIdsAreSame()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var item = fixture.Create<Item>();

      item.ID.Should().Be(item.InnerData.Definition.ID);
    }
  }
}
