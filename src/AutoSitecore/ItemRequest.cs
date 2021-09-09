using AutoSitecore.Builders;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;
using System.Collections.Generic;

namespace AutoSitecore
{
  public class ItemRequest : ISpecimenBuilder
  {
    public ID ID { get; set; }
    public string Name { get; set; }
    public FieldRequestCollection Fields { get; internal set; } = new FieldRequestCollection();

    public object Create(object request, ISpecimenContext context)
    {
      ItemData itemData =
        context.Resolve(new ItemDataAttribute(Name, ID.ToString())) as ItemData;

      return context.Resolve(itemData);

    }
  }
}