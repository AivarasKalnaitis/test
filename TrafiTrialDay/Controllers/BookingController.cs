using Microsoft.AspNetCore.Mvc;
using System.Net;
using TrafiTrialDay.AwsSqs;
using TrafiTrialDay.Data.Entities;
using TrafiTrialDay.Data.Repositories;

namespace TrafiTrialDay.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly string deviceCommandQueueAddress;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly ICarsRepository _carsRepository;
        private readonly IConfiguration _configuration;
        private readonly IAWSSQSService _AWSSQSService;

        public BookingController(IBookingsRepository bookingsRepository, ICarsRepository carsRepository, IConfiguration configuration, IAWSSQSService aWSSQSService)
        {
            _configuration = configuration;
            deviceCommandQueueAddress = _configuration.GetValue<string>("AWSSQS:DeviceCommandsQueueAddress");

            _bookingsRepository = bookingsRepository;
            _carsRepository = carsRepository;
            _AWSSQSService = aWSSQSService;
        }

        [HttpPost]
        [Route("{carId}")]
        public async Task<ActionResult<Booking>> Create(string carId)
        {
            // kaip handlint kai daug contorllers ir noi ta pacia message, taip pat turetu handlint unexpected -
                //.aspnet middlaware http request per handlerius leiodzia ir galima savo idet
                //ARBA helper class kuri priima funkcija kuri grazina taska

            var car = _carsRepository.GetAll().FirstOrDefault(x => x.Id == carId);

            if (car is null)
            {
                return NotFound($"Car with ID {carId} not found");
            }

            var booking = _bookingsRepository.Create(carId);

            var unlockCarCommand = new DeviceCommand(car.Tracker.Imei, CommandType.Unlock);
            var sendResult = await _AWSSQSService.SendDeviceCommand(unlockCarCommand, deviceCommandQueueAddress);

            if(sendResult.HttpStatusCode != HttpStatusCode.OK)
            {
                return BadRequest($"Failed to send to queue. StatusCode: {sendResult.HttpStatusCode}");
            }

            return Ok(booking);
        }

        [HttpPost]
        [Route("{bookingId}/finish")]
        public async Task<ActionResult<Booking>> Finish(string bookingId)
        {
            var booking = _bookingsRepository.GetById(bookingId);

            if(booking is null)
            {
                return NotFound($"Booking with ID {bookingId} not found");
            }

            booking.FinishedAt = DateTime.Now;
            booking.Status = BookingStatus.Finished;

            var lockCarCommand = new DeviceCommand(booking.Car.Tracker.Imei, CommandType.Lock);
            var sendResult = await _AWSSQSService.SendDeviceCommand(lockCarCommand, deviceCommandQueueAddress);

            if (sendResult.HttpStatusCode != HttpStatusCode.OK)
            {
                return BadRequest($"Failed to send to queue. StatusCode: {sendResult.HttpStatusCode}");
            }

            _bookingsRepository.Finish(booking);

            return Ok(booking);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Booking>> GetBookings()
        {
            return Ok(_bookingsRepository.GetAll());
        }
    }
}