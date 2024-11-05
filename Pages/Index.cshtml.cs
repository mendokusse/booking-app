using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Data;

namespace BookingApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BookingContext dbContext;

        public IndexModel(BookingContext dbContext) {
            this.dbContext = dbContext;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
