using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Database;
using CryptoWallet.Common.Dialog;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Common.Security;
using CryptoWallet.Common.Validations;
using CryptoWallet.Modules.Register;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace CryptoWallet.Modules.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private IDialogMessage dialogMessage;
        private ValidatableObject<string> email;
        private INavigationService navigationService;
        private ValidatableObject<string> password;
        private IRepository<User> userRepository;

        public LoginViewModel(INavigationService navigationService,
            IRepository<User> repository,
            IDialogMessage message)
        {
            this.navigationService = navigationService;
            this.userRepository = repository;
            this.dialogMessage = message;
            AddValidations();
        }

        public ValidatableObject<string> Email
        {
            get => this.email;
            set { SetProperty(ref this.email, value); }
        }

        public ICommand LoginCommand { get => new Command(async () => await LoginUser(), () => IsNotBusy); }

        public ValidatableObject<string> Password
        {
            get => this.password;
            set { SetProperty(ref this.password, value); }
        }

        public ICommand RegisterCommand { get => new Command(async () => await GoToRegister()); }

        private void AddValidations()
        {
            this.Email = new ValidatableObject<string>();
            this.Password = new ValidatableObject<string>();

            this.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email is empty." });
            this.Email.Validations.Add(new EmailRule<string> { ValidationMessage = "Email is not in correct format." });
            this.Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is empty." });
        }

        private async Task DisplayCredentialsError()
        {
            await this.dialogMessage.DisplayAlert("Error", "Credentials are wrong.", "OK");
            Password.Value = "";
        }

        private bool EntriesCorrectlyPopulated()
        {
            this.Email.Validate();
            this.Password.Validate();

            return this.Email.IsValid && this.Password.IsValid;
        }

        private async Task GoToRegister()
        {
            await this.navigationService.InsertAsRoot<RegisterViewModel>();
        }

        private async Task LoginUser()
        {
            if (!EntriesCorrectlyPopulated())
            {
                return;
            }
            IsBusy = true;
            var user = (await this.userRepository.GetAllAsync())
                .FirstOrDefault(x => x.Email == Email.Value);
            if (user == null)
            {
                await DisplayCredentialsError();
                IsBusy = false;
                return;
            }
            if (!SecurePasswordHasher.Verify(Password.Value, user.HashedPassword))
            {
                await DisplayCredentialsError();
                IsBusy = false;
                return;
            }

            Preferences.Set(Constants.IS_USER_LOGGED_IN, true);
            Preferences.Set(Constants.USER_ID, Email.Value);
            this.navigationService.GoToMainFlow();
            IsBusy = false;
        }
    }
}