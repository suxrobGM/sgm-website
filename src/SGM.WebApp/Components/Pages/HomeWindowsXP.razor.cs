using Microsoft.JSInterop;

namespace SGM.WebApp.Components.Pages;

public partial class HomeWindowsXP
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("playXpStartupSound");
            await JS.InvokeVoidAsync("startXpClock");
        }
    }
}
