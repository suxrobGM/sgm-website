namespace SGM.WebApp.Options;

/// <summary>
/// Configuration for the FreeKassa payment proxy. suxrobgm.net is the registered
/// FreeKassa merchant (meat.gg cannot verify its own antibot-protected domain), so it
/// owns both secret words. <see cref="ProxySecret"/> is a separate shared HMAC key used
/// only for the meat.gg &lt;-&gt; suxrobgm.net hop.
/// </summary>
public record FreeKassaOptions
{
    /// <summary>FreeKassa merchant/shop id (the <c>m</c> form parameter).</summary>
    public required string MerchantId { get; init; }

    /// <summary>Secret word 1 — used to sign the outgoing SCI payment form.</summary>
    public required string Secret1 { get; init; }

    /// <summary>Secret word 2 — used to verify the incoming notification (webhook).</summary>
    public required string Secret2 { get; init; }

    /// <summary>Shared HMAC secret for authenticating the meat.gg &lt;-&gt; suxrobgm.net hop.</summary>
    public required string ProxySecret { get; init; }

    /// <summary>meat.gg endpoint that receives the relayed, verified payment confirmation.</summary>
    public required string MeatggCallbackUrl { get; init; }

    /// <summary>FreeKassa hosted checkout base URL.</summary>
    public string PayUrl { get; init; } = "https://pay.fk.money/";

    /// <summary>Passphrase that unlocks the <c>/pay/new</c> link generator (admin only).</summary>
    public required string AdminKey { get; init; }
}
