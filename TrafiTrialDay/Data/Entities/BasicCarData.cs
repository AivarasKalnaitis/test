namespace TrafiTrialDay.Data.Entities
{
    public sealed class BasicCarData
    {
        public string Id { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public int Seats { get; set; }

        public string Plate { get; set; }

        public string Color { get; set; }

        public bool ChildSeat { get; set; }

        public Engine Engine { get; set; }

        public Tracker Tracker { get; set; }

        public int RangeFullKm { get; set; }
    }

    public sealed class Engine
    {
        public FuelType Fuel { get; set; }

        public TransmissionType Transmission { get; set; }
    }

    public sealed class Tracker
    {
        public string Phone { get; set; }

        public string Imei { get; set; }
    }

    public enum FuelType
    {
        Petrol,
        Diesel,
        Electric
    }

    public enum TransmissionType
    {
        Automatic,
        Manual
    }
}
