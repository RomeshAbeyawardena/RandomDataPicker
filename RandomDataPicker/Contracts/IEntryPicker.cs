using RandomDataPicker.Models;

namespace RandomDataPicker.Contracts;

public interface IEntryPicker
{
    IEnumerable<Entry> PickEntries(IEnumerable<Entry> entries, int numberOfEntries);
}
