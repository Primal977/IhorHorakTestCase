using Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using System.Net.Mime;
using System.Text;

namespace Common
{
    public abstract class BaseAPIClient<T>
    {
        protected readonly HttpClient HttpClient;
        protected readonly string ApiBaseUrl;
        protected IAsyncPolicy<HttpResponseMessage> RetryPolicy;

        protected BaseAPIClient(HttpClient httpClient, string apiBaseUrl)
        {
            HttpClient = httpClient;

            ApiBaseUrl = apiBaseUrl;

            if (string.IsNullOrEmpty(ApiBaseUrl))
            {
                throw new InvalidOperationException("You must set the ApiBaseUrl on your RestClient service implementation.  Please check your settings.");
            }

            HttpClient.BaseAddress = new Uri(ApiBaseUrl);

            httpClient.DefaultRequestHeaders.UserAgent.Clear();

            RetryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(5000));
        }
        public virtual JsonSerializerSettings JsonSerializerSettings =>
          new()
          {
              ContractResolver = new DefaultContractResolver
              {
                  NamingStrategy = new CamelCaseNamingStrategy()
              }
          };

        public async Task<TResponse> Get<TResponse>(string endpointUrl)
             where TResponse : class
        {
            var response = await SendRequest(HttpMethod.Get, endpointUrl, null);

            return await response.DeserializeContent<TResponse>(JsonSerializerSettings);
        }

        public async Task<TResponse> Post<TRequest, TResponse>(string endpointUrl, TRequest requestBody)
              where TRequest : class
        {
            var response = await SendRequest(HttpMethod.Post, endpointUrl, requestBody);

            return await response.DeserializeContent<TResponse>(JsonSerializerSettings);
        }

        public async Task<TRequest> Put<TRequest>(string endpointUrl, TRequest requestBody) where TRequest : class
        {
            await SendRequest(HttpMethod.Put, endpointUrl, requestBody);

            return requestBody;
        }
        public async Task Delete(string endpointUrl) => await SendRequest(HttpMethod.Delete, endpointUrl, null);

        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endpointUrl, object? requestBody)
        {
            var response = await RetryPolicy.ExecuteAndCaptureAsync(async () =>
            {
                var requestMessage = new HttpRequestMessage(method, new Uri(endpointUrl, UriKind.RelativeOrAbsolute));

                if (requestBody != null)
                {
                    var requestContent = new StringContent(
                       JsonConvert.SerializeObject(requestBody, JsonSerializerSettings),
                       Encoding.UTF8,
                       MediaTypeNames.Application.Json);

                    requestMessage.Content = requestContent;
                }

                var resp = await HttpClient.SendAsync(requestMessage);
                return resp;
            });

            return response.Result ?? response.FinalHandledResult;
        }
    }
}
