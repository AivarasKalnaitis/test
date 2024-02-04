using Amazon.SQS.Model;
using Amazon.SQS;
using Newtonsoft.Json;
using TrafiTrialDay.Data.Entities;
using System.Net;

namespace TrafiTrialDay.AwsSqs
{
    public interface IAWSSQSHelper
    {
        Task<List<Message>> GetDeviceState(string queueAddress);
        Task<SendMessageResponse> SendDeviceCommand(DeviceCommand deviceCommand, string queueAddress);
    }

    public class AWSSQSHelper : IAWSSQSHelper
    {
        private readonly IAmazonSQS _sqs;

        public AWSSQSHelper(IAmazonSQS sqs)
        {
            _sqs = sqs;
        }

        public async Task<List<Message>> GetDeviceState(string queueAddress)
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = queueAddress,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5
                };

                var result = await _sqs.ReceiveMessageAsync(request);

                return result.Messages.Any() ? result.Messages : new List<Message>();
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
                string message = JsonConvert.SerializeObject(deviceCommand);
                var sendRequest = new SendMessageRequest
                {
                    QueueUrl = queueAddress,
                    MessageBody = message,
                    MessageGroupId = deviceCommand.Imei,
                    MessageDeduplicationId = Guid.NewGuid().ToString(),
                };

                return await _sqs.SendMessageAsync(sendRequest);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}