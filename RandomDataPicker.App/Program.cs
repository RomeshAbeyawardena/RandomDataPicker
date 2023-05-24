// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using RandomDataPicker.App;
using RandomDataPicker.Core;

var services = new ServiceCollection()
    .RegisterServices("entries.json")
    .AddSingleton<EntryPicker>()
    .BuildServiceProvider();

var appEntryPicker = services.GetRequiredService<EntryPicker>();

appEntryPicker.Inject(new RandomDataPicker.Models.Entry {
    City = "Lincoln",
    Email = "romesh.a@outlook.com",
    IsFlagged = false,
    Name = "Romesh Abeyawardena"
}, 150 * 20);

var userEntry = new RandomDataPicker.Models.Entry { IsFlagged = true };

Console.Write("Name: ");
userEntry.Name = Console.ReadLine();
Console.Write("Email: ");
userEntry.Email = Console.ReadLine();
Console.Write("City: ");
userEntry.City = Console.ReadLine();
Console.Write("Tickets (default:3): ");
if (!int.TryParse(Console.ReadLine(), out var tickets))
{
    tickets = 3;
}

appEntryPicker.Inject(userEntry, 150 * tickets);


using var registration = new CancellationTokenSource();

var separater = new string('#', Console.WindowWidth);
var lineSpace = new string(' ', Console.WindowWidth / 2 - 24);
var lineSeparater = new string('-', Console.WindowWidth);
while (!registration.Token.IsCancellationRequested)
{
    Console.WriteLine($"🎫 Out of {appEntryPicker.EntryCount} entries, the winners are... 🏅");
    foreach (var entry in appEntryPicker.GetEntries(5))
    {
        if (entry.IsFlagged)
        {
            Console.WriteLine(separater);   
            Console.WriteLine($"{lineSpace}Congratulations you won!{lineSpace}");
            Console.WriteLine(separater);
        }
        Console.WriteLine(entry);
    }
    var choice = Console.ReadKey(true);

    if(choice.Key != ConsoleKey.Y || choice.Key == ConsoleKey.Escape)
    {
        registration.Cancel();
    }
    
    Console.WriteLine(lineSeparater);
}