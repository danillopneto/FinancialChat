using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Jobsity.Challenge.FinancialChat.Core.Infra.Gateways
{
    public abstract class BaseGateway<T>
    {
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

        protected ILogger<T> Logger { get; }

        protected BaseGateway(
                              IHttpClientFactory httpClientFactory,
                              ILogger<T> logger)
        {
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<TResponse> SendGetRequest<TResponse>(
                                                                       string clientName,
                                                                       string url,
                                                                       CancellationToken cancellationToken = default)
        {
            var responseContent = string.Empty;

            try
            {
                using var client = GetClient(clientName);

                var clientResponse = await client.GetAsync(url, cancellationToken);
                responseContent = await clientResponse.Content.ReadAsStringAsync(cancellationToken);

                if (typeof(TResponse).IsValueType)
                {
                    return JsonConvert.DeserializeObject<TResponse>(responseContent, JsonSettings);
                }

                return (TResponse)(object)responseContent;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error trying to reach {Url}: {ResponseContent}", url, responseContent);
                throw;
            }
        }

        public virtual async Task SendPostRequest<TRequest>(
                                                            string clientName,
                                                            string url,
                                                            TRequest body,
                                                            CancellationToken cancellationToken = default)
        {
            var responseContent = string.Empty;

            try
            {
                using var client = GetClient(clientName);
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var clientResponse = await client.PostAsync(url, content, cancellationToken);
                responseContent = await clientResponse.Content.ReadAsStringAsync();
                clientResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error trying to reach {Url}: {ResponseContent}", url, responseContent);
                throw;
            }
        }

        protected virtual HttpClient GetClient(string name)
        {
            return HttpClientFactory.CreateClient(name);
        }
    }
}