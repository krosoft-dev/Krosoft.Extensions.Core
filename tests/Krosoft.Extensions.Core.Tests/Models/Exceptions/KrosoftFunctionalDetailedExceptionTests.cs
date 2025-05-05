using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Models.Exceptions;

[TestClass]
public class KrosoftFunctionalDetailedExceptionTests
{
    [TestMethod]
    public void Constructor_WithMessageAndDetails_ShouldSetProperties()
    {
        var details = new HashSet<ErrorDetail>
        {
            new ErrorDetail("MyType", "MyProp", ["Error 1", "Error 2"])
        };

        var ex = new KrosoftFunctionalDetailedException("message", details);

        Check.That(ex.Message).IsEqualTo("message");
        Check.That(ex.ErrorDetails).ContainsExactly(details);
        Check.That(ex.InnerException).IsNull();
    }

    [TestMethod]
    public void Constructor_WithMessageErrorsAndDetails_ShouldSetProperties()
    {
        var errors = new HashSet<string> { "ERR001", "ERR002" };
        var details = new HashSet<ErrorDetail>
        {
            new ErrorDetail("TypeX", "PropX", ["X1"])
        };
        var inner = new InvalidOperationException("inner");

        var ex = new KrosoftFunctionalDetailedException("Erreur", errors, details, inner);

        Check.That(ex.Message).IsEqualTo("Erreur");
        Check.That(ex.Errors).ContainsExactly(errors);
        Check.That(ex.ErrorDetails).ContainsExactly(details);
        Check.That(ex.InnerException).IsSameReferenceAs(inner);
    }

    [TestMethod]
    public void Constructor_WithErrorsAndDetails_ShouldSetProperties()
    {
        var errors = new HashSet<string> { "E100" };
        var details = new HashSet<ErrorDetail>
        {
            new ErrorDetail("A", "B", ["msg"])
        };

        var ex = new KrosoftFunctionalDetailedException(errors, details);

        Check.That(ex.Errors).ContainsExactly(errors);
        Check.That(ex.ErrorDetails).ContainsExactly(details);
    }
}