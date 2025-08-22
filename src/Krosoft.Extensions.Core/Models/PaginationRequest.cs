namespace Krosoft.Extensions.Core.Models;

public record PaginationRequest : ISearchPaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public ISet<string> SortBy { get; set; } = new HashSet<string>();
    public string? Text { get; set; }
}