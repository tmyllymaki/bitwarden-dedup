using System.Text.Json.Serialization;

namespace BitwardenDuplicateRemover.Models;

public class BitwardenLogin
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BitwardenUri>? Uris { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string? Totp { get; set; }
}