using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Validations;

using Xamarin.Forms;

namespace CryptoWallet.Modules.AddTransaction
{
    [QueryProperty("ID", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {

        private bool isDeposit;

        public AddTransactionViewModel()
        {
            this.AvailableAssets = new ObservableCollection<Coin>(Coin.GetAvailableAssets());
            this.Amount = new ValidatableObject<decimal>();
            this.Amount.Validations.Add(new NonNegativeRule { ValidationMessage = "Please enter amount greater than zero" });
        }

        public bool IsDeposit
        {
            get => this.isDeposit;
            set { SetProperty(ref this.isDeposit, value); }
        }


        private string id;
        public string Id
        {
            set
            {
                this.id = Uri.UnescapeDataString(value);
            }
        }

        private ObservableCollection<Coin> availableAssets;

        public ObservableCollection<Coin> AvailableAssets
        {
            get => this.availableAssets;
            set { SetProperty(ref this.availableAssets, value); }
        }

        private Coin selectedCoin;

        public Coin SelectedCoin
        {
            get => this.selectedCoin;
            set { SetProperty(ref this.selectedCoin, value); }
        }

        private DateTime transactionDate;

        public DateTime TransactionDate
        {
            get => this.transactionDate;
            set { SetProperty(ref this.transactionDate, value); }
        }

        private ValidatableObject<decimal> amount;

        public ValidatableObject<decimal> Amount
        {
            get => this.amount;
            set { SetProperty(ref this.amount, value); }
        }


        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction()); }

        private async Task AddTransaction()
        {
            this.Amount.Validate();
            if (this.Amount.IsValid)
            {
                return;
            }
        }

    }
}
