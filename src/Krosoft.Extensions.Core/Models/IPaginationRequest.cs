namespace Krosoft.Extensions.Core.Models;

public interface IPaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public ISet<string> SortBy { get; set; }
}