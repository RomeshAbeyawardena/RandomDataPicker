using RandomDataPicker.Contracts;
using RandomDataPicker.Models;

namespace RandomDataPicker.App;

public sealed class EntryPicker
{
    private readonly IEntryProvider entryProvider;
    private readonly IEntryPicker entryPicker;
    private readonly IEntryInjector entryInjector;
    private readonly Random random;
    private IEnumerable<Entry> entries = Array.Empty<Entry>(); 

    /// <summary>
    /// Initialises the entry data and enumerates the IDs
    /// </summary>
    private void Initialise()
    {
        entries = entryProvider.GetEntries();
        int ct = 0;
        foreach(var entry in entries)
        {
            entry.Id = ct++;
        }

        foreach (var entry in entries.ToArray())
        {
            entries = entryInjector.InjectEntry(entries, entry, random.Next(150, 150 * 3));
        }
    }

    public EntryPicker(IEntryProvider entryProvider, IEntryPicker entryPicker, IEntryInjector entryInjector, Random random)
    {
        this.entryProvider = entryProvider;
        this.entryPicker = entryPicker;
        this.entryInjector = entryInjector;
        this.random = random;
        Initialise();
    }

    public int EntryCount => entries.Count();
    
    /// <summary>
    /// Injects the entry a specified number of times
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="numberOfTimes"></param>
    public void Inject(Entry entry, int numberOfTimes)
    {
        entries = entryInjector.InjectEntry(entries, entry, numberOfTimes);
    }

    /// <summary>
    /// Gets entries from source
    /// </summary>
    /// <param name="numberOfEntries"></param>
    /// <returns></returns>
    public IEnumerable<Entry> GetEntries(int numberOfEntries)
    {
        return entryPicker.PickEntries(entries, numberOfEntries);
    }
}
