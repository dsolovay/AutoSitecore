using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace AutoSitecore
{
  internal class ItemDataCustomization : ISpecimenBuilder
  {
    private readonly IFixture _fixture;

    public ItemDataCustomization(IFixture fixture)
    {
      _fixture = fixture;
    }

    public object Create(object request, ISpecimenContext context)
    {
      ParameterInfo info = request as ParameterInfo;

      if (info == null)
      {
        return new NoSpecimen();
      }

      ItemDataAttribute itemData = info.GetCustomAttributes(typeof (ItemDataAttribute)).FirstOrDefault() as ItemDataAttribute;

      List<System.Attribute> fields = info.GetCustomAttributes(typeof(FieldDataAttribute)).ToList();

      if (itemData == null && fields.Count == 0)
      {
        return new NoSpecimen();
      }
      
      return new AutoSitecoreFactory(_fixture).MakeItem(itemData, fields);
    }
  }
}