namespace BitwardenDuplicateRemover.Models;

public class BitwardenFolder
{
    public string Id { get; set; }
    public string Name { get; set; }

    public void Deconstruct(out string id, out string name)
    {
        id = Id;
        name = Name;
    }
}