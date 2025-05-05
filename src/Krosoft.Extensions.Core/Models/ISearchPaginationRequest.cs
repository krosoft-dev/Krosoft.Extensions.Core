namespace Krosoft.Extensions.Core.Models;

public interface ISearchPaginationRequest : IPaginationRequest
{
    public string? Text { get; set; }
}