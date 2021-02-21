using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoWallet.Common.Database
{
    public interface IRepository<T> where T : IDatabaseItem, new()
    {
        Task<int> DeleteAsync(T item);

        Task<List<T>> GetAllAsync();

        Task<T> GetById(int id);

        Task<int> SaveAsync(T item);
    }
}