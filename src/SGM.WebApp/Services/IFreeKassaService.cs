namespace SGM.WebApp.Services;

/// <summary>
/// Handles the FreeKassa payment proxy: validates signed redirects coming from meat.gg,
/// builds the signed FreeKassa SCI checkout URL, verifies FreeKassa notifications, and
/// relays verified confirmations back to meat.gg.
/// </summary>
public interface IFreeKassaService
{
    /// <summary>
    /// Verifies the HMAC signature on a redirect from meat.gg. <paramref name="amount"/> and
    /// <paramref name="ret"/> must be the raw (decoded) query values, exactly as meat.gg signed them.
    /// </summary>
    bool VerifyProxyRedirect(string order, string amount, string currency, string ret, string sign);

    /// <summary>Builds the FreeKassa hosted-checkout URL (with the SCI <c>s</c> signature).</summary>
    string BuildCheckoutUrl(string order, string amount, string currency, string returnUrl);

    /// <summary>Verifies a FreeKassa notification <c>SIGN</c> (md5 with secret word 2).</summary>
    bool VerifyNotification(string merchantId, string amount, string orderId, string sign);

    /// <summary>Whether the request originated from a known FreeKassa notification IP.</summary>
    bool IsFreeKassaIp(string? ip);

    /// <summary>Relays a verified payment confirmation to meat.gg's callback API (signed).</summary>
    Task<bool> RelayToMeatAsync(string orderId, string amount, string externalId, CancellationToken ct = default);
}
