using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;

namespace AutoSitecore
{
  class ItemDataRequest
  {
    public string Name { get; internal set; }
    public ID TemplateId { get; internal set; }
    public bool HasFields { get; internal set; }
    public List<Attribute> CustomFields { get; internal set; } = new List<Attribute>();
    public ID ItemId { get; internal set; }
  }
}
