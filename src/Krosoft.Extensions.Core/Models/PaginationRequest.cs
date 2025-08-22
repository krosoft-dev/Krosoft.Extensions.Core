namespace Krosoft.Extensions.Core.Models;

public record PaginationRequest : ISearchPaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public ISet<string> SortBy { get; set; } = new HashSet<string>();
    public string? Text { get; set; }
}