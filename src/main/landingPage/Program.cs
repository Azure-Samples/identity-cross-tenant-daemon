using Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var app = BuildWebApp();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


WebApplication BuildWebApp()
{
    const string settingsFile = "appsettings.Development.json";

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    if (!File.Exists(settingsFile))
    {
        throw new Exception($"Please create and correctly fill out the file: {settingsFile}. See the readme for details.");
    }

    // Using the Options for settings: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0
    builder.Services.Configure<AzureAdOptions>(configuration.GetSection(AzureAdOptions.AzureAdAppRegistration));
    builder.Services.Configure<MSGraphOptions>(configuration.GetSection(MSGraphOptions.MSGraphSettings));
    builder.Services.Configure<DaemonPermissionOptions>(configuration.GetSection(DaemonPermissionOptions.DaemonPermissions));

    string[]? initialScopes = configuration.GetValue<string>($"{MSGraphOptions.MSGraphSettings}:Scopes")?.Split(' ');

    // https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/2-WebApp-graph-user/2-3-Multi-Tenant
    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApp(configuration.GetSection(AzureAdOptions.AzureAdAppRegistration))
                    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                    .AddMicrosoftGraph(configuration.GetSection(MSGraphOptions.MSGraphSettings))
                    .AddInMemoryTokenCaches();

    builder.Services.AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });

    builder.Services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy
        options.FallbackPolicy = options.DefaultPolicy;

    });

    // Add services to the container.
    builder.Services.AddRazorPages();

    return builder.Build();
}
