namespace TrafiTrialDay.Data.Entities
{
    public class RequiredCarData
    {
        public BasicCarData BasicCarData { get; set; }
        public decimal RangeRemainingKm { get; set; }

        public decimal FuelRemainingFraction { get; set; }

        public LatLng Position { get; set; }

        public sealed class LatLng
        {
            public double Lat { get; set; }

            public double Lng { get; set; }
        }

        public RequiredCarData(BasicCarData basicCarData, DeviceState deviceState)
        {
            BasicCarData = basicCarData;
            FuelRemainingFraction = deviceState.FuelRemainingFraction;
            RangeRemainingKm = basicCarData.RangeFullKm * FuelRemainingFraction;
            Position = new LatLng { Lat = deviceState.Position.Lat, Lng = deviceState.Position.Lng };
        }
    }
}