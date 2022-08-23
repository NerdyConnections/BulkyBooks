using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            //ROuting in asp.net razor pages maps the Pages folder
            ///Index.cshtml maps to /Pages/Index.cshtml
            //Index.cshtml

            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}