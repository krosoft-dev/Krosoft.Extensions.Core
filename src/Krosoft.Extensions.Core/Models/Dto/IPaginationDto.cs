namespace Krosoft.Extensions.Core.Models.Dto;

public interface IPaginationDto
{
    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string[]? SortBy { get; set; }

    public string? Text { get; set; }
}