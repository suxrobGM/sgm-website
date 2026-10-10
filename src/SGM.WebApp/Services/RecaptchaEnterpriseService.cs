using Google.Apis.Auth.OAuth2;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.Options;
using SGM.WebApp.Options;
using Event = Google.Cloud.RecaptchaEnterprise.V1.Event;

namespace SGM.WebApp.Services;

public sealed class RecaptchaEnterpriseService(IOptions<GoogleRecaptchaOptions> options) : ICaptchaService
{
    private readonly RecaptchaEnterpriseServiceClient _client = new RecaptchaEnterpriseServiceClientBuilder
    {
        Credential = CredentialFactory.FromFile<ServiceAccountCredential>(options.Value.KeyPath)
            .ToGoogleCredential()
    }.Build();

    private readonly string _projectId = options.Value.ProjectId;
    private readonly string _siteKey = options.Value.SiteKey;

    public async Task<bool> VerifyCaptchaAsync(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var response = await _client.CreateAssessmentAsync(new CreateAssessmentRequest
        {
            Parent = $"projects/{_projectId}",
            Assessment = new Assessment { Event = new Event { SiteKey = _siteKey, Token = token } }
        });

        // The token must be valid and issued for the contact form's action. Scores of 0.1-0.3 are likely bots.
        return response.TokenProperties.Valid &&
               response.TokenProperties.Action == "contact" &&
               response.RiskAnalysis.Score >= 0.5;
    }
}
