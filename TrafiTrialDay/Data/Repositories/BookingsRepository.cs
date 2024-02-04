using System.Linq;
using TrafiTrialDay.Data.Entities;

namespace TrafiTrialDay.Data.Repositories
{
    public interface IBookingsRepository
    {
        IEnumerable<Booking> GetAll();
        Booking GetById(string bookingId);
        Booking Create(string carId);
        void Finish(Booking booking);
    }

    public class BookingsRepository : IBookingsRepository
    {
        private readonly List<Booking> _bookings = new List<Booking>();

        private readonly ICarsRepository _carsRepository;

        public BookingsRepository(ICarsRepository carsRepository)
        {
            _carsRepository = carsRepository;
        }

        public IEnumerable<Booking> GetAll()
        {
            return _bookings;
        }

        public Booking GetById(string bookingId)
        {
            return _bookings.FirstOrDefault(x => x.Id == bookingId);
        }

        public Booking Create(string carId)
        {
            var carById = _carsRepository.GetById(carId);

            var booking = new Booking
            {
                Id = carId,
                Status = BookingStatus.Started,
                Car = carById,
                StartedAt = DateTime.UtcNow
            };

            int carToBookIndex = _carsRepository.GetAll().IndexOf(carById);
            _carsRepository.GetAll().RemoveAt(carToBookIndex);

            _bookings.Add(booking);

            return booking;
        }

        public void Finish(Booking booking)
        {
            _carsRepository.GetAll().Add(booking.Car);

            var index = _bookings.IndexOf(booking);
            _bookings[index] = booking;
        }
    }
}