using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoWallet.Common.Models;

namespace CryptoWallet.Common.Network
{
    public interface ICryptoService
    {
        Task<List<Coin>> GetLatestPrices();
    }
}