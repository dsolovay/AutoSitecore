using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;

namespace AutoSitecore
{
  internal class ItemFieldBuilder : ISpecimenBuilder
  {
    private readonly IFixture _fixture;
    private readonly bool autoGenerateFields;
    private readonly List<System.Attribute> customFieldAttributes = new List<System.Attribute>();

    public ItemFieldBuilder(IFixture fixture, bool autoGenerateFields, List<System.Attribute> customFieldAttributes)
    {
      _fixture = fixture;
      this.autoGenerateFields = autoGenerateFields;
      this.customFieldAttributes = customFieldAttributes;
    }

    public object Create(object request, ISpecimenContext context)
    {
      var info = request as ParameterInfo;

      if (info == null || info.ParameterType != typeof (FieldList) || info.Name != "fields")
      {
        return new NoSpecimen();
      }

      var list = new FieldList();

      //TODO Get desired fields from context
      List<ID> ids = new List<ID>();
      List<string> values = new List<string>();

      if (this.autoGenerateFields)
      {
        ids = _fixture.CreateMany<ID>().ToList();
        values = _fixture.CreateMany<string>("value").ToList();
      }

      foreach (var field in this.customFieldAttributes)
      {
        FieldDataAttribute fieldData = field as FieldDataAttribute;
        if (fieldData == null) { continue; }

        fieldData.GeneratedID = _fixture.Create<ID>();
        ids.Add(fieldData.GeneratedID);
        string value = fieldData.Value ?? _fixture.Create("value");
        values.Add(value);
      }
       
      for (int i = 0; i < ids.Count; i++)
      {
        list.Add(ids[i], values[i]);
      }
      return list;


    }
  }
}