using RandomDataPicker.Contracts;

namespace RandomDataPicker.Core;

public record DefaultApplicationConfiguration : IApplicationConfiguration
{
    public string? Json { get; private set; }
    public string? JsonPath 
    { 
        set 
        {
            if (File.Exists(value))
            {
                Json = File.ReadAllText(value);
            }
        } 
    }
}
