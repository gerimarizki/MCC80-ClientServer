using API.Contracts;
using API.DTOs.Bookings;
using API.Models;

namespace API.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IEmployeeRepository employeeRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
            _roomRepository = roomRepository;
        }

        public IEnumerable<GetBookingDto>? GetBooking()
        {
            var bookings = _bookingRepository.GetAll();
            if (!bookings.Any())
            {
                return null; // No Booking  found
            }
            var toDto = bookings.Select(booking =>
                                                new GetBookingDto
                                                {
                                                    Guid = booking.Guid,
                                                    StartDate = booking.StartDate,
                                                    EndDate = booking.EndDate,
                                                    Status = booking.Status,
                                                    Remarks = booking.Remarks,
                                                    RoomGuid = booking.RoomGuid,
                                                    EmployeeGuid = booking.EmployeeGuid
                                                }).ToList();

            return toDto; // Booking found
        }

        public GetBookingDto? GetBooking(Guid guid)
        {
            var booking = _bookingRepository.GetByGuid(guid);
            if (booking is null)
            {
                return null; // booking not found
            }

            var toDto = new GetBookingDto
            {
                Guid = booking.Guid,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Remarks = booking.Remarks,
                RoomGuid = booking.RoomGuid,
                EmployeeGuid = booking.EmployeeGuid
            };
            return toDto; // bookings found
        }

        public GetBookingDto? CreateBooking(NewBookingDto newBookingDto)
        {
            var booking = new Booking
            {
                Guid = new Guid(),
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdBooking = _bookingRepository.Create(booking);
            if (createdBooking is null)
            {
                return null; // Booking not created
            }

            var toDto = new GetBookingDto
            {
                Guid = createdBooking.Guid,
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
            };
            return toDto; // Booking created
        }

        public int UpdateBooking(UpdateBookingDto updateBookingDto)
        {
            var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

            var booking = new Booking
            {
                Guid = updateBookingDto.Guid,
                StartDate = updateBookingDto.StartDate,
                EndDate = updateBookingDto.EndDate,
                Status = updateBookingDto.Status,
                Remarks = updateBookingDto.Remarks,
                RoomGuid = updateBookingDto.RoomGuid,
                EmployeeGuid = updateBookingDto.EmployeeGuid,
                ModifiedDate = DateTime.Now,
                CreatedDate = getBooking!.CreatedDate
            };

            var isUpdate = _bookingRepository.Update(booking);
            if (!isUpdate)
            {
                return 0; // Booking not updated
            }

            return 1;
        }

        public int DeleteBooking(Guid guid)
        {
            var isExist = _bookingRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var booking = _bookingRepository.GetByGuid(guid);
            var isDelete = _bookingRepository.Delete(booking!);
            if (!isDelete)
            {
                return 0; // Booking not deleted
            }

            return 1;
        }

        public DetailBookingDto? GetDetailByGuid(Guid bookingGuid)
        {
            var resultBooking = _bookingRepository.GetByGuid(bookingGuid);
            if (resultBooking is null)
            {
                return null; // Return null if the booking with the given Guid is not found
            }

            var resultEmployee = _employeeRepository.GetByGuid(resultBooking.EmployeeGuid);
            if (resultEmployee is null)
            {
                return null; // Return null if the associated employee is not found
            }

            var resultRoom = _roomRepository.GetByGuid(resultBooking.RoomGuid);
            if (resultRoom is null)
            {
                return null; // Return null if the associated room is not found
            }

            var detailDto = new DetailBookingDto
            {
                BookingGuid = resultBooking.Guid,
                BookedNik = resultEmployee.NIK,
                BookedBy = resultEmployee.FirstName + " " + resultEmployee.LastName,
                RoomName = resultRoom.Name,
                StartDate = resultBooking.StartDate,
                EndDate = resultBooking.EndDate,
                Status = resultBooking.Status,
                Remarks = resultBooking.Remarks
            };

            return detailDto;
        }


        public IEnumerable<DetailBookingDto> GetALl()
        {
            var resultBooking = _bookingRepository.GetAll();
            if (!resultBooking.Any())
            {
                return Enumerable.Empty<DetailBookingDto>();
            }

            var detailDtos = new List<DetailBookingDto>();
            foreach (var result in resultBooking)
            {
                var resultEmployee = _employeeRepository.GetByGuid(result.EmployeeGuid);
                if (resultEmployee is null)
                {
                    return Enumerable.Empty<DetailBookingDto>();
                }

                var resultRoom = _roomRepository.GetByGuid(result.RoomGuid);
                if (resultRoom is null)
                {
                    return Enumerable.Empty<DetailBookingDto>();
                }

                var toDto = new DetailBookingDto
                {
                    BookingGuid = result.Guid,
                    BookedNik = resultEmployee.NIK,
                    BookedBy = resultEmployee.FirstName + " " + resultEmployee.LastName,
                    RoomName = resultRoom.Name,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    Status = result.Status,
                    Remarks = result.Remarks
                };

                detailDtos.Add(toDto);

            }

            return detailDtos;
        }
    }
}
