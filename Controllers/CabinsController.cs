using BookingApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingApp.DTOs;
using BookingApp.Models;

namespace BookingApp.Controllers {
    [ApiController]
    [Route("cabin")]
    public class CabinController : ControllerBase {

        private readonly BookingContext dbContext;

        public CabinController(BookingContext context) {
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult GetAllCabins() {
            var cabins = dbContext.Cabins.ToList();
            return Ok(cabins);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCabinByIdAsync(int id) {
            var cabin = await dbContext.Cabins
                        .FirstOrDefaultAsync(c => c.Id == id);

            if(cabin == null) return NotFound();

            return Ok(cabin);
        }

        [HttpGet("avaliable")]
        public IActionResult GetAllAvaliableCabins(DateTime CheckInDate, DateTime CheckOutDate) {
            var bookings = dbContext.Bookings
                                .Where(b => (b.CheckInDate < CheckOutDate && b.CheckOutDate > CheckInDate)) 
                                .Select(b => b.CabinId) 
                                .ToList();

            var availableCabins = dbContext.Cabins
                                     .Where(c => !bookings.Contains(c.Id)) 
                                    .ToList();

            return Ok(availableCabins);
        }

        [HttpGet("filter")]
        public IActionResult GetCabinsByFilter(string? name, int? capacity, decimal? maxPrice)
        {
            var query = dbContext.Cabins.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (capacity.HasValue)
            {
                query = query.Where(c => c.Capacity >= capacity.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerNight <= maxPrice.Value);
            }

            var filteredCabins = query.ToList();
            return Ok(filteredCabins);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCabin([FromBody] CabinDto cabinDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cabin = new Cabin {
                Name = cabinDto.Name,
                ShortDescription = cabinDto.ShortDescription,
                LongDescription = cabinDto.LongDescription,
                Capacity = cabinDto.Capacity,
                PricePerNight = cabinDto.PricePerNight
            };

            dbContext.Cabins.Add(cabin);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCabinByIdAsync), new {id = cabin.Id}, cabin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCabinAsync(int id, [FromBody] CabinDto cabinDto) {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var cabin = await dbContext.Cabins
                            .FirstOrDefaultAsync(c => c.Id == id);

            if (cabin == null) return NotFound();

            cabin.Name = cabinDto.Name;
            cabin.ShortDescription = cabinDto.ShortDescription;
            cabin.LongDescription = cabinDto.LongDescription;
            cabin.Capacity = cabinDto.Capacity;
            cabin.PricePerNight = cabinDto.PricePerNight;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCabinByIdAsync(int id) {
            var cabin = await dbContext.Cabins.FindAsync(id);

            if (cabin == null) return NotFound();

            dbContext.Cabins.Remove(cabin);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpGet("{id}/photos")]
        public async Task<IActionResult> GetAllPhotosByCabinAsync(int id) {

            var cabinExists = await dbContext.Cabins.AnyAsync(c => c.Id == id);
            if (!cabinExists) {
                return NotFound();
            }

            var photos = await dbContext.CabinPhotos
                                .Where(p => p.CabinId == id)
                                .ToListAsync();

            return Ok(photos);
        }

        [HttpDelete("photos/{id}")]
        public async Task<IActionResult> DeleteCabinPhotoByIdAsync(int id) {
            var photo = await dbContext.CabinPhotos.FindAsync(id);

            if(photo == null) return NotFound();

            dbContext.CabinPhotos.Remove(photo);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}