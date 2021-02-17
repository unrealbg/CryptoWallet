using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Database;
using CryptoWallet.Common.Dialog;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Common.Validations;

using Xamarin.Forms;

namespace CryptoWallet.Modules.AddTransaction
{
    [QueryProperty("Id", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {
        private IRepository<Transaction> repository;
        private IDialogMessage dialogMessage;
        private INavigationService navigationService;
        private bool isDeposit;

        public AddTransactionViewModel(IRepository<Transaction> repository, IDialogMessage dialogMessage, INavigationService navigationService)
        {

            this.repository = repository;
            this.dialogMessage = dialogMessage;
            this.navigationService = navigationService;
            this.AvailableAssets = new ObservableCollection<Coin>(Coin.GetAvailableAssets());
            AddValidations();
        }

        public override async Task InitializeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(this.Id) || !int.TryParse(this.Id, out int transactionId))
            {
                this.TransactionDate = DateTime.UtcNow;
                this.IsDeposit = true;
                return;
            }

            Transaction transaction = await this.repository.GetById(transactionId);
            this.IsDeposit = transaction.Status == Constants.TRANSACTION_DEPOSITED;
            this.Amount.Value = transaction.Amount;
            this.TransactionDate = transaction.TransactionDate;
            this.SelectedCoin = Coin.GetAvailableAssets().First(x => x.Symbol == transaction.Symbol);
        }

        public bool IsDeposit
        {
            get => this.isDeposit;
            set { SetProperty(ref this.isDeposit, value); }
        }


        private string id;
        public string Id
        {
            get => this.id;
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


        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction() , () => IsNotBusy); }

        public ICommand GoBackCommand { get => new Command(async () => await GoBack()); }

        private async Task GoBack()
        {
            var shouldGoBack = await this.dialogMessage.DisplayAlert("Confirm", "Are you sure you want to navigate back? Any unsaved changes will be lost",
                                                                "OK", "Cancel");
            if (shouldGoBack)
            {
                await this.navigationService.GoBackAsync();
            }
        }

        private async Task AddTransaction()
        {
            this.Amount.Validate();
            if (!this.amount.IsValid)
            {
                return;
            }

            if (this.SelectedCoin == null)
            {
                await this.dialogMessage.DisplayAlert("Error", "Please select a coin.", "OK");
                return;
            }

            IsBusy = true;
            await SaveNewTransaction();
            await this.navigationService.PopAsync();

            IsBusy = false;
        }

        private async Task SaveNewTransaction()
        {
            Transaction transaction = new()
            {
                Amount = this.Amount.Value,
                TransactionDate = this.TransactionDate,
                Symbol = this.SelectedCoin.Symbol,
                Status = this.IsDeposit == true ? Constants.TRANSACTION_DEPOSITED : Constants.TRANSACTION_WITHDRAWN,
                Id = string.IsNullOrEmpty(this.Id) ? 0 : int.Parse(this.Id)
            };

            await this.repository.SaveAsync(transaction);
        }

        private void AddValidations()
        {
            this.Amount = new ValidatableObject<decimal>();
            this.Amount.Validations.Add(new NonNegativeRule { ValidationMessage = "Please enter amount greater than zero" });
        }
    }
}
