using AbbyRazor.DAL;
using AbbyRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyRazor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }

    
        public async Task<IActionResult> OnPost()
        {
          await  _db.Categories.AddAsync(Category);
          await  _db.SaveChangesAsync();
            return RedirectToPage("Index");

        }
    }
}
