namespace Krosoft.Extensions.Core.Models.Dto;

public record ErrorDto
{
    /// <summary>
    /// Code de l'erreur.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Message principale de l'erreur.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Liste de toutes les erreurs.
    /// </summary>
    public ISet<string> Errors { get; set; } = new HashSet<string>();
}