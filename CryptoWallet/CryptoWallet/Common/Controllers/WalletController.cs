using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CryptoWallet.Common.Models;

namespace CryptoWallet.Common.Controllers
{
    public class WalletController : IWalletController
    {
        private List<Coin> defaultAssets = new()
        {
            new Coin
            {
                Name = "Bitcoin",
                Amount = 1,
                Symbol = "BTC",
                DollarValue = 9500
            },
            new Coin
            {
                Name = "Ethereum",
                Amount = 2,
                Symbol = "ETH",
                DollarValue = 300
            },
            new Coin
            {
                Name = "Litecoin",
                Amount = 3,
                Symbol = "LTC",
                DollarValue = 150
            },
        };
        public Task<List<Coin>> GetCoins(bool forceReload = false)
        {
            return Task.FromResult(this.defaultAssets);
        }
    }
}
