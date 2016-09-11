using AutoSitecore;
using FluentAssertions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xunit;

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
    public void CanSetId([ItemData(id: ContentRootId)] Item item)
    {
      item.ID.Should().Be(ItemIDs.ContentRoot);
    }



  }
}
