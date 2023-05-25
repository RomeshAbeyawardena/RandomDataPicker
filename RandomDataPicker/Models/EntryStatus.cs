namespace RandomDataPicker.Models;

[MessagePack.MessagePackObject(true)]
public record EntryStatus
{
    public bool IsLoaded { get; set; }
    public int? TotalNumberOfEntries { get; set; }
    public bool IsPopulated { get; set; }
}
