using System.Windows.Markup;
using BookingApp.Data;
using BookingApp.DTOs;
using BookingApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// удаление и апдейт

namespace BookingApp.Controllers {
    [ApiController]
    [Route("service")]
    public class ServicesController : ControllerBase {

        private readonly BookingContext dbContext;

        public ServicesController(BookingContext dbContext) {
            this.dbContext = dbontext;
        }

        [HttpGet]
        public IActionResult GetAllServices() {
            var services = dbContext.Services.ToList();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceById(int id) {
            var service = dbContext.Services.FirstOrDefault(s => s.Id == id);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceDto serviceDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var service = new Service {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                Price = serviceDto.Price
            };

            dbContext.Services.Add(service);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceById), new {id = service.Id}, service);
        }
    }
}