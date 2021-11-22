using Azure.Identity;
using DaemonApp.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Options;

// .NET 6 - See https://aka.ms/new-console-template for more information

var builder = new ConfigurationBuilder()
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json", optional: false)
    .AddJsonFile("daemonAppSettings.json", optional: false);

var configuration = builder.Build();

var deamonAppSettings = configuration
    .GetSection(DaemonAppOptions.DaemonAppSettings);

var azureAppRegistration = configuration
    .GetSection(AzureAdOptions.AzureAdAppRegistration);

var tenantId = deamonAppSettings.GetSection(nameof(DaemonAppOptions.TenantId)).Value;
var clientId = azureAppRegistration.GetSection(nameof(AzureAdOptions.ClientId)).Value;
var secret = azureAppRegistration.GetSection(nameof(AzureAdOptions.ClientSecret)).Value;

// #1 let's first get the access token so that we can examine it.

var accessToken = await GetAccessToken();
Console.WriteLine($"Access Token (Decode token using https://jwt.ms)\r\n{accessToken}\r\n");

// #2 Let's use Microsoft Graph to get data.
Console.WriteLine($"List all users in tenant (requires the 'User.Read.All' Application Permission):");
await WriteAllUserNamesInTenantFromMSGraph();

Console.ReadLine();


// https://docs.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS#using-a-client-secret
async Task WriteAllUserNamesInTenantFromMSGraph()
{
    
    var options = new TokenCredentialOptions
    {
        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
    };
        
    var clientSecretCredentials = new ClientSecretCredential(tenantId, clientId, secret, options);

    var graphClient = new GraphServiceClient(clientSecretCredentials);

    var allUsers = await graphClient.Users.Request().GetAsync();

    foreach (var user in allUsers)
    {
        Console.WriteLine(user.DisplayName);
    }
}

async Task<string> GetAccessToken()
{
    // https://docs.microsoft.com/en-us/graph/auth-v2-service#4-get-an-access-token
    var scopes = new string[] { $"https://graph.microsoft.com/.default", };

    var app = ConfidentialClientApplicationBuilder.Create(clientId)
        .WithClientSecret(secret)
        .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
        .WithTenantId(tenantId)
        .Build();

    var authenticationResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

    return authenticationResult.AccessToken;
}
