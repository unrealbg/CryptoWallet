using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Navigation;
using CryptoWallet.Modules.Login;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace CryptoWallet
{
    public class AppShellViewModel
    {
        private INavigationService navigationService;

        public AppShellViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public ICommand SignOutCommand { get => new Command(async () => await SignOut()); }

        private async Task SignOut()
        {
            Preferences.Remove(Constants.IS_USER_LOGGED_IN);
            this.navigationService.GoToLoginFlow();
            await this.navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}