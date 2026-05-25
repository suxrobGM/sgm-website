using Microsoft.AspNetCore.Mvc;
using SGM.WebApp.Services;

namespace SGM.WebApp.Controllers;

/// <summary>
/// Endpoints for the FreeKassa payment proxy. suxrobgm.net is the registered FreeKassa merchant
/// and relays verified confirmations to meat.gg.
/// </summary>
[ApiController]
[Route("api/freekassa")]
public sealed class FreeKassaController(IFreeKassaService freeKassa) : ControllerBase
{
    /// <summary>
    /// FreeKassa notification (webhook) handler. Verifies the FreeKassa signature, relays a
    /// signed confirmation to meat.gg, and answers "YES" so FreeKassa marks the order paid.
    /// Accepts both GET and POST since the notification method is configurable in the dashboard.
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
        var externalId = Field(form, "intid") ?? string.Empty;

        if (merchantId is null || amount is null || orderId is null || sign is null)
        {
            return Content("NO");
        }

        var isSignatureValid = freeKassa.VerifyNotification(merchantId, amount, orderId, sign);
        if (!isSignatureValid)
        {
            return Content("NO");
        }

        await freeKassa.RelayToMeatAsync(orderId, amount, externalId);
        return Content("YES");
    }

    /// <summary>Reads a field from the posted form, falling back to the query string.</summary>
    private string? Field(IFormCollection? form, string key)
    {
        if (form is not null && form.TryGetValue(key, out var formValue))
        {
            return formValue.ToString();
        }

        return Request.Query.TryGetValue(key, out var queryValue) ? queryValue.ToString() : null;
    }
}
