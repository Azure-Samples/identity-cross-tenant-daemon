namespace Options
{
    public record AzureAdOptions
    {
        public const string AzureAdAppRegistration = nameof(AzureAdAppRegistration);

        public string? Instance { get; init; }
        public string? TenantId { get; init; }
        public Guid ClientId { get; init; }
        public string? ClientSecret { get; init; }
        public string? Audience { get; init; }
        public string? Authority { get; init; }
        public string? Scopes { get; init; }
        public string? ValidIssuers { get; init; }
        public string? CallbackPath { get; init; }
    }
}
