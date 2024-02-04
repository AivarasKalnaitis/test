using Amazon.SQS.Model;
using Newtonsoft.Json;
using System.Net;
using TrafiTrialDay.Data.Entities;

namespace TrafiTrialDay.AwsSqs
{
    public interface IAWSSQSService
    {
        Task<List<DeviceState>> GetDevicesState(string queueAddress);
        Task<SendMessageResponse> SendDeviceCommand(DeviceCommand deviceCommand, string queueAddress);
    }
    public class AWSSQSService : IAWSSQSService
    {
        private readonly IAWSSQSHelper _AWSSQSHelper;

        public AWSSQSService(IAWSSQSHelper AWSSQSHelper)
        {
            _AWSSQSHelper = AWSSQSHelper;
        }

        public async Task<List<DeviceState>> GetDevicesState(string queueAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(queueAddress))
                {
                    throw new ArgumentNullException(nameof(queueAddress));
                }

                var devicesState = new List<DeviceState>();

                for (int i = 0; i < 10; i++)         // background worker, kai task sukas zinutes vis skaito
                {

                    List<Message> messages = await _AWSSQSHelper.GetDeviceState(queueAddress);

                    devicesState.AddRange(messages.Select(x => JsonConvert.DeserializeObject<DeviceState>(x.Body)).ToList()); // jei failina kai kurios - per jas foreachint ir turet viduj jo try/catch, jei kyla expeciton handlinam, jei tvarkoj det i srarsa 
                }

        

                return devicesState;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SendMessageResponse> SendDeviceCommand(DeviceCommand deviceCommand, string queueAddress)
        {
            try
            {
                return await _AWSSQSHelper.SendDeviceCommand(deviceCommand, queueAddress);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
