using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using SGM.WebApp.Options;

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

    public bool VerifyProxyRedirect(string order, string amount, string currency, string ret, string sign)
    {
        var expected = HmacSha256Hex($"{order}:{amount}:{currency}:{ret}", _options.ProxySecret);
        return FixedTimeEqualsHex(expected, sign);
    }

    public string BuildCheckoutUrl(string order, string amount, string currency, string returnUrl)
    {
        // FreeKassa SCI form signature: md5("merchantId:amount:secret1:currency:order").
        var sign = Md5Hex($"{_options.MerchantId}:{amount}:{_options.Secret1}:{currency}:{order}");

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
        var expected = Md5Hex($"{merchantId}:{amount}:{_options.Secret2}:{orderId}");
        return FixedTimeEqualsHex(expected, sign);
    }

    public bool IsFreeKassaIp(string? ip) => ip is not null && FreeKassaIps.Contains(ip);

    public async Task<bool> RelayToMeatAsync(
        string orderId, string amount, string externalId, CancellationToken ct = default)
    {
        var sign = HmacSha256Hex($"{orderId}:{amount}:{externalId}", _options.ProxySecret);
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

    private static string Md5Hex(string input)
        => Convert.ToHexStringLower(MD5.HashData(Encoding.UTF8.GetBytes(input)));

    private static string HmacSha256Hex(string input, string key)
        => Convert.ToHexStringLower(
            HMACSHA256.HashData(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(input)));

    private static bool FixedTimeEqualsHex(string expected, string actual)
    {
        if (string.IsNullOrEmpty(actual) || expected.Length != actual.Length)
        {
            return false;
        }

        var a = Encoding.ASCII.GetBytes(expected.ToLowerInvariant());
        var b = Encoding.ASCII.GetBytes(actual.ToLowerInvariant());
        return CryptographicOperations.FixedTimeEquals(a, b);
    }
}
