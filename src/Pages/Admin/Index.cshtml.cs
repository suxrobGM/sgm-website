using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.ServerAnalytics.Models;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;

namespace SuxrobGM_Website.Pages.Admin
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class AdminIndexModel : PageModel
    {
        private SqliteDbContext _context;
        public List<Student> DataSouce { get; set; }

        public void OnGet()
        {
            //_context = new SqliteDbContext("Data Source = app_analytics.sqlite");
            //var dataSource = new List<WebTraffic>(_context.Traffics);
            
            DataSouce = new List<Student>()
            {
                new Student() { Name = "Suxrob", Age = 21 },
                new Student() { Name = "Sitora", Age = 20 }
            };
            //ViewData.Add("dataSource", dataSource);
        }
    }
}