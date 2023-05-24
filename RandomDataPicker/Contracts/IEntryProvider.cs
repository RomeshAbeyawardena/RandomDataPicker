using RandomDataPicker.Models;

namespace RandomDataPicker.Contracts;

public interface IEntryProvider
{
    IEnumerable<Entry> GetEntries();
}
