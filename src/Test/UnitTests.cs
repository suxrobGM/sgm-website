using System.IO;
using Xunit;
using Microsoft.Extensions.Configuration;
using SuxrobGM_Website.Services;

namespace SuxrobGM_Website.Test
{
    public class UnitTests
    {
        [Fact]
        public async void TestEmailSender()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var emailSender = new EmailSender(configuration);
            await emailSender.SendEmailAsync("suxrobGM@gmail.com", "Unit Test", "Tested EmailSender() method");
        }
    }
}
