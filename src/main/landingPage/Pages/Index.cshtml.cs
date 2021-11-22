using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LandingPage.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(
            ILogger<IndexModel> logger)
        {
            _logger = logger;

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostSignUp() 
            => RedirectToPage($"/{nameof(WelcomeModel).Replace("Model", string.Empty)}");
    }
}