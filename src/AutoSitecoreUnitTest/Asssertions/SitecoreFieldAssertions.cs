using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Sitecore.Data.Fields;

namespace AutoSitecoreUnitTest.Asssertions
{
  public class SitecoreFieldAssertions: ReferenceTypeAssertions<Field, SitecoreFieldAssertions>
  {

    protected override string Context => "field";

    public SitecoreFieldAssertions(Field instance)
    {
      this.Subject = instance;
    }

    public AndConstraint<SitecoreFieldAssertions> HaveValue(string expected, string because = "", params object[] becauseArgs)
    {
      Execute.Assertion
        .BecauseOf(because, becauseArgs)
        .ForCondition(Subject != null)
        .FailWith("Expected {context:field} to have value {0}{reason}, but the field is null",expected)
        .Then
        .ForCondition(Subject.Value == expected)
        .FailWith("Expected {context:field} to have value {0}{reason}, but found {1}", expected, Subject.Value);

      return new AndConstraint<SitecoreFieldAssertions>(this);

    }
  }
}