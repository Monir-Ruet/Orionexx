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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }

    public int? RetryCount { get; set; }

    public string Status { get; set; } = nameof(EventStatus.Pending);

    public string? Error { get; set; }

    public void MarkAsProcessed()
    {
        Status = EventStatus.Processed.ToString();
        ProcessedAt = DateTime.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        Status = EventStatus.Failed.ToString();
        Error = error;
        RetryCount++;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkForRetry()
    {
        Status = EventStatus.Pending.ToString();
        Error = null;
        RetryCount??= 0;
        RetryCount++;
    }
}