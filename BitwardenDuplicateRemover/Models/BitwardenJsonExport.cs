namespace BitwardenDuplicateRemover.Models;

public class BitwardenJsonExport
{
    public bool Encrypted { get; set; }
    public BitwardenFolder[] Folders { get; set; }
    public BitwardenItem[] Items { get; set; }
}