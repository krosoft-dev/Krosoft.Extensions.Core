namespace Krosoft.Extensions.Core.Models;

public record KrosoftToken
{
    public KrosoftToken()
    {
        Permissions = new HashSet<string>();
        TenantsId = new HashSet<string>();
        Properties = new Dictionary<string, object>();
    }

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? RoleId { get; set; }
    public string? LangueId { get; set; }
    public string? LangueCode { get; set; }
    public ISet<string> Permissions { get; }
    public ISet<string> TenantsId { get; }
    public IDictionary<string, object> Properties { get; }

    public void AddProperty(string key, object value)
    {
        Properties[key] = value;
    }

    public T? GetProperty<T>(string key)
    {
        if (Properties.TryGetValue(key, out var value))
        {
            if (value is T typedValue)
            {
                return typedValue;
            }
        }

        return default;
    }
}