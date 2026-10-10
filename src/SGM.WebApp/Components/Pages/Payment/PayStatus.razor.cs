using Microsoft.AspNetCore.Components;

namespace SGM.WebApp.Components.Pages.Payment;

public partial class PayStatus
{
    [SupplyParameterFromQuery(Name = "status")] public string? Status { get; set; }
    [SupplyParameterFromQuery(Name = "us_ret")] public string? Ret { get; set; }

    private bool IsSuccess => string.Equals(Status, "success", StringComparison.OrdinalIgnoreCase);

    private string returnUrl = "https://suxrobgm.net";
    private string returnLabel = "Back to suxrobgm.net";

    // Open-redirect guard: follow us_ret only when it's an https url on one of our domains.
    // meat.gg payments round-trip a meat.gg url; links minted on suxrobgm.net round-trip a suxrobgm.net url.
    protected override void OnParametersSet()
    {
        if (!Uri.TryCreate(Ret, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps)
        {
            return;
        }

        if (IsHost(uri, "meat.gg"))
        {
            returnUrl = uri.ToString();
            returnLabel = "Back to meat.gg";
        }
        else if (IsHost(uri, "suxrobgm.net"))
        {
            returnUrl = uri.ToString();
        }
    }

    // Matches the apex host and any subdomain (".meat.gg" suffix), but not "notmeat.gg".
    private static bool IsHost(Uri uri, string host) =>
        $".{uri.Host}".EndsWith($".{host}", StringComparison.OrdinalIgnoreCase);
}
