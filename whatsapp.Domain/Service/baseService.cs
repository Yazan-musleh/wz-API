using Microsoft.Extensions.Configuration;
using RestSharp;

namespace whatsapp.Domain.Service
{
    public abstract class BaseService
    {
        public readonly RestClient _client;
        public readonly string _apiKey;

        public BaseService(IConfiguration config)
        {
            var baseUrl = config["WaAPI:BaseUrl"];
            _apiKey = config["WaAPI:ApiKey"];
            _client = new RestClient(baseUrl);
        }

        protected async Task<string> SendRequest(string endpoint, Method method, object body = null)
        {
            var request = new RestRequest(endpoint, method);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");

            if (body != null)
                request.AddJsonBody(body);

            var response = await _client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.Content}");
            }

            return response.Content;
        }

        public async Task<string> GetStatus()
        {
            return await SendRequest("/instance/status", Method.Get);
        }
    }
}
