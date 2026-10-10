namespace SGM.WebApp.Services;

/// <summary>
/// FreeKassa payment proxy: validates signed redirects from meat.gg, builds the signed SCI checkout
/// URL, verifies FreeKassa notifications and relays verified confirmations back to meat.gg.
/// </summary>
public interface IFreeKassaService
{
    /// <summary>
    /// HMAC signature for a payment link. Mints links on suxrobgm.net and, via
    /// <see cref="VerifyProxyRedirect"/>, verifies links minted by meat.gg.
    /// </summary>
    string SignProxyRedirect(string order, string amount, string currency, string ret);

    /// <summary>
    /// <paramref name="amount"/> and <paramref name="ret"/> must be the raw (decoded) query values,
    /// exactly as meat.gg signed them.
    /// </summary>
    bool VerifyProxyRedirect(string order, string amount, string currency, string ret, string sign);

    /// <summary>Constant-time check of the passphrase that unlocks the <c>/pay/new</c> link generator.</summary>
    bool IsAdminKeyValid(string? key);

    string BuildCheckoutUrl(string order, string amount, string currency, string returnUrl);

    bool VerifyNotification(string merchantId, string amount, string orderId, string sign);

    Task<bool> RelayToMeatAsync(string orderId, string amount, string externalId, CancellationToken ct = default);
}
