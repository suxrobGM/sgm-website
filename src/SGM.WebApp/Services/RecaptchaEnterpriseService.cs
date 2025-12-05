using Google.Apis.Auth.OAuth2;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.Options;
using SGM.WebApp.Options;
using Event = Google.Cloud.RecaptchaEnterprise.V1.Event;

namespace SGM.WebApp.Services;

public sealed class RecaptchaEnterpriseService : ICaptchaService
{
    private readonly RecaptchaEnterpriseServiceClient _client;

    private readonly string _projectId;
    private readonly string _siteKey;

    public RecaptchaEnterpriseService(IOptions<GoogleRecaptchaOptions> options)
    {
        _projectId = options.Value.ProjectId;
        _siteKey = options.Value.SiteKey;
        _client = new RecaptchaEnterpriseServiceClientBuilder
        {
            Credential = GoogleCredential.FromFile(options.Value.KeyPath)
        }.Build();
    }

    public async Task<bool> VerifyCaptchaAsync(string token)
    {
        var assessment = new Assessment
        {
            Event = new Event
            {
                SiteKey = _siteKey,
                Token = token
            }
        };

        var response = await _client.CreateAssessmentAsync(new CreateAssessmentRequest
        {
            Parent = $"projects/{_projectId}",
            Assessment = assessment
        });

        // Check that Google accepted the token
        if (!response.TokenProperties.Valid) return false;

        // Make sure it was generated for this action
        if (response.TokenProperties.Action != "contact") return false;

        // Decide to use the risk score (0.0-1.0). 0.1-0.3 ≈ likely bot.
        return response.RiskAnalysis.Score >= 0.5;
    }
}
