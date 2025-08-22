using JetBrains.Annotations;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
[TestSubject(typeof(PaginationRequest))]
public class PaginationRequestTest
{
    [TestMethod]
    public void Empty()
    {
        var paginationRequest = new PaginationRequest();

        Check.That(paginationRequest).IsNotNull();
        Check.That(paginationRequest.PageNumber).IsEqualTo(0);
        Check.That(paginationRequest.PageSize).IsEqualTo(0);
        Check.That(paginationRequest.SortBy).IsNotNull();
        Check.That(paginationRequest.SortBy).IsEmpty();
        Check.That(paginationRequest.Text).IsNull();
    }
}