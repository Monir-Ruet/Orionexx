using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orionexx.Core.Shared.Enums.Events;

namespace Orionexx.Core.Shared.Entities.Events;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string EventType { get; set; }

    [Required]
    public string Payload { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public required string Destination { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }

    public int Attempts { get; set; }
        
    public string Status { get; set; } = nameof(EventStatus.Pending);

    public string? ErrorMessage { get; set; }
}