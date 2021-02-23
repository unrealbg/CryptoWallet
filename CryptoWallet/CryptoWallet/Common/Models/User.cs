using CryptoWallet.Common.Database;

namespace CryptoWallet.Common.Models
{
    public class User : BaseDatabaseItem
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string HashedPassword { get; set; }
    }
}