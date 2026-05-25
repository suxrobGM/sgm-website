using Microsoft.AspNetCore.Components;

namespace SGM.WebApp.Components.Pages;

public partial class PayStatus
{
    [SupplyParameterFromQuery(Name = "status")] public string? Status { get; set; }
    [SupplyParameterFromQuery(Name = "us_ret")] public string? Ret { get; set; }

    private bool IsSuccess => string.Equals(Status, "success", StringComparison.OrdinalIgnoreCase);

    // Open-redirect guard: only return to a meat.gg https URL, otherwise fall back to the homepage.
    private string ReturnUrl =>
        Uri.TryCreate(Ret, UriKind.Absolute, out var uri) &&
        uri.Scheme == Uri.UriSchemeHttps &&
        (uri.Host == "meat.gg" || uri.Host.EndsWith(".meat.gg", StringComparison.OrdinalIgnoreCase))
            ? uri.ToString()
            : "https://meat.gg";
}
