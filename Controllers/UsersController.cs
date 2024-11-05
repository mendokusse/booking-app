using Microsoft.AspNetCore.Mvc;
using BookingApp.Data;
using Microsoft.AspNetCore.Authorization;
using BookingApp.DTOs;
using BookingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Controllers {
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase {
        private readonly BookingContext dbContext;
        private readonly PasswordHasher<User> passwordHasher;

        public UsersController(BookingContext dbContext) {
            this.dbContext = dbContext;
            passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet("clients")]
        [Authorize(Roles = "Administrator,Employee")]
        public IActionResult GetClients() {
            var customers = dbContext.Users
                        .Where(u => u.Role.Name.Equals("Client")).ToList();

            return Ok(customers);
        }

        [HttpGet("employees")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetEmployees() {
            var employees = dbContext.Users.Where(u => u.Role.Name == "Employee").ToList();

        return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id) {
            var userId = User.FindFirst("sub")?.Value; 
            if (userId == null) return Unauthorized();

            var user = await dbContext.Users
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user == null) return NotFound();

            return Ok(new { user.Name, user.Email, Role = user.Role.Name });
        }

        [HttpGet("{id}/bookings")]
        [Authorize]
        public async Task<IActionResult> GetUserBookingsAsync(int id) {
            var user = await dbContext.Users
                .Include(u => u.Bookings) 
                .ThenInclude(b => b.Cabin) 
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {
                return NotFound("Пользователь не найден.");
            }

            return Ok(user.Bookings);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto) {
            if (dbContext.Users.Any(u => u.Email == userDto.Email)) return BadRequest("Пользователь с таким e-mail уже существует.");

            var user = new User {
                Name = userDto.Name,
                Email = userDto.Email,
                RoleId = userDto.RoleId,
            };

            user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return Ok("Пользователь успешно зарегистрирован");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto) {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null) return Unauthorized("Пользователь не найден");

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed) return Unauthorized("Неверный пароль.");

            return Ok("Успешный вход.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserDto userDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await dbContext.Users
                                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var existingUser = await dbContext.Users
                                .FirstOrDefaultAsync(u => u.Email == userDto.Email && u.Id != id);

            if (existingUser != null) return BadRequest("Пользователь с таким email уже существует.");

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.RoleId = userDto.RoleId;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePasswordAsync(int id, [FromBody] ChangePasswordDto passwordDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordDto.OldPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Failed) {
                return BadRequest("Неправильный старый пароль.");
            }

            user.PasswordHash = passwordHasher.HashPassword(user, passwordDto.NewPassword);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(int id) {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null) return NotFound();

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}