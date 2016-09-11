using System;
using System.Linq;
using AutoSitecore;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
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
    }
    

    [Theory(Skip="Cannot test Theory exceptions in xUnit"), AutoSitecore]
    public void InvalidIdThrows([ItemData(itemId:"invalid value")]Item item)
    {
      // xUnit does not have an ExpectedExceptionAttribute, so no way to document
      // that an invalid attribute throws.
    }
  }

  
}
