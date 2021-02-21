using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CryptoWallet.Common.Extensions;

using SQLite;

namespace CryptoWallet.Common.Database
{
    public class Repository<T> : IRepository<T> where T : IDatabaseItem, new()
    {
        private readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
          {
              return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
          });

        public Repository()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        private SQLiteAsyncConnection Database => lazyInitializer.Value;

        public Task<int> DeleteAsync(T item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<List<T>> GetAllAsync()
        {
            return Database.Table<T>().ToListAsync();
        }

        public Task<T> GetById(int id)
        {
            return Database.Table<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(T item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        private async Task InitializeAsync()
        {
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
            {
                await Database.CreateTableAsync(typeof(T)).ConfigureAwait(false);
            }
        }
    }
}