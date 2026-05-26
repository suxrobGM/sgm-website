using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using SGM.WebApp.Options;
using SGM.WebApp.Utils;

namespace SGM.WebApp.Services;

public sealed class FreeKassaService(
    IOptions<FreeKassaOptions> options,
    HttpClient httpClient,
    ILogger<FreeKassaService> logger)
    : IFreeKassaService
{
    private readonly FreeKassaOptions _options = options.Value;

    // FreeKassa notification source IPs (https://docs.freekassa.net).
    private static readonly HashSet<string> FreeKassaIps =
    [
        "168.119.157.136",
        "168.119.60.227",
        "178.154.197.79",
        "51.250.54.238",
    ];

    public string SignProxyRedirect(string order, string amount, string currency, string ret)
        => CryptoUtils.HmacSha256Hex($"{order}:{amount}:{currency}:{ret}", _options.ProxySecret);

    public bool VerifyProxyRedirect(string order, string amount, string currency, string ret, string sign)
        => CryptoUtils.FixedTimeEqualsHex(SignProxyRedirect(order, amount, currency, ret), sign);

    public bool IsAdminKeyValid(string? key) => CryptoUtils.FixedTimeEquals(key, _options.AdminKey);

    public string BuildCheckoutUrl(string order, string amount, string currency, string returnUrl)
    {
        // FreeKassa SCI form signature: md5("merchantId:amount:secret1:currency:order").
        var sign = CryptoUtils.Md5Hex($"{_options.MerchantId}:{amount}:{_options.Secret1}:{currency}:{order}");

        var sb = new StringBuilder(_options.PayUrl);
        sb.Append(_options.PayUrl.Contains('?') ? '&' : '?');
        sb.Append("m=").Append(Uri.EscapeDataString(_options.MerchantId));
        sb.Append("&oa=").Append(Uri.EscapeDataString(amount));
        sb.Append("&currency=").Append(Uri.EscapeDataString(currency));
        sb.Append("&o=").Append(Uri.EscapeDataString(order));
        sb.Append("&s=").Append(sign);
        // Custom param round-trips through FreeKassa to the success/fail redirect.
        sb.Append("&us_ret=").Append(Uri.EscapeDataString(returnUrl));
        return sb.ToString();
    }

    public bool VerifyNotification(string merchantId, string amount, string orderId, string sign)
    {
        if (!string.Equals(merchantId, _options.MerchantId, StringComparison.Ordinal))
        {
            logger.LogWarning("FreeKassa notify: merchant id mismatch ('{MerchantId}')", merchantId);
            return false;
        }

        // FreeKassa notification signature: md5("merchantId:amount:secret2:orderId").
        var expected = CryptoUtils.Md5Hex($"{merchantId}:{amount}:{_options.Secret2}:{orderId}");
        return CryptoUtils.FixedTimeEqualsHex(expected, sign);
    }

    public bool IsFreeKassaIp(string? ip) => ip is not null && FreeKassaIps.Contains(ip);

    public async Task<bool> RelayToMeatAsync(
        string orderId, string amount, string externalId, CancellationToken ct = default)
    {
        var sign = CryptoUtils.HmacSha256Hex($"{orderId}:{amount}:{externalId}", _options.ProxySecret);
        var payload = new { orderId, amount, externalId, sign };

        try
        {
            var response = await httpClient.PostAsJsonAsync(_options.MeatggCallbackUrl, payload, ct);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "FreeKassa relay to meat.gg failed ({Status}) for order {Order}",
                    response.StatusCode, orderId);
                return false;
            }

            logger.LogInformation("FreeKassa relay to meat.gg succeeded for order {Order}", orderId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "FreeKassa relay to meat.gg threw for order {Order}", orderId);
            return false;
        }
    }
}
