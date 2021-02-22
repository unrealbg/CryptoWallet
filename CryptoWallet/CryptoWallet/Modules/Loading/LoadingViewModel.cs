using System.Threading.Tasks;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Modules.Login;

using Xamarin.Essentials;

namespace CryptoWallet.Modules.Loading
{
    public class LoadingViewModel : BaseViewModel
    {
        private INavigationService navigationService;

        public LoadingViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override Task InitializeAsync(object parameter)
        {
            if (!Preferences.ContainsKey(Constants.SHOWN_ONBOARDING))
            {
                Preferences.Set(Constants.SHOWN_ONBOARDING, true);
                this.navigationService.GoToLoginFlow();

                return Task.CompletedTask;
            }

            if (Preferences.ContainsKey(Constants.IS_USER_LOGGED_IN) && Preferences.Get(Constants.IS_USER_LOGGED_IN, false) == true)
            {
                this.navigationService.GoToMainFlow();

                return Task.CompletedTask;
            }

            this.navigationService.GoToLoginFlow();
            return this.navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}