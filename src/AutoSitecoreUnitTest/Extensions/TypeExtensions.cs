using FluentAssertions.Execution;
using FluentAssertions.Types;

namespace AutoSitecoreUnitTest.Extensions
{
  static class TypeAssertionExtensions
  {

    public static void BeSubstituteOf<T>(this TypeAssertions typeAssertions, string because = "", params object[] becauseArgs)
    {
      string actualTypeName = typeAssertions.Subject.ToString();
      string expectedRootTypeName = typeof (T).Name;
      string expectedTypeName = $"Castle.Proxies.{expectedRootTypeName}Proxy";


      Execute.Assertion
        .ForCondition(actualTypeName.Equals(expectedTypeName))
        .BecauseOf(because, becauseArgs)
        .FailWith(@"Expected type {0} to be a Substitute of {1}{reason}, but its type name is not {2}", typeAssertions.Subject, typeof(T), expectedTypeName);

      Execute.Assertion
        .ForCondition(typeAssertions.Subject.IsSubclassOf(typeof (T)))
        .BecauseOf(because, becauseArgs)
        .FailWith(@"Expected type {0} to be a Substitute of {1}{reason}, but it is not a subclass of it", typeAssertions.Subject, typeof (T));

    }

    public static void NotBeSubstitute(this TypeAssertions typeAssertions, string because = "",
      params object[] becauseArgs)
    {
      string typeName = typeAssertions.Subject.ToString();
      Execute.Assertion
        .ForCondition(!typeName.StartsWith("Castle.Proxies"))
        .BecauseOf(because, becauseArgs)
        .FailWith(@"Expected type {0} to not be Substitute{reason}, but its type name begins with ""Castle.Proxies""",
          typeAssertions.Subject);
    }

  }
}
