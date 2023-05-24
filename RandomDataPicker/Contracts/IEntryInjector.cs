using RandomDataPicker.Models;

namespace RandomDataPicker.Contracts;

public interface IEntryInjector
{
    IEnumerable<Entry> InjectEntry(IEnumerable<Entry> entries, Entry entry, int numberofTimes, bool injectRandomly = true);
    IEnumerable<Entry> InjectEntries(IEnumerable<Entry> entries, IEnumerable<Entry> newEntries, bool rejectDuplicates = true);
}
