using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;
using SGM.Application.Services;
using SGM.Application.Options;

namespace SGM.IntegrationTests;

public class EmailSenderTest
{
    [Fact]
    public async void SendMailTest()
    {
        var options = GetAppConfiguration().GetSection("EmailConfig").Get<EmailSenderOptions>();
        var emailSender = new EmailSender(options, null);
        await emailSender.SendMailAsync("suxrobGM@gmail.com", "Test", "Test email");
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
