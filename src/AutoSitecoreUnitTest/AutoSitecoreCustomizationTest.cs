using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoSitecore;
using AutoSitecoreUnitTest.Extensions;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Pipelines.HttpRequest;
using Xunit;
using Xunit.Abstractions;
using Version = Sitecore.Data.Version;

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

    #region Item constructor element tests

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

      definition.Should().NotBeNull();
      definition.ID.Should().NotBe(ID.Null);
    }

    /// <summary>
    /// Documents default values created by AutoFixture. It should be possible to change these using 
    /// SpecimenBuilder customizations.
    /// </summary>
    [Fact]
    public void CreatesItemData()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var data = fixture.Create<ItemData>();

      data.Should().NotBeNull();
      data.Language.Should().Be(Language.Invariant);
      data.Version.Should().Be(Version.Parse(0));
    }

    #endregion


    #region Database tests

    [Fact]
    public void CreatesSubstituteDatabase()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var db = fixture.Create<Database>();

      db.Should().NotBeNull();
      db.GetType().Should().BeSubstituteOf<Database>();
    }

    [Fact]
    public void AlwaysReturnsSameDatabase()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var db1 = fixture.Create<Database>();
      var db2 = fixture.Create<Database>();
      var item = fixture.Create<Item>();

      db1.Should().BeSameAs(db2);
      db1.Should().BeSameAs(item.Database);
    }

    #endregion


    [Fact]
    public void CreatesItemAsSubstitute()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());

      var item = fixture.Create<Item>();

      item.GetType().Should().BeSubstituteOf<Item>();
    }

    [Theory, AutoSitecore]
    public void ItemDefintionHasName(ItemDefinition definition)
    {
      definition.Name.Should().NotBeEmpty();
    }

    [Theory, AutoSitecore]
    public void ItemDataHasName(ItemData data)
    {
      data.Definition.Name.Should().NotBeEmpty();
    }

    [Theory, AutoSitecore]
    public void NoAutoPropertiesMakesRealItem([NoAutoProperties] Item item)
    {
      item.GetType().Should().Be<Item>(
        "this documents unexpected behavior of AutoFixture");
    }

    [Theory, AutoSitecore]
    public void FixtureCanCreateItem(IFixture fixture, string itemName)
    { 
      Item item = fixture.Create<Item>();
      item.GetType().Should().BeSubstituteOf<Item>();
    }

    [Theory, AutoSitecore]
    public void ItemKeyMatchesItemNameToLower(Item item)
    {
      item.Name.Should().NotBeEmpty();
      item.Key.Should().Be(item.Name.ToLower());
    }

    /// <summary>
    /// When Item is injected as a parameter, the Key is accessed during the process (I have not yet
    /// identified why or where), and this causes it to get tied to the generated itemName.  However,
    /// if an item is generated directly from a fixture, and OmitAutoProperties is set to true, then
    /// a new name can be assigned, and the key will match it.
    /// </summary> 
    [Theory, AutoSitecore]
    public void ItemNameCanBeSetIfCreatedFromFixture(IFixture fixture, string itemName)
    {
      fixture.OmitAutoProperties = true;
      Item item = fixture.Create<Item>();
      item.Name.Returns(itemName);
      item.Key.Should().Be(itemName.ToLower());
    }

    [Theory, AutoSitecore]
    public void ItemNameMatchesInnerDataDefintionName(Item item)
    {
      string innerName = item.InnerData.Definition.Name;
      item.Name.Should().Be(innerName, "becaue they are set to match during Substitute construction");
    }

 
    [Fact]
    public void ItemIdsAreSame()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());
      var item = fixture.Create<Item>();
      item.ID.Should().Be(item.InnerData.Definition.ID);
    }

    [Theory, AutoSitecore]
    public void CanSetPath(Item item)
    {
      item.Paths.FullPath.Returns("/sitecore/content/home");
    }

    [Theory, AutoSitecore]
    public void CanCreateManyItems(IEnumerable<Item> items)
    {
      items.Count().Should().Be(3, "this is AutoFixture standard behavior");
      items.First().GetType().ToString().Should().Be("Castle.Proxies.ItemProxy");
      items.First().ID.Should().NotBe(items.Last().ID);
    }
  }
}
