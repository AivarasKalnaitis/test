using Microsoft.AspNetCore.Mvc;
using TrafiTrialDay.AwsSqs;
using TrafiTrialDay.Data.Entities;
using TrafiTrialDay.Data.Repositories;

namespace TrafiTrialDay.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAWSSQSService _AWSSQSService;
        private readonly ICarsRepository _carsRepository;

        public CarController(ICarsRepository carsRepository, IAWSSQSService aWSSQSService, IConfiguration configuration)
        {
            _carsRepository = carsRepository;
            _AWSSQSService = aWSSQSService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task <IEnumerable<RequiredCarData>> GetAvailableCars()
        {
            var availableCars = new List<RequiredCarData>();
            var basicCarsData = _carsRepository.GetAll();
            var devicesState = await GetDevicesState();

            foreach (var basicData in basicCarsData)
            {
                var carAsDevice = devicesState.FirstOrDefault(x => x.Imei == basicData.Tracker.Imei);

                if (carAsDevice != null)
                {
                    availableCars.Add(new RequiredCarData(basicData, carAsDevice));
                }
            }

            return availableCars;
        }

        private async Task<List<DeviceState>> GetDevicesState()
        {
            string devicesStateAddress = _configuration.GetValue<string>("AWSSQS:DeviceStateQueueAddress");
            return await _AWSSQSService.GetDevicesState(devicesStateAddress);
        }
    }
}