using BookingApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingApp.DTOs;
using BookingApp.Models;

//написать delete-операцию!

namespace BookingApp.Controllers {
    [ApiController]
    [Route("api/booking")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingContext dbContext;

        public BookingsController(BookingContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetBookings() {
            var bookings = dbContext.Bookings.ToList();
            return Ok(bookings);
        }

        [HttpGet("filter")]
        public IActionResult GetBookingsByFilter(string? userName, DateTime? checkInDate, string? status)
        {
            var query = dbContext.Bookings
                                .Include(b => b.User) 
                                .AsQueryable();

            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(b => b.User.Name.Contains(userName));
            }

            if (checkInDate.HasValue)
            {
                query = query.Where(b => b.CheckInDate.Date == checkInDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }

            var filteredBookings = query.ToList();
            return Ok(filteredBookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookingById(int id) {
            var booking = dbContext.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Cabin)
                            .FirstOrDefaultAsync(b => b.Id == id);
                            
            if (booking == null) return NotFound();
            
            return Ok(booking); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingByIdAsync(int id) {
            var booking = await dbContext.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Cabin)
                            .FirstOrDefaultAsync(b => b.Id == id);
                            
            if (booking == null) return NotFound();
            
            return Ok(booking); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDto bookingDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var booking = new Booking {
                UserId = bookingDto.UserId,
                CabinId = bookingDto.CabinId,
                CheckInDate = bookingDto.CheckInDate,
                CheckOutDate = bookingDto.CheckOutDate,
                Status = bookingDto.Status,
            };

            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookingAsync(int id, [FromBody] BookingCreateDto bookingDto) {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var booking = await dbContext.Bookings
                                .Include(b => b.User)
                                .Include(b => b.Cabin)
                                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            booking.UserId = bookingDto.UserId;
            booking.CabinId = bookingDto.CabinId;
            booking.CheckInDate = bookingDto.CheckInDate;
            booking.CheckOutDate = bookingDto.CheckOutDate;
            booking.Status = bookingDto.Status;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}