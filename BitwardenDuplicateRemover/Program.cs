using System.Text.Json;
using BitwardenDuplicateRemover.Models;

if (args.Length != 1)
{
    Console.WriteLine("Usage: dotnet run <path to bitwarden export json>");
    return;
}

var filePath = args[0];
var newFilePath = filePath.Insert(filePath.LastIndexOf('.'), "_dedup");

await using var fileStream = new FileStream(filePath, FileMode.Open);
var json = await JsonSerializer.DeserializeAsync<BitwardenJsonExport>(fileStream, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});
if (json is null)
{
    Console.WriteLine("Could not deserialize json");
    return;
}

var visitedGroupNames = new HashSet<string>();
var groupNameToId = new Dictionary<string, string>();
var idToNewId = new Dictionary<string, string>();
var idToFolder = new Dictionary<string, BitwardenFolder>();

foreach (var (id, name) in json.Folders)
{
    if (visitedGroupNames.Contains(name))
    {
        idToNewId.Add(id, groupNameToId[name]);
    }
    else
    {
        visitedGroupNames.Add(name);
        groupNameToId.Add(name, id);
        idToNewId.Add(id, id);
        idToFolder.Add(id, new BitwardenFolder
        {
            Id = id,
            Name = name
        });
    }
}

var itemHashToNewItem = new Dictionary<string, BitwardenItem>();

foreach (var item in json.Items)
{
    var itemHash = ItemHash(item);
    if (itemHashToNewItem.TryGetValue(itemHash, out var newItem))
    {
        Merge(newItem, item);
        continue;
    }

    newItem = new BitwardenItem
    {
        Id = Guid.NewGuid().ToString(),
        OrganizationId = item.OrganizationId,
        FolderId = item.FolderId is null ? null : idToNewId[item.FolderId],
        Type = item.Type,
        Reprompt = item.Reprompt,
        Name = item.Name,
        Notes = item.Notes,
        Favorite = item.Favorite,
        Login = item.Login,
        Card = item.Card,
        SecureNote = item.SecureNote,
        CollectionIds = item.CollectionIds
    };

    itemHashToNewItem.Add(itemHash, newItem);
}


void Merge(BitwardenItem source, BitwardenItem target)
{
    if (target.Login?.Uris is not null && source.Type == 1)
    {
        source.Login ??= new BitwardenLogin();
        source.Login.Uris ??= new List<BitwardenUri>();
        foreach (var uri in target.Login.Uris.Where(uri => !source.Login.Uris.Contains(uri)))
        {
            source.Login.Uris.Add(uri);
        }
    }

    if (target.Favorite)
    {
        source.Favorite = true;
    }

    if (!string.IsNullOrEmpty(target.Notes))
    {
        if (!string.IsNullOrEmpty(source.Notes) && !source.Notes.Contains(target.Notes))
        {
            source.Notes += $"\n\n{target.Notes}";
        }
        else
        {
            source.Notes = target.Notes;
        }
    }
}

var newItems = itemHashToNewItem.Values.ToArray();
var options = new JsonSerializerOptions()
{
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
await using var newFileStream = new FileStream(newFilePath, FileMode.Create);
await JsonSerializer.SerializeAsync(newFileStream, new BitwardenJsonExport
{
    Folders = idToFolder.Values.ToArray(),
    Items = newItems
}, options);

// Hashes the item, ignoring the id and folder id
string ItemHash(BitwardenItem item)
{
    return
        $"{item.Name}-{item.Login?.Username ?? "x"}-{item.Login?.Password ?? "x"}-{item.SecureNote?.Type ?? 0}-{item.Card?.GetHashCode().ToString() ?? "x"}-{item.Type}-{item.Reprompt}";
}