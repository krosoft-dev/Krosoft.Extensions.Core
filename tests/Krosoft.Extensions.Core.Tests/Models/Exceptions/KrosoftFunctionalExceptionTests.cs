using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Models.Exceptions;

[TestClass]
public class KrosoftFunctionalExceptionTests
{
    [TestMethod]
    public void KrosoftFunctionalException_Message()
    {
        var ex = new KrosoftFunctionalException("err-0", new HashSet<string> { "err-1", "err-2", "err-3" });
        Check.That(ex.Errors).HasSize(3);
        Check.That(ex.Message).IsEqualTo("err-0");
        Check.That(ex.InnerException).IsNull();
    }

    [TestMethod]
    public void KrosoftFunctionalException_Errors()
    {
        var ex = new KrosoftFunctionalException("err-1");
        Check.That(ex.Errors).HasSize(1);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNull();
    }

    [TestMethod]
    public void KrosoftFunctionalException_Errors_Multitples_erreurs()
    {
        var ex = new KrosoftFunctionalException(new HashSet<string> { "err-1", "err-2", "err-3" });
        Check.That(ex.Errors).HasSize(3);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNull();
    }

    [TestMethod]
    public void KrosoftFunctionalException_InnerException()
    {
        var ex = new KrosoftFunctionalException("err-1", new NotImplementedException());
        Check.That(ex.Errors).HasSize(1);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNotNull();
        Check.That(ex.InnerException!.Message).IsEqualTo("The method or operation is not implemented.");
    }

    [TestMethod]
    public void KrosoftFunctionalException_InnerException_Multitples_erreurs()
    {
        var ex = new KrosoftFunctionalException(new HashSet<string> { "err-1", "err-2", "err-3" }, new NotImplementedException());
        Check.That(ex.Errors).HasSize(3);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNotNull();
        Check.That(ex.InnerException!.Message).IsEqualTo("The method or operation is not implemented.");
    }
}