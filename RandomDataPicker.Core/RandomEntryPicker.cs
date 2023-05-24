using RandomDataPicker.Contracts;
using RandomDataPicker.Models;

namespace RandomDataPicker.Core;
public sealed class RandomEntryPicker : IEntryPicker
{
    private readonly Random random;
    public RandomEntryPicker(Random random)
    {
        this.random = random;
    }
    public IEnumerable<Entry> PickEntries(IEnumerable<Entry> entries, int numberOfEntries)
    {
        var entryList = new List<Entry>();
        var lastPickedIndexes = new List<int>();
        while(entryList.Count < numberOfEntries)
        {
            var index = random.Next(0, entries.Count() - 1);

            var entry = entries.ElementAt(index);
            if (lastPickedIndexes.Contains(index) 
                || entryList.Any(e => e.LogicallyEquals(entry)))
            {
                continue;
            }

            lastPickedIndexes.Add(index);
            entryList.Add(entry);
        }

        return entryList;
    }
}
