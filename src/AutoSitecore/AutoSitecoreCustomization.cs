﻿using System.Runtime.CompilerServices;
using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace AutoSitecore
{
  public class AutoSitecoreCustomization: ICustomization
  {
    public void Customize(IFixture fixture)
    {
    
      fixture.Inject(Substitute.For<Database>());

      fixture.Customizations.Insert(0, new ItemBuilder(fixture));
      
    }
  }
}