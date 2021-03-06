using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;

namespace AutoSitecore.Builders
{
  internal class ItemIdBuilder : ISpecimenBuilder
  {
    private readonly ID _itemId;

    public ItemIdBuilder(ID itemId)
    {
      _itemId = itemId;
    }

    public object Create(object request, ISpecimenContext context)
    {
      var info = request as ParameterInfo;

      if (info == null || info.ParameterType != typeof (ID) || info.Name != "itemID")
      {
        return new NoSpecimen();
      }

      if (_itemId == ID.Undefined)
      {
        return new ID();
      }

      return _itemId;
    }
  }
}