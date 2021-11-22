using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace LandingPage.Pages
{
    [Authorize, AuthorizeForScopes(Scopes = new[] { "AppRoleAssignment.ReadWrite.All" })]
    public class AppRoleAssignmentModelBase
    {
    }
}