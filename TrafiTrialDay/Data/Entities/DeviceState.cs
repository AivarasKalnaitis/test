namespace TrafiTrialDay.Data.Entities
{
    public sealed class DeviceState
    {
        public string Imei { get; set; }

        public int MileageTotalKm { get; set; }

        public decimal FuelRemainingFraction { get; set; }

        public bool Ignition { get; set; }

        public bool Locked { get; set; }

        public LatLng Position { get; set; }
    }

    public struct LatLng
    {
        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}