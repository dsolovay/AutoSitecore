using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoSitecore;
using NSubstitute;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Xunit;

namespace AutoSitecoreUnitTest
{
  public class DocumentationTest
  {
    [Theory, AutoSitecore]
    public void CreateTestItem(Item item)
    {
      // Basic item values set up
      Assert.NotNull(item);
      Assert.NotEqual(item.ID, ID.Null);
      Assert.NotEqual(item.TemplateID, ID.Null);

      // NSubstitute features for all virtual fields
      item.Name.Returns("some new name");
      item.DidNotReceiveWithAnyArgs().Add("", new TemplateID());
    }

    [Theory, AutoSitecore]
    public void CreateItemWithValues([ItemData(itemId:"{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}",
      templateId:"{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}", name:"Home", fields:true)] Item item)
    {
      Assert.Equal(ID.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"), item.ID);
      Assert.Equal(ID.Parse("{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}"), item.TemplateID);
      Assert.Equal("Home", item.Name);
      Assert.Equal("home", item.Key); 
      Assert.Equal(3, item.Fields.Count()); // Follows AutoFixture standard of creating three items.

      // fields can be accessed on item or Fields collection
      ID id = item.InnerData.Fields.GetFieldIDs().First();
      string value = item.InnerData.Fields[id];
      Assert.Equal(value, item[id]);
      Assert.Equal(value, item.Fields[id].Value);
    }

    [Theory, AutoSitecore]
    public void DatabaseIsSame(Item item, Database db)
    {
      Assert.Same(db, item.Database);
    }
  }
}
