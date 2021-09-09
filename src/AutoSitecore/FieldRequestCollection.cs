using System.Collections;
using System.Collections.Generic;

namespace AutoSitecore
{
  public class FieldRequestCollection: IEnumerable<FieldRequest>
  {
    private List<FieldRequest> internalList = new List<FieldRequest>();
    public IEnumerator<FieldRequest> GetEnumerator() => internalList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(string Name, Sitecore.Data.ID ID, string Value)
    {
      internalList.Add(new FieldRequest { Name=Name, ID=ID, Value=Value });
    }

  }
}