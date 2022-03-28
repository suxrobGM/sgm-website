using Newtonsoft.Json.Linq;

namespace SGM.Application.Services;

internal class GoogleRecaptchaService : ICaptchaService
{
    private const string apiEndpoint = "https://www.google.com/recaptcha/api/siteverify";
    private readonly GoogleRecaptchaOptions _options;
    private readonly HttpClient _httpClient;

    public GoogleRecaptchaService(GoogleRecaptchaOptions options)
    {
        if (string.IsNullOrEmpty(options.SecretKey))
            throw new ArgumentException("Secret key is an empty string");

        _options = options;
        _httpClient = new HttpClient();
    }

    public async Task<bool> VerifyCaptchaAsync(string captchaValue)
    {
        var postQueries = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("secret", _options.SecretKey!),
            new KeyValuePair<string, string>("response", captchaValue)
        };

        var response = await _httpClient.PostAsync(new Uri(apiEndpoint), new FormUrlEncodedContent(postQueries));
        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonData = JObject.Parse(responseContent);

        if (bool.TryParse(jsonData["success"]?.ToString(), out var value))
        {
            return value;
        }
        else
        {
            return false;
        }
    }
}
