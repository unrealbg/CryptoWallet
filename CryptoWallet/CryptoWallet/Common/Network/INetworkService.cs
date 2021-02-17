using System.Threading.Tasks;

namespace CryptoWallet.Common.Network
{
    public interface INetworkService
    {
        Task<TResult> GetAsync<TResult>(string uri);
        Task<TResult> PostAsync<TResult>(string uri, string jsonData);
        Task<TResult> PutAsync<TResult>(string uri, string jsonData);
        Task DeleteAsync(string uri);
    }
}
