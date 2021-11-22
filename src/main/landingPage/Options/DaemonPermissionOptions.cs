namespace Options
{
    public record DaemonPermissionOptions
    {
        public const string DaemonPermissions = nameof(DaemonPermissions);

        public string? Scopes { get; init; }
    }
}
