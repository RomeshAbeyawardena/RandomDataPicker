namespace RandomDataPicker.Models;

[MessagePack.MessagePackObject(true)]
public record Entry
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public bool IsFlagged { get; set; }
    public bool LogicallyEquals(Entry entry)
    {
        return entry != null &&
            !string.IsNullOrWhiteSpace(entry.Name) && entry.Name.Equals(Name) &&
            !string.IsNullOrWhiteSpace(entry.Email) && entry.Email.Equals(Email)
            && !string.IsNullOrWhiteSpace(entry.City) && entry.City.Equals(City);
    }
}
