using RandomDataPicker.Contracts;
using RandomDataPicker.Extensions;
using RandomDataPicker.Models;

namespace RandomDataPicker.Core;

public sealed class DefaultEntryInjector : IEntryInjector
{
    private readonly Random random;

    public DefaultEntryInjector(Random random)
    {
        this.random = random;
    }

    public IEnumerable<Entry> InjectEntries(IEnumerable<Entry> entries, IEnumerable<Entry> additionalEntries, bool rejectDuplicates = true)
    {
        return rejectDuplicates 
            ? entries.Union(additionalEntries)
            : entries.Concat(additionalEntries);
    }

    public IEnumerable<Entry> InjectEntry(IEnumerable<Entry> entries, Entry entry, int numberofTimes, bool injectRandomly = true)
    {
        var repeatedEntries = new Entry[numberofTimes];
        Array.Fill(repeatedEntries, entry);

        if (!injectRandomly)
        {
            return InjectEntries(entries, repeatedEntries, false);
        }

        var entryList = entries.ToList();
        foreach (var e in repeatedEntries)
        {
            var c = e.ShallowCopy() ?? throw new NullReferenceException();
            var index = random.Next(0, entryList.Count - 1);

            while (index > 0 && (entryList.ElementAt(index - 1) == entry 
                || entryList.ElementAt(index + 1) == entry))
            {
                index = random.Next(0, entryList.Count - 1);
            }

            c.Id = index + 1;

            entryList.Insert(index, c);
        }

        return entryList;
    }
}
