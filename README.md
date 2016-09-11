# AutoSitecore
Autofixture customizations for Sitecore 8.2

## What is this?
AutoSitecore is an extension to [AutoFixture](https://github.com/AutoFixture/AutoFixture) which allows injecting NSubstitute Items directly into 
unit test parameters.  It leverages the testability features of Sitecore 8.2, streamlining the creation of test items:

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
    
In addition, the `ItemData` attribute can be used to set properties on the item:

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
    
Finally, AutoSitecore creaetes a Substitute for the `Sitecore.Data.Database` class, and within each test, this is a singleton, using AutoFixture's `Fixture.Inject` capability (See http://stackoverflow.com/a/18172472/402949).  So, you can get at the same substitute in several ways:
  
    [Theory, AutoSitecore]
    public void DatabaseIsSame(Item item, Database db)
    {
      Assert.Same(db, item.Database);
    }

Note: These tests are included in [DocumentationTest.cs](src/AutoSitecoreUnitTest/DocumentationTest.cs).
    
## Sounds great, how do I get started?

  1. Create a C# class library.
  2. Install the following NuGet packages:
     * AutoSitecore
        * This loads a number of projects, notably Microsoft.Extensions.DependencyInjection. These are required by the Sitecore Item and Database classes to be instantiated.
     * XUnit (or NUnit)
     * AutoFixture.Xunit2 (or AutoFixture with NUnit)
        * It is important to match the version number of XUnit or NUnit to the AutoFixture extension.  For example, NUNit 3.x requires AutoFixture.NUnit3, and XUnit 2.x requires AutoFixture.Xunit2.
  3. Add reference to the following Sitecore DLLs:
      * Sitecore.Kernell.dll version 8.2 
      * Sitecore.Logging.dll version 8.2 
  4. Create an AutoSitecore attribute in your test project. Of course, you don't have to name it AutoSitecoreAttribute, but doing so will make it consistent with the examples.  
  
     ```
     public class AutoSitecoreAttribute : AutoDataAttriubte
     {
       Fixture.Customize(new AutoSitecoreCustomization());
     }
     ```
     See [AutoSitecoreAttribute.cs](src/AutoSitecoreUnitTest/AutoSitecoreAttribute.cs) in the AutoSitecoreUnitTest project. This is not included in the main project so as to avoid taking on a dependency on the XUnit for NUnit attribute projects, so that users can select their preference.
     
## Can I further customize this?

Absolutely, that is one of the core strengths of AutoFixture.  Some suggestions:

  1. Create a subclass of ItemDataAttribute called FolderItemAttribute that always creates folder items.
  2. Create a AutoFixture Customization to create Fields with certain values: e.g. "Title":"Welcome to Sitecore".
  3. Add the NSubstituteCustomization to your AutoSitecoreAttribute to allow creating substitutes of interfaces, or classes that depend on interfaces. (I considred adding this to the default implemenation, but decided it made more sense to leave this up to users.)

## What's next?

   1. Add abilty to use ItemDataAttribute functionality when creating items within test body (as opposed to method parameters).
   2. Suggesgtions, via Issue tracker, are welcome.
