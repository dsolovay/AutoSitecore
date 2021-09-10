using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Sitecore.Data.Fields;

namespace AutoSitecoreUnitTest.Asssertions
{
  public class SitecoreFieldAssertions: ReferenceTypeAssertions<Field, SitecoreFieldAssertions>
  {
    private Field instance;


    protected override string Context => "field";

    public SitecoreFieldAssertions(Field instance)
    {
      this.instance = instance;
    }

    public AndConstraint<SitecoreFieldAssertions> HaveValue(string expected, string because = "", params object[] becauseArgs)
    {
      Execute.Assertion
        .BecauseOf(because, becauseArgs)
        .ForCondition(instance != null)
        .FailWith("Expected {context:field} to have value {0}{reason}, but the field is null",expected)
        .Then
        .ForCondition(instance.Value == expected)
        .FailWith("Expected {context:field} to have value {0}{reason}, but found {1}", expected, instance.Value);

      return new AndConstraint<SitecoreFieldAssertions>(this);

    }
  }
}