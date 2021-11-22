using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Options;

namespace LandingPage.Pages
{
    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { "User.Read", "AppRoleAssignment.ReadWrite.All", "Application.Read.All" })]
    public class WelcomeModel : PageModel
    {
        private readonly IOptions<AzureAdOptions> _azureAdOptions;
        private readonly IOptions<DaemonPermissionOptions> _daemonPermissions;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string? TenantId { get; private set; } = "<Something went wrong.>";

        [BindProperty]
        public string? Permission { get; private set; } = "empty";

        public WelcomeModel(
            IOptions<AzureAdOptions> azureAdOptions, 
            IOptions<DaemonPermissionOptions> daemonPermissionOptions,
            ILogger<IndexModel> logger)
        {
            _azureAdOptions = azureAdOptions;
            _daemonPermissions = daemonPermissionOptions;
            _logger = logger;
        }


        public async Task<IActionResult> OnGet()
        {
            TenantId = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            Permission = _daemonPermissions.Value.Scopes;

            return Page();
        }

        public IActionResult OnPostAppRoleAssignment()
            => RedirectToPage($"/{nameof(AppRoleAssignmentModel).Replace("Model", string.Empty)}");
    }
}
