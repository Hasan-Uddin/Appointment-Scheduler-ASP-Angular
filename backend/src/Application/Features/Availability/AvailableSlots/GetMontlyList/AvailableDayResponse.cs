namespace Application.Features.Availability.AvailableSlots.GetMontlyList;

public sealed class AvailableDayResponse
{
    public string Date { get; init; } = default!; // yyyy-MM-dd
    public bool HasSlots { get; init; }
    public int SlotsCount { get; init; }
}
