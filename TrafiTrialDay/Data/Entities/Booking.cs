namespace TrafiTrialDay.Data.Entities
{
    public sealed class Booking
    {
        public string Id { get; set; }

        public BookingStatus Status { get; set; }

        public BasicCarData Car { get; set; }

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset? FinishedAt { get; set; }
    }

    public enum BookingStatus
    {
        Started,
        Finished
    }
}