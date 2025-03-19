using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using whatsapp.Domain.Service;
using whatsapp.Domain.ServiceContract;
using System.Text.Json;

namespace whatsapp.Application.Services
{

    public class AuthService : BaseService, IAuthService
    {
        public AuthService(IConfiguration config) : base(config)
        {
        }


        public async Task<string> GetQRCode()
        {
            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/qr");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            var response = await client.GetAsync(request);

            Console.WriteLine("{0}", response.Content);

            return response.Content;
        }

        public async Task<string> GetPairCode(string phoneNumber)
        {

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/request-pairing-code");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            request.AddJsonBody("{\"phoneNumber\":\""+phoneNumber+"\",\"showNotification\":true}", false);
            var response = await client.PostAsync(request);

            Console.WriteLine("{0}", response.Content);

            using(JsonDocument doc = JsonDocument.Parse(response.Content))
            {
                JsonElement root = doc.RootElement;
                var pairingCode = root.GetProperty("data").GetProperty("data").GetProperty("pairingCode").ToString();
            
                return pairingCode;
            }

        }

        public async Task<string> GetStatus()
        {

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/status");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            var response = await client.GetAsync(request);

            Console.WriteLine("{0}", response.Content);

            using (JsonDocument doc = JsonDocument.Parse(response.Content))
            {
                JsonElement root = doc.RootElement;
                string instanceStatus = root.GetProperty("clientStatus").GetProperty("instanceStatus").ToString();

                Console.WriteLine(instanceStatus);
                return instanceStatus;
            }
        }

        public async Task<string> Logout()
        {
            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/logout");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            var response = await client.PostAsync(request);

            Console.WriteLine("{0}", response.Content);

            return response.Content;
        }

        public async Task<string> RetrieveUserData()
        {

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/me");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
            var response = await client.GetAsync(request);

            Console.WriteLine("{0}", response.Content);

            return response.Content;
        }
    }

}
