namespace Options
{
    public record MSGraphOptions
    {
        public const string MSGraphSettings = nameof(MSGraphSettings);

        public string? BaseUrl { get; init; }
        public string? Scopes { get; init; }
    }
}
