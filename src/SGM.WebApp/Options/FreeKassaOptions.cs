namespace SGM.WebApp.Options;

/// <summary>
/// suxrobgm.net is the registered FreeKassa merchant (meat.gg cannot verify its own antibot-protected
/// domain), so it owns both secret words. <see cref="ProxySecret"/> is a separate HMAC key used only
/// for the meat.gg &lt;-&gt; suxrobgm.net hop.
/// </summary>
public record FreeKassaOptions
{
    /// <summary>The <c>m</c> form parameter.</summary>
    public required string MerchantId { get; init; }

    /// <summary>Signs the outgoing SCI payment form.</summary>
    public required string Secret1 { get; init; }

    /// <summary>Verifies the incoming notification (webhook).</summary>
    public required string Secret2 { get; init; }

    public required string ProxySecret { get; init; }

    /// <summary>meat.gg endpoint that receives the relayed, verified payment confirmation.</summary>
    public required string MeatggCallbackUrl { get; init; }

    public string PayUrl { get; init; } = "https://pay.fk.money/";

    /// <summary>Passphrase that unlocks the <c>/pay/new</c> link generator.</summary>
    public required string AdminKey { get; init; }
}
