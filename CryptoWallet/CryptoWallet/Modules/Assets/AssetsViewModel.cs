using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Controllers;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Modules.AddTransaction;

using Xamarin.Forms;

namespace CryptoWallet.Modules.Assets
{
    public class AssetsViewModel : BaseViewModel
    {
        private ObservableCollection<Coin> assets;
        private INavigationService navigationService;
        private IWalletController walletController;

        public AssetsViewModel(IWalletController walletController, INavigationService navigationService)
        {
            this.walletController = walletController;
            this.navigationService = navigationService;
            this.Assets = new ObservableCollection<Coin>();
        }

        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction()); }

        public ObservableCollection<Coin> Assets
        {
            get => this.assets;
            set { SetProperty(ref this.assets, value); }
        }

        public override async Task InitializeAsync(object parameter)
        {
            var assets = await this.walletController.GetCoins();
            this.Assets = new ObservableCollection<Coin>(assets);
        }

        private async Task AddTransaction()
        {
            await this.navigationService.PushAsync<AddTransactionViewModel>();
        }
    }
}