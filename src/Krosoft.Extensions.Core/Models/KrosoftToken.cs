namespace Krosoft.Extensions.Core.Models;

public record KrosoftToken
{
    public KrosoftToken()
    {
        DroitsCode = new HashSet<string>();
    }

    public string? Id { get; set; }
    public string? TenantId { get; set; }
    public string? Nom { get; set; }
    public string? Email { get; set; }
    public string? ProprietaireId { get; set; }
    public string? RoleId { get; set; }
    public bool RoleIsInterne { get; set; }
    public string? RoleHomePage { get; set; }
    public string? LangueId { get; set; }
    public string? LangueCode { get; set; }
    public ISet<string> DroitsCode { get; }
}