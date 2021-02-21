using System.Threading.Tasks;

namespace CryptoWallet.Common.Network
{
    public interface INetworkService
    {
        Task DeleteAsync(string uri);

        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> PostAsync<TResult>(string uri, string jsonData);

        Task<TResult> PutAsync<TResult>(string uri, string jsonData);
    }
}