using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Modules.Login;

using Xamarin.Forms;

namespace CryptoWallet.Modules.Onboarding
{
    public class OnboardingViewModel : BaseViewModel
    {
        private INavigationService navigationService;

        public OnboardingViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public ObservableCollection<OnboardingItem> OnboardingSteps { get; set; } = new ObservableCollection<OnboardingItem>
        {
            new OnboardingItem("welcome.png", "Welcome to CryptoWallet", "Manage all your crypto assets! It's simple and easy!"),
            new OnboardingItem("nice.png", "Nice and Tidy Crypto Portfolio", "Keep BTC, ETH, XRP and many other tokens."),
            new OnboardingItem("security.png", "Your Safety is Our Top Priority", "On top-notch security features will keep you completely safe.")
        };

        public ICommand SkipCommand { get => new Command(async () => await Skip()); }

        private async Task Skip()
        {
            await this.navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}