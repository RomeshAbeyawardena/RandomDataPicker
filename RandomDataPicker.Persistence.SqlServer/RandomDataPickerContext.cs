using Microsoft.EntityFrameworkCore;

namespace RandomDataPicker.Persistence.SqlServer;
public class RandomDataPickerContext : DbContext
{
    public RandomDataPickerContext(DbContextOptions options)
        : base(options)
    {
        
    }

    public DbSet<Models.Entry> Entries { get; set; }
}
