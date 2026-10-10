using Microsoft.AspNetCore.Mvc;
using SGM.WebApp.Services;

namespace SGM.WebApp.Controllers;

/// <summary>
/// suxrobgm.net is the registered FreeKassa merchant and relays verified confirmations to meat.gg.
/// </summary>
[ApiController]
[Route("api/freekassa")]
public sealed class FreeKassaController(
    IFreeKassaService freeKassa,
    ILogger<FreeKassaController> logger) : ControllerBase
{
    /// <summary>
    /// FreeKassa notification webhook. Answering "YES" marks the order paid. Accepts GET and POST
    /// because the notification method is configurable in the FreeKassa dashboard.
    /// </summary>
    [HttpGet("notify")]
    [HttpPost("notify")]
    public async Task<IActionResult> Notify()
    {
        var form = Request.HasFormContentType ? await Request.ReadFormAsync() : null;

        var merchantId = Field(form, "MERCHANT_ID");
        var amount = Field(form, "AMOUNT");
        var orderId = Field(form, "MERCHANT_ORDER_ID");
        var sign = Field(form, "SIGN");
        var externalId = Field(form, "intid");

        // A missing field reads as "" and fails the signature check.
        if (!freeKassa.VerifyNotification(merchantId, amount, orderId, sign))
        {
            return Content("NO");
        }

        // Links minted on suxrobgm.net itself ("gen-" prefix) have no meat.gg order to relay to.
        // meat.gg order ids are numeric, so the prefix can never collide.
        if (orderId.StartsWith("gen-", StringComparison.Ordinal))
        {
            logger.LogInformation("FreeKassa notify: generic link paid (order {Order})", orderId);
        }
        else
        {
            await freeKassa.RelayToMeatAsync(orderId, amount, externalId);
        }

        return Content("YES");
    }

    /// <summary>Reads a field from the posted form, falling back to the query string.</summary>
    private string Field(IFormCollection? form, string key)
    {
        if (form is not null && form.TryGetValue(key, out var formValue))
        {
            return formValue.ToString();
        }

        return Request.Query.TryGetValue(key, out var queryValue) ? queryValue.ToString() : "";
    }
}
