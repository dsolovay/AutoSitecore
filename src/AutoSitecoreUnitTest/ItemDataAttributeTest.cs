using AutoSitecore;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xunit;

namespace AutoSitecoreUnitTest
{
  public class ItemDataAttributeTest
  {
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

    [Theory, AutoSitecore]
    public void CanSetTemplateId([ItemData(templateId:new ID("543E0C95-305E-4FAD-AA5B-EDD46F613595"))] Item item)
    {
      item1.TemplateID.Should().Be(new ID("543E0C95-305E-4FAD-AA5B-EDD46F613595")); 
    }
  }
}
