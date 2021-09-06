using System;
using System.Linq;
using AutoSitecore;
using AutoSitecoreUnitTest.Extensions;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Ploeh.AutoFixture;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using NSubstitute;
using Xunit;
using Xunit.Sdk;

namespace AutoSitecoreUnitTest
{
  public class ItemDataAttributeTest
  {
    private const string ContentRootId = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";

    private const string TemplateIdAsString = "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}";

    [Theory, AutoSitecore]
    public void CanSetName([ItemData(name: "specifiedName")] Item item)
    {
      item.Name.Should().Be("specifiedName");
      item.Key.Should().Be("specifiedname");
    }

    [Theory, AutoSitecore]
    public void NameDoesNotPersist([ItemData("specifiedName")] Item item1, Item item2)
    {
      item1.Name.Should().Be("specifiedName");
      item2.Name.Should().NotBe("specifiedName");
    }

    /// <summary>
    /// Unfortunately, there is no way to pass TemplateIDs.Folder to an attribute, as 
    /// GUIDs and IDs cannot be compile time constants. See http://stackoverflow.com/a/1443738/402949
    /// </summary>
    [Theory, AutoSitecore]
    public void CanSetTemplateId([ItemData(templateId: TemplateIdAsString)] Item item)
    {
      item.TemplateID.Should().Be(TemplateIDs.Folder); 
    }
     

    [Theory, AutoSitecore]
    public void CanSetId([ItemData(itemId: ContentRootId)] Item item)
    {
      item.ID.Should().Be(ItemIDs.ContentRoot);
    }

    [Theory, AutoSitecore]
    public void CanLeaveFieldsEmpty([ItemData(fields: false)] Item item)
    {
      item.InnerData.Fields.Count.Should().Be(0);
    }
    [Theory, AutoSitecore]
    public void CanCreateFields([ItemData(fields: true)] Item item)
    {
      FieldList innerFields = item.InnerData.Fields;
      innerFields.Count.Should().Be(3, "non virtual inner collection");
      item.Fields.Count.Should().Be(3, "virtual method value set by Factory");

      foreach (var id in innerFields.GetFieldIDs())
      {
        string value = innerFields[id];
        id.Should().NotBe(ID.Null, "field ID should be set");
        value.Should().NotBeNullOrEmpty("anonymous field value should be set");
        item[id].Should().Be(value, "item indexer set");
        item.Fields[id].Value.Should().Be(value, "Fields indexer set"); 
      }
      var firstId = innerFields.GetFieldIDs().First();
      var lastId = innerFields.GetFieldIDs().Last();

      firstId.Should().NotBe(lastId, "IDs should differ");
      innerFields[firstId].Should().NotBe(innerFields[lastId], "values should differ");

      item.Fields[firstId].Should().BeSameAs(item.Fields[0]);

      item.Fields[lastId].Should().BeSameAs(item.Fields[item.Fields.Count - 1]);
      item.Fields[firstId].Should().NotBeSameAs(item.Fields[lastId]);
    }

    [Fact]
    public void CanCreateSpecificFields()
    {
      IFixture fixture = new Fixture().Customize(new AutoSitecoreCustomization());
      Item item = fixture.Create<Item>();
      item["Title"].Returns("Welcome to Sitecore");
      item["Title"].Should().Be("Welcome to Sitecore");
      item.Fields["Title"].Should().BeNull(because: "Must be set separately.");

      item.Fields["Title"].Returns(Substitute.For<Field>(ID.Null, item));
      item.Fields["Title"].Value.Returns("Second value");
      item.Fields["Title"].Value.Should().Be("Second value");

    }

    [Fact()]
    public void InvalidIdThrows()
    { 
      Assert.Throws<ArgumentException>(() =>  new ItemDataAttribute(itemId: "invalid value"));
    }

    [Theory, AutoSitecore]
    public void CanAccessFieldsByIndexer([ItemData(fields: true)] Item item)
    {
      item.Fields[0].GetType().Should().BeSubstituteOf<Sitecore.Data.Fields.Field>();
      item.Fields[1].GetType().Should().BeSubstituteOf<Sitecore.Data.Fields.Field>();
      item.Fields[0].Should().NotBeSameAs(item.Fields[1]);
    }

    [Theory, AutoSitecore]
    public void CanSetUsingIndexer([ItemData(fields: true)] Item item, string someValue, string someFieldName)
    {
      
      item.Fields[0].Value.Returns(someValue);
      Field substituteField = item.Fields[0];
      item.Fields[someFieldName].Returns(substituteField);

      item.Fields[someFieldName].Value.Should().Be(someValue);
    }


    [Theory, AutoSitecore]
    public void CanSetFieldValue([FieldData(name: "Field 1", value: "Value 1")][FieldData(name: "Field 2", value: "Value 2")]Item item)
    {

      item.Fields["Field 1"].Value.Should().Be("Value 1");
    }

    [Theory, AutoSitecore]
    public void CanSetFieldIds(
      [FieldData(id:"11b1c214-e002-4616-86f1-a36ec008a3b4", value: "Value 1")]
      [FieldData(id:"d1441cbc-1052-488a-ac07-486e0d3c64fb", value: "Value 2")]
    Item item)
    {
      item.Fields[new ID("11b1c214-e002-4616-86f1-a36ec008a3b4")].Value.Should().Be("Value 1");
      item.Fields[new ID("d1441cbc-1052-488a-ac07-486e0d3c64fb")].Value.Should().Be("Value 2");
    }

  }


}
