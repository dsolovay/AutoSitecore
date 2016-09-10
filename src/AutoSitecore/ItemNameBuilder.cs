using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace AutoSitecore
{
  internal class ItemNameBuilder : ISpecimenBuilder
  {
    private readonly string _itemName;

    public ItemNameBuilder(string itemName)
    {
      if (itemName == null) throw new ArgumentException(nameof(itemName)); //TODO Replace with domain objects.
      _itemName = itemName; 
    }

    public object Create(object request, ISpecimenContext context)
    {
      ParameterInfo p = request as ParameterInfo;

      if (p == null || p.ParameterType != typeof(string) || p.Name != "itemName")
      {
        return new NoSpecimen();
      }

      return _itemName;
    }
  }
}