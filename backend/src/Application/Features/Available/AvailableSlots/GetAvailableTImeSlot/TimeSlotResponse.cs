namespace Application.Features.Available.AvailableSlots.GetAvailableTImeSlot;

public sealed class TimeSlotResponse
{
    public string StartTime { get; init; } = default!; // ISO string
    public string EndTime { get; init; } = default!;
    public bool IsAvailable { get; init; }
}
