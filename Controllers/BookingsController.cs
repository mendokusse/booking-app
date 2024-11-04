using BookingApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Controllers {
    [ApiController]
    [Route("api/booking")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingContext dbContext;

        public BookingsController(BookingContext dbContext) {
            this.dbContext = dbContext;
        }

        
    }
}