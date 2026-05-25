using Microsoft.AspNetCore.Components;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Pages;

public partial class Pay
{
    [Inject] private IFreeKassaService FreeKassa { get; set; } = null!;

    [SupplyParameterFromQuery(Name = "order")] public string? Order { get; set; }
    [SupplyParameterFromQuery(Name = "amount")] public string? Amount { get; set; }
    [SupplyParameterFromQuery(Name = "currency")] public string? Currency { get; set; }
    [SupplyParameterFromQuery(Name = "desc")] public string? Desc { get; set; }
    [SupplyParameterFromQuery(Name = "ret")] public string? Ret { get; set; }
    [SupplyParameterFromQuery(Name = "sign")] public string? Sign { get; set; }

    private bool valid;
    private string checkoutUrl = string.Empty;

    protected override void OnInitialized()
    {
        var hasRequiredParams = !string.IsNullOrEmpty(Order) && !string.IsNullOrEmpty(Amount) &&
            !string.IsNullOrEmpty(Currency) && !string.IsNullOrEmpty(Ret) && !string.IsNullOrEmpty(Sign);

        if (!hasRequiredParams)
        {
            valid = false;
            return;
        }

        valid = FreeKassa.VerifyProxyRedirect(Order!, Amount!, Currency!, Ret!, Sign!);
        if (valid)
        {
            checkoutUrl = FreeKassa.BuildCheckoutUrl(Order!, Amount!, Currency!, Ret!);
        }
    }
}
