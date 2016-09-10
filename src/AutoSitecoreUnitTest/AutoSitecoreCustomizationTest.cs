using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xunit;

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
    public void CreatesItem()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var item = fixture.Build<Item>().OmitAutoProperties().Create();
      item.ID.Should().NotBe(ID.Null);
      item.Name.Should().NotBeEmpty();
      item.TemplateID.Should().NotBe(ID.Null);
      item.BranchId.Should().NotBe(ID.Null);
      item.Fields.Count.Should().Be(0);
    }
  }

  public class AutoSitecoreCustomization: ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Inject(Substitute.For<Database>());
       

      //fixture.Build<Item>().FromFactory(() =>
      //{
      //  ID itemID = fixture.Create<ID>();
      //  ;
      //  ItemData data = fixture.Create<ItemData>();
      //  Database db = fixture.Create<Database>();
      //  return Substitute.For<Item>(itemID, data, db);
      //});
    }
  }
}
