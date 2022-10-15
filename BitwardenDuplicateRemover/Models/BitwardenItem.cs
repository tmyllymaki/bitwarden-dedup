using System.Text.Json.Serialization;

namespace BitwardenDuplicateRemover.Models;

public class BitwardenItem
{
    public string Id { get; set; }
    public string? OrganizationId { get; set; }
    public string? FolderId { get; set; }
    public int Type { get; set; }
    public int Reprompt { get; set; }
    public string Name { get; set; }
    public string? Notes { get; set; }
    public bool Favorite { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BitwardenLogin? Login { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BitwardenCard? Card { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BitwardenSecureNote? SecureNote { get; set; }

    public string[]? CollectionIds { get; set; }
}