using Ploeh.AutoFixture;
using Sitecore.Data;
using System.Collections.Generic;

namespace AutoSitecoreUnitTest
{
  internal class ItemCustomization:ICustomization
  {
   
    public ID ID { get; set; }
    public string Name { get; internal set; }
    public FieldRequestCollection Fields { get; internal set; } = new FieldRequestCollection();

    public void Customize(IFixture fixture)
    {
      throw new System.NotImplementedException();
    }

  }
}