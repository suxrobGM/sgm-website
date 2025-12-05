namespace SGM.WebApp.Options;

public record GoogleRecaptchaOptions
{
    public required string SiteKey { get; init; }
    public required string ProjectId { get; init; }
    public required string KeyPath { get; init; }
}
