namespace Krosoft.Extensions.Core.Models.Dto;

public record ErrorApiDto
{
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
}