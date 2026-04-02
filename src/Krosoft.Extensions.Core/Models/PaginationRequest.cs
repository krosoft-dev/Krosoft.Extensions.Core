using Krosoft.Extensions.Core.Models.Dto;

namespace Krosoft.Extensions.Core.Models;

public record PaginationRequest : ISearchPaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public ISet<string> SortBy { get; set; } = new HashSet<string>();
    public string? Text { get; set; }

    public static PaginationRequest From(IPaginationDto dto) => new()
    {
        Text = dto.Text,
        PageNumber = dto.PageNumber ?? 1,
        PageSize = dto.PageSize ?? 10,
        SortBy = dto.SortBy != null ? dto.SortBy.ToHashSet() : new HashSet<string>()
    };
}