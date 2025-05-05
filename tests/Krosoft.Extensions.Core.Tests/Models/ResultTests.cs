using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
public class ResultTests
{
    private static Result<Addresse> CheckFaulted()
    {
        var result = new Result<Addresse>(new Exception("Test"));
        Check.That(result).IsInstanceOf<Result<Addresse>>();
        Check.That(result.IsSuccess).IsFalse();
        Check.That(result.IsFaulted).IsTrue();
        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception?.Message).IsEqualTo("Test");

        return result;
    }

    private static Result<Addresse> CheckSuccess()
    {
        var result = new Result<Addresse>(new Addresse(null!, null!,
                                                       "Paris", null!));
        Check.That(result).IsInstanceOf<Result<Addresse>>();
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.IsFaulted).IsFalse();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value?.Ville).IsEqualTo("Paris");
        Check.That(result.Exception).IsNull();

        return result;
    }

    [TestMethod]
    public void FaultedTest()
    {
        CheckFaulted();
    }

    [TestMethod]
    public void SuccessTest()
    {
        CheckSuccess();
    }

    [TestMethod]
    public void ValidateFailTest()
    {
        var result = CheckFaulted();

        Check.ThatCode(() => result.Validate())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Test");
    }

    [TestMethod]
    public void ValidateTest()
    {
        var result = CheckSuccess();

        var address = result.Validate();

        Check.That(address).IsNotNull();
        Check.That(address.Ville).IsEqualTo("Paris");
    }
}