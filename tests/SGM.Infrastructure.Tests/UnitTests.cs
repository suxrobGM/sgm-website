using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;
using SGM.Infrastructure.Services;

namespace SGM.Infrastructure.Tests
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
        //public void TestPopularTags()
        //{
        //    var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>();
        //    dbOptions.UseSqlServer(GetAppConfiguration().GetConnectionString("RemoteConnection"))
        //        .UseLazyLoadingProxies();

        //    var context = new ApplicationDbContext(dbOptions.Options);

        //    var articles = context;
        //    var tags = new List<string>();

        //    foreach (var article in articles)
        //    {
        //        tags.AddRange(article.GetTags());
        //    }

        //    var popularTags = tags.GroupBy(str => str)
        //        .Select(i => new { Name = i.Key, Count = i.Count() })
        //        .OrderByDescending(k => k.Count);
        //}

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
