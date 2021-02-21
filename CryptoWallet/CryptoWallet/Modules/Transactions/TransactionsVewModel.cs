using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Controllers;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Modules.AddTransaction;

using Xamarin.Forms;

namespace CryptoWallet.Modules.Transactions
{
    public class TransactionsVewModel : BaseViewModel
    {
        private string filter = string.Empty;
        private bool isRefreshing;
        private INavigationService navigationService;
        private Transaction selectedTransaction;
        private ObservableCollection<Transaction> transactions;
        private IWalletController walletController;

        public TransactionsVewModel(IWalletController walletController, INavigationService navigationService)
        {
            this.walletController = walletController;
            this.navigationService = navigationService;
            this.Transactions = new ObservableCollection<Transaction>();
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set { SetProperty(ref this.isRefreshing, value); }
        }

        public ICommand RefreshTransactionsCommand { get => new Command(async () => await RefreshTransactions()); }

        public Transaction SelectedTransaction
        {
            get => selectedTransaction;
            set { SetProperty(ref selectedTransaction, value); }
        }

        public ICommand TradeCommand { get => new Command(async () => await PerformNavigation()); }

        public ObservableCollection<Transaction> Transactions
        {
            get => this.transactions;
            set { SetProperty(ref this.transactions, value); }
        }

        public ICommand TransactionSelectedCommand { get => new Command(async () => await TransactionSelected()); }

        public override async Task InitializeAsync(object parameter)
        {
            this.filter = parameter.ToString();
            await GetTransactions();
        }

        private async Task GetTransactions()
        {
            this.IsRefreshing = true;
            List<Transaction> transactions = await this.walletController.GetTransactions();
            if (!string.IsNullOrEmpty(this.filter))
            {
                transactions = transactions.Where(t => t.Status == this.filter).ToList();
            }
            this.Transactions = new ObservableCollection<Transaction>(transactions);
            this.IsRefreshing = false;
        }

        private async Task PerformNavigation()
        {
            await this.navigationService.PushAsync<AddTransactionViewModel>();
        }

        private async Task RefreshTransactions()
        {
            await GetTransactions();
        }

        private async Task TransactionSelected()
        {
            await this.navigationService.PushAsync<AddTransactionViewModel>($"id={this.SelectedTransaction.Id}");
        }
    }
}