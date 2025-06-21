using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;

namespace SGM.BlogApp.Pages;

[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminIndexModel : PageModel
{
    private readonly SqliteDbContext _context;

    public AdminIndexModel(SqliteDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        var dataSource = _context.Traffics;
        
        ViewData.Add("dataSource", dataSource);

        return Page();
    }

    public async Task<IActionResult> OnGetRemoveItemsAsync()
    {
        var items = _context.Traffics;
        _context.Traffics.RemoveRange(items);
        
        await _context.SaveChangesAsync();
        return Page();
    }
}
