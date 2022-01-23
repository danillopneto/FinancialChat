using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Jobsity.Challenge.FinancialChat.Core.Infra.Gateways
{
    public abstract class BaseGateway<T>
    {
        private readonly ILogger<T> _logger;

        protected abstract string BaseClient { get; }

        protected virtual string Client { get; set; }

        protected IHttpClientFactory HttpClientFactory { get; }

        protected virtual JsonSerializerSettings JsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
            }
        }

        protected BaseGateway(
                              IHttpClientFactory httpClientFactory,
                              ILogger<T> logger)
        {
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendPostRequest<TRequest>(
                                                    string url,
                                                    TRequest body)
        {
            var responseContent = string.Empty;

            try
            {
                using var client = GetClient();
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var clientResponse = await client.PostAsync(url, content);
                responseContent = await clientResponse.Content.ReadAsStringAsync();
                clientResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to reach {Url}: {ResponseContent}", url, responseContent);
            }
        }

        protected HttpClient GetClient()
        {
            return HttpClientFactory.CreateClient(BaseClient);
        }
    }
}
