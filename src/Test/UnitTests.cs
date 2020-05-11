using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using SuxrobGM_Website.Data;
using SuxrobGM_Website.Services;

namespace SuxrobGM_Website.Test
{
    public class UnitTests
    {
        [Fact]
        public async void TestEmailSender()
        {
            var emailSender = new EmailSender(GetAppConfiguration());
            await emailSender.SendEmailAsync("suxrobGM@gmail.com", "Unit Test", "Tested EmailSender() method");
        }

        [Fact]
        public void TestPopularTags()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>();
            dbOptions.UseSqlServer(GetAppConfiguration().GetConnectionString("RemoteConnection"))
                .UseLazyLoadingProxies();

            var context = new ApplicationDbContext(dbOptions.Options);

            var articles = context.Articles;
            var tags = new List<string>();

            foreach (var article in articles)
            {
                tags.AddRange(article.GetTags());
            }

            var popularTags = tags.GroupBy(str => str)
                .Select(i => new { Name = i.Key, Count = i.Count() })
                .OrderByDescending(k => k.Count);
        }

        private IConfigurationRoot GetAppConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }
    }
}
