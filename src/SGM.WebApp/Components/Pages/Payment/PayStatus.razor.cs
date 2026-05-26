using Microsoft.AspNetCore.Components;

namespace SGM.WebApp.Components.Pages.Payment;

public partial class PayStatus
{
    [SupplyParameterFromQuery(Name = "status")] public string? Status { get; set; }
    [SupplyParameterFromQuery(Name = "us_ret")] public string? Ret { get; set; }

    private bool IsSuccess => string.Equals(Status, "success", StringComparison.OrdinalIgnoreCase);

    private string ReturnUrl = "https://suxrobgm.net";
    private string ReturnLabel = "Back to suxrobgm.net";

    protected override void OnParametersSet()
    {
        var target = ResolveReturnTarget();
        if (target is not null)
        {
            ReturnUrl = target.ToString();
            ReturnLabel = IsHost(target, "meat.gg") ? "Back to meat.gg" : "Back to suxrobgm.net";
        }
    }

    // Open-redirect guard: follow us_ret only when it's an https url on one of our domains,
    // otherwise fall back to the homepage. meat.gg payments round-trip a meat.gg url; generic
    // links minted on suxrobgm.net round-trip a suxrobgm.net url.
    private Uri? ResolveReturnTarget()
    {
        if (Uri.TryCreate(Ret, UriKind.Absolute, out var uri) &&
            uri.Scheme == Uri.UriSchemeHttps &&
            (IsHost(uri, "meat.gg") || IsHost(uri, "suxrobgm.net")))
        {
            return uri;
        }

        return null;
    }

    // Matches the apex host and any subdomain (".meat.gg" suffix), but not "notmeat.gg".
    private static bool IsHost(Uri uri, string host) =>
        $".{uri.Host}".EndsWith($".{host}", StringComparison.OrdinalIgnoreCase);
}
