using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoWallet.Common.Models;

namespace CryptoWallet.Common.Controllers
{
    public interface IWalletController
    {
        Task<List<Coin>> GetCoins(bool forceReload = false);

        Task<List<Transaction>> GetTransactions(bool foreceReload = false);
    }
}