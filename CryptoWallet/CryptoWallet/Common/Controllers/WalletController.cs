using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CryptoWallet.Common.Database;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Network;

namespace CryptoWallet.Common.Controllers
{
    public class WalletController : IWalletController
    {
        private IRepository<Transaction> transactionRepository;
        private ICryptoService cryptoService;
        private List<Coin> cachedCoins = new();
        private List<Coin> defaultAssets = new()
        {
            new Coin
            {
                Name = "Bitcoin",
                Amount = 0M,
                Symbol = "BTC",
                DollarValue = 0
            },
            new Coin
            {
                Name = "Ethereum",
                Amount = 0,
                Symbol = "ETH",
                DollarValue = 0
            },
            new Coin
            {
                Name = "Litecoin",
                Amount = 0,
                Symbol = "LTC",
                DollarValue = 0
            },
        };
        public WalletController(IRepository<Transaction> transactionRepository, ICryptoService cryptoService)
        {
            this.transactionRepository = transactionRepository;
            this.cryptoService = cryptoService;
        }
        public async Task<List<Coin>> GetCoins(bool forceReload = false)
        {
            List<Transaction> transactions = await LoadTransactions(forceReload);
            if (transactions.Count == 0 || this.cachedCoins.Count == 0)
            {
                return this.defaultAssets;
            }

            var groupedTransactions = transactions.GroupBy(x => x.Symbol);
            List<Coin> result = new();

            foreach (var item in groupedTransactions)
            {
                decimal amount = item.Where(x => x.Status == Constants.TRANSACTION_DEPOSITED).Sum(x => x.Amount)
                               - item.Where(x => x.Status == Constants.TRANSACTION_WITHDRAWN).Sum(x => x.Amount);

                Coin newCoin = new()
                {
                    Symbol = item.Key,
                    Amount = amount,
                    DollarValue = amount * (decimal)this.cachedCoins.FirstOrDefault(x => x.Symbol == item.Key).Price,
                    Name = Coin.GetAvailableAssets().FirstOrDefault(x => x.Symbol == item.Key).Name
                };

                result.Add(newCoin);
            }

            return result.OrderByDescending(x => x.DollarValue).ToList();
        }

        public async Task<List<Transaction>> GetTransactions(bool foreceReload = false)
        {
            List<Transaction> transactions = await LoadTransactions(foreceReload);
            if (transactions.Count == 0 || this.cachedCoins.Count == 0)
            {
                return transactions;
            }

            transactions.ForEach(t =>
            {
                t.StatusImageSource = t.Status == Constants.TRANSACTION_DEPOSITED ?
                                            Constants.TRANSACTION_DEPOSITED_IMAGE :
                                            Constants.TRANSACTION_WITHDRAWN_IMAGE;
                t.DollarValue = t.Amount * (decimal)this.cachedCoins.First(x => x.Symbol == t.Symbol).Price;
            });

            return transactions;
        }

        private async Task<List<Transaction>> LoadTransactions(bool forceReload)
        {
            if (this.cachedCoins.Count == 0 || forceReload)
            {
                this.cachedCoins = await this.cryptoService.GetLatestPrices();
            }

            var transactions = await this.transactionRepository.GetAllAsync();
            transactions = transactions.OrderByDescending(x => x.TransactionDate).ToList();
            return transactions;
        }
    }
}
