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

using Microcharts;

using SkiaSharp;

using Xamarin.Forms;

namespace CryptoWallet.Modules.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private ObservableCollection<Coin> assets;
        private int coinsHeight;
        private bool hasTransactions;
        private bool isRefreshing;
        private ObservableCollection<Transaction> latestTransactions;
        private INavigationService navigationService;
        private decimal portfolioValue;
        private Chart portfolioView;
        private int transactionsHeight;
        private IWalletController walletController;

        public WalletViewModel(IWalletController walletController, INavigationService navigationService)
        {
            this.walletController = walletController;
            this.navigationService = navigationService;
            this.Assets = new ObservableCollection<Coin>();
            this.LatestTransactions = new ObservableCollection<Transaction>();
        }

        public ICommand AddNewTransactionCommand { get => new Command(async () => await AddNewTransaction()); }

        public ObservableCollection<Coin> Assets
        {
            get => this.assets;
            set
            {
                SetProperty(ref this.assets, value);
                if (this.assets == null)
                {
                    return;
                }

                this.CoinsHeight = this.assets.Count * 85;
            }
        }

        public int CoinsHeight
        {
            get => this.coinsHeight;
            set { SetProperty(ref this.coinsHeight, value); }
        }

        public bool HasTransactions
        {
            get => this.hasTransactions;
            set { SetProperty(ref this.hasTransactions, value); }
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set { SetProperty(ref this.isRefreshing, value); }
        }

        public ObservableCollection<Transaction> LatestTransactions
        {
            get => this.latestTransactions;
            set
            {
                SetProperty(ref this.latestTransactions, value);
                if (this.latestTransactions == null)
                {
                    return;
                }

                this.HasTransactions = this.latestTransactions.Count > 0;

                if (this.latestTransactions.Count == 0)
                {
                    this.TransactionsHeight = 430;
                    return;
                }

                this.TransactionsHeight = this.latestTransactions.Count * 85;
            }
        }

        public decimal PortfolioValue
        {
            get => this.portfolioValue;
            set { SetProperty(ref this.portfolioValue, value); }
        }

        public Chart PortfolioView
        {
            get => portfolioView;
            set { SetProperty(ref portfolioView, value); }
        }

        public ICommand RefreshAssetsCommand { get => new Command(async () => await InitializeAsync(true)); }

        public int TransactionsHeight
        {
            get => this.transactionsHeight;
            set { SetProperty(ref this.transactionsHeight, value); }
        }

        public override async Task InitializeAsync(object parameter)
        {
            bool reload = (bool)parameter;

            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            this.IsRefreshing = true;

            var transactions = await this.walletController.GetTransactions(reload);
            this.LatestTransactions = new ObservableCollection<Transaction>(transactions.Take(5));

            var assets = await this.walletController.GetCoins(reload);
            this.Assets = new ObservableCollection<Coin>(assets.Take(3));
            BuildChart(assets);
            this.PortfolioValue = assets.Sum(x => x.DollarValue);

            this.IsRefreshing = false;
            IsBusy = false;
        }

        private async Task AddNewTransaction()
        {
            await this.navigationService.PushAsync<AddTransactionViewModel>();
        }

        private void BuildChart(List<Coin> assets)
        {
            SKColor whiteColor = SKColor.Parse("ffffff");
            List<ChartEntry> entries = new();
            List<Coin> colors = Coin.GetAvailableAssets();

            foreach (var asset in assets)
            {
                entries.Add(new ChartEntry((float)asset.DollarValue)
                {
                    TextColor = whiteColor,
                    ValueLabel = asset.Name,
                    Color = SKColor.Parse(colors.First(x => x.Symbol == asset.Symbol).HexColor)
                });
            }

            DonutChart chart = new() { Entries = entries };
            chart.BackgroundColor = whiteColor;
            this.PortfolioView = chart;
        }
    }
}