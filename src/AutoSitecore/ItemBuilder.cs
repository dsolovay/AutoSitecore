using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace AutoSitecore
{
  public class ItemBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {

      var info = request as Type;
      if (info == null || info != typeof(Sitecore.Data.Items.Item))
      {
        return new NoSpecimen();
      }

      ItemData data = context.Resolve(typeof(ItemData)) as ItemData;
      Database db = context.Resolve(typeof(Database)) as Database;
      var item = Substitute.For<Item>(data.Definition.ID, data, db);

      item.Name.Returns(item.InnerData.Definition.Name);
      item.TemplateID.Returns(item.InnerData.Definition.TemplateID);

      return item;

      //TODO migrate field creation logic


    }
  }


}