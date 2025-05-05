using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Models.Exceptions;

[TestClass]
public class KrosoftTechnicalExceptionTests
{
    [TestMethod]
    public void KrosoftTechnicalException_Erreurs()
    {
        var ex = new KrosoftTechnicalException("err-1");
        Check.That(ex.Errors).HasSize(1);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNull();
    }

    [TestMethod]
    public void KrosoftTechnicalException_Erreurs_Multitples_erreurs()
    {
        var ex = new KrosoftTechnicalException(new HashSet<string> { "err-1", "err-2", "err-3" });
        Check.That(ex.Errors).HasSize(3);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNull();
    }

    [TestMethod]
    public void KrosoftTechnicalException_InnerException()
    {
        var ex = new KrosoftTechnicalException("err-1", new NotImplementedException());
        Check.That(ex.Errors).HasSize(1);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNotNull();
        Check.That(ex.InnerException!.Message).IsEqualTo("The method or operation is not implemented.");
    }

    [TestMethod]
    public void KrosoftTechnicalException_InnerException_Multitples_erreurs()
    {
        var ex = new KrosoftTechnicalException(new HashSet<string> { "err-1", "err-2", "err-3" }, new NotImplementedException());
        Check.That(ex.Errors).HasSize(3);
        Check.That(ex.Message).IsEqualTo("err-1");
        Check.That(ex.InnerException).IsNotNull();
        Check.That(ex.InnerException!.Message).IsEqualTo("The method or operation is not implemented.");
    }
}