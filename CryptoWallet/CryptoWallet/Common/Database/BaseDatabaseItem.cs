
using SQLite;

namespace CryptoWallet.Common.Database
{
    public abstract class BaseDatabaseItem : IDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }

}
