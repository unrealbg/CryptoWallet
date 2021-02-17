using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CryptoWallet.Common.Network
{

    public class NetworkService : INetworkService
    {
        private HttpClient httpClient;

        public NetworkService()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(uri);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = JsonConvert.DeserializeObject<TResult>(serialized);

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string jsonData)
        {
            StringContent content = new(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await this.httpClient.PostAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = JsonConvert.DeserializeObject<TResult>(serialized);

            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string uri, string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await this.httpClient.PutAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = JsonConvert.DeserializeObject<TResult>(serialized);

            return result;
        }

        public async Task DeleteAsync(string uri)
        {
            await this.httpClient.DeleteAsync(uri);
        }
    }
}
