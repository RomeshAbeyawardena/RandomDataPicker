using RandomDataPicker.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomDataPicker.Persistence.Models;

[Table(TABLE_NAME)]
public record Entry : IIdentity
{
    public const string TABLE_NAME = "Entry";
    [Key]
    public Guid Id { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public string? Email { get; set; }
    [Required] public string? City { get; set; }
    public DateTimeOffset Created { get; set; }
}
