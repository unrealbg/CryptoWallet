using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Database;
using CryptoWallet.Common.Dialog;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Common.Validations;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace CryptoWallet.Modules.AddTransaction
{
    [QueryProperty("Id", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {
        private ValidatableObject<decimal> amount;
        private ObservableCollection<Coin> availableAssets;
        private IDialogMessage dialogMessage;
        private string id;
        private bool isDeposit;
        private INavigationService navigationService;
        private IRepository<Transaction> repository;
        private Coin selectedCoin;

        private DateTime transactionDate;

        public AddTransactionViewModel(IRepository<Transaction> repository, IDialogMessage dialogMessage, INavigationService navigationService)
        {
            this.repository = repository;
            this.dialogMessage = dialogMessage;
            this.navigationService = navigationService;
            this.AvailableAssets = new ObservableCollection<Coin>(Coin.GetAvailableAssets());
            AddValidations();
        }

        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction(), () => IsNotBusy); }

        public ValidatableObject<decimal> Amount
        {
            get => this.amount;
            set { SetProperty(ref this.amount, value); }
        }

        public ObservableCollection<Coin> AvailableAssets
        {
            get => this.availableAssets;
            set { SetProperty(ref this.availableAssets, value); }
        }

        public ICommand GoBackCommand { get => new Command(async () => await GoBack()); }

        public string Id
        {
            get => this.id;
            set
            {
                this.id = Uri.UnescapeDataString(value);
            }
        }

        public bool IsDeposit
        {
            get => this.isDeposit;
            set { SetProperty(ref this.isDeposit, value); }
        }

        public Coin SelectedCoin
        {
            get => this.selectedCoin;
            set { SetProperty(ref this.selectedCoin, value); }
        }

        public DateTime TransactionDate
        {
            get => this.transactionDate;
            set { SetProperty(ref this.transactionDate, value); }
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

        private void AddValidations()
        {
            this.Amount = new ValidatableObject<decimal>();
            this.Amount.Validations.Add(new NonNegativeRule { ValidationMessage = "Please enter amount greater than zero" });
        }

        private async Task GoBack()
        {
            var shouldGoBack = await this.dialogMessage.DisplayAlert("Confirm", "Are you sure you want to navigate back? Any unsaved changes will be lost",
                                                                "OK", "Cancel");
            if (shouldGoBack)
            {
                await this.navigationService.GoBackAsync();
            }
        }

        private async Task SaveNewTransaction()
        {
            string userId = Preferences.Get(Constants.USER_ID, string.Empty);

            Transaction transaction = new()
            {
                Amount = this.Amount.Value,
                TransactionDate = this.TransactionDate,
                Symbol = this.SelectedCoin.Symbol,
                Status = this.IsDeposit == true ? Constants.TRANSACTION_DEPOSITED : Constants.TRANSACTION_WITHDRAWN,
                Id = string.IsNullOrEmpty(this.Id) ? 0 : int.Parse(this.Id),
                UserId = userId
            };

            await this.repository.SaveAsync(transaction);
        }
    }
}