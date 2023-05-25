using RandomDataPicker.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomDataPicker.Persistence.Models;

[Table(TABLE_NAME)]
[MessagePack.MessagePackObject(true)]
public record Entry : IIdentity, ICreated
{
    public const string TABLE_NAME = "Entry";
    [Key]
    public Guid Id { get; set; }
    public int CId { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public string? Email { get; set; }
    [Required] public string? City { get; set; }
    public bool IsFlagged { get; set; }
    public DateTimeOffset Created { get; set; }
}
