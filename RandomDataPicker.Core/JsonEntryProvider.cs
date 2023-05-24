using RandomDataPicker.Contracts;
using RandomDataPicker.Models;
using System.Text.Json;

namespace RandomDataPicker.Core;

public sealed class JsonEntryProvider : IEntryProvider
{
    private readonly IApplicationConfiguration configuration;
    public JsonEntryProvider(IApplicationConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<Entry> GetEntries()
    {
        if (string.IsNullOrWhiteSpace(configuration.Json))
        {
            throw new NullReferenceException("Json must not be an empty string");
        }

        return JsonSerializer.Deserialize<List<Entry>>(configuration.Json, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new NullReferenceException("Unable to generate entries");
    }
}
