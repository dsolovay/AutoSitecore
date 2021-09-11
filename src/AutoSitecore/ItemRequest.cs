﻿using AutoSitecore.Builders;
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
    internal ItemData ItemData { get; private set; }

    public object Create(object request, ISpecimenContext context)
    {
      ItemDataRequest itemDataRequest = new ItemDataRequest { Name=Name, ItemId=ID };
      foreach(var field in Fields)
      {
        itemDataRequest.CustomFields.Add(new FieldDataAttribute(field.Name, field.Value, field.ID?.ToString()));
      }
      ItemData = context.Resolve(itemDataRequest) as ItemData;

      return context.Resolve(this);

    }
  }
}