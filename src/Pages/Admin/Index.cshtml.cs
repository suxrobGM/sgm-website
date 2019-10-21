using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.ServerAnalytics.Models;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;

namespace SuxrobGM_Website.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminIndexModel : PageModel
    {
        private SqliteDbContext _context;

        public void OnGet()
        {
            _context = new SqliteDbContext("Data Source = app_analytics.sqlite");
            var dataSource = new List<WebTraffic>(_context.Traffics);
            
            ViewData.Add("dataSource", dataSource);
        }
    }
}