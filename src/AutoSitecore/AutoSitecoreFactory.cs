using AutoSitecore.Builders;
using NSubstitute;
using Ploeh.AutoFixture;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace AutoSitecore
{
  internal class AutoSitecoreFactory
  {
    private IFixture _fixture;

    public AutoSitecoreFactory(IFixture fixture)
    {
      this._fixture = fixture;
    }

   
    // TODO Refactor to use specimen builder, so that customizations are accessible.

      
    }
  }