using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Pages.Payment;

public partial class PayNew
{
    [Inject] private IFreeKassaService FreeKassa { get; set; } = null!;
    [Inject] private NavigationManager Nav { get; set; } = null!;
    [Inject] private IJSRuntime Js { get; set; } = null!;

    private static readonly string[] Currencies = ["RUB", "USD", "EUR", "KZT"];

    private string adminKey = string.Empty;
    private bool unlocked;
    private bool keyError;

    private string amount = string.Empty;
    private string currency = "RUB";
    private string description = string.Empty;

    private string? generatedLink;
    private string? formError;
    private string copyLabel = "Copy link";

    private void Unlock()
    {
        keyError = !FreeKassa.IsAdminKeyValid(adminKey);
        unlocked = !keyError;
    }

    private void Generate()
    {
        formError = null;
        generatedLink = null;
        copyLabel = "Copy link";

        // Re-check the key: the circuit holds it server-side, but never trust the unlocked flag alone.
        if (!FreeKassa.IsAdminKeyValid(adminKey))
        {
            unlocked = false;
            return;
        }

        if (!decimal.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var value) ||
            value <= 0)
        {
            formError = "Enter a valid amount greater than zero.";
            return;
        }

        // The signed amount string must match exactly what /pay/freekassa receives back (decoded).
        var normalizedAmount = value.ToString("0.##", CultureInfo.InvariantCulture);
        var order = "gen-" + Guid.NewGuid().ToString("N")[..12];
        var ret = Nav.BaseUri; // suxrobgm.net root → status page shows a neutral thank-you
        var sign = FreeKassa.SignProxyRedirect(order, normalizedAmount, currency, ret);

        var query = new Dictionary<string, object?>
        {
            ["order"] = order,
            ["amount"] = normalizedAmount,
            ["currency"] = currency,
            ["desc"] = description,
            ["ret"] = ret,
            ["sign"] = sign,
        };

        generatedLink = Nav.GetUriWithQueryParameters($"{Nav.BaseUri}pay/freekassa", query);
    }

    private async Task CopyLink()
    {
        if (string.IsNullOrEmpty(generatedLink))
        {
            return;
        }

        await Js.InvokeVoidAsync("navigator.clipboard.writeText", generatedLink);
        copyLabel = "Copied!";
    }
}
