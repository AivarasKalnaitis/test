namespace TrafiTrialDay.Data.Entities
{
    public sealed class DeviceCommand
    {
        public string Imei { get; set; }

        public CommandType Command { get; set; }

        public DeviceCommand(string imei, CommandType command)
        {
            Imei = imei;
            Command = command;
        }
    }

    public enum CommandType
    {
        Lock,
        Unlock
    }
}