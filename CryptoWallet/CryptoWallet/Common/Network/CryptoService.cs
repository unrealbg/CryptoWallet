using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoWallet.Common.Models;

namespace CryptoWallet.Common.Network
{
    public class CryptoService : ICryptoService
    {
        private const string PRICES_ENDPOINT = "simple/price?ids=bitcoin%2Cbitcoin-cash%2Cdash%2Cethereum%2Ceos%2Clitecoin%2Cmonero%2Cripple%2Cstellar&vs_currencies=usd";
        private INetworkService networkService;

        public CryptoService(INetworkService networkService)
        {
            this.networkService = networkService;
        }

        public async Task<List<Coin>> GetLatestPrices()
        {
            string url = Constants.CRYPTO_API + PRICES_ENDPOINT;
            Dictionary<string, Dictionary<string, double?>> result = await this.networkService.GetAsync<Dictionary<string, Dictionary<string, double?>>>(url);
            List<Coin> coins = Coin.GetAvailableAssets();

            foreach (var coin in coins)
            {
                Dictionary<string, double?> coinPrices = result[coin.Name.Replace(' ', '-').ToLower()];
                double? coinPrice = coinPrices["usd"];
                coin.Price = coinPrice.HasValue ? coinPrice.Value : 0;
            }

            return coins;
        }
    }
}