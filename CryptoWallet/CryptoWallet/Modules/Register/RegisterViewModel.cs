using System.Threading.Tasks;
using System.Windows.Input;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Database;
using CryptoWallet.Common.Models;
using CryptoWallet.Common.Navigation;
using CryptoWallet.Common.Security;
using CryptoWallet.Common.Validations;
using CryptoWallet.Modules.Login;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace CryptoWallet.Modules.Register
{
    public class RegisterViewModel : BaseViewModel
    {
        private ValidatableObject<string> email;
        private ValidatableObject<string> name;
        private INavigationService navigationService;
        private ValidatableObject<string> password;
        private IRepository<User> userRepository;

        public RegisterViewModel(INavigationService navigationService, IRepository<User> userRepository)
        {
            this.navigationService = navigationService;
            this.userRepository = userRepository;
            AddValidations();
        }

        public ValidatableObject<string> Email
        {
            get => this.email;
            set { SetProperty(ref this.email, value); }
        }

        public ICommand LoginCommand { get => new Command(async () => await GoToLogin()); }

        public ValidatableObject<string> Name
        {
            get => this.name;
            set { SetProperty(ref this.name, value); }
        }

        public ValidatableObject<string> Password
        {
            get => this.password;
            set { SetProperty(ref this.password, value); }
        }

        public ICommand RegisterUserCommand { get => new Command(async () => await RegisterUser(), () => IsNotBusy); }

        private void AddValidations()
        {
            this.Email = new ValidatableObject<string>();
            this.Name = new ValidatableObject<string>();
            this.Password = new ValidatableObject<string>();

            this.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Ëmail is empty." });
            this.Email.Validations.Add(new EmailRule<string> { ValidationMessage = "Email is not in a correct format" });

            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Name is empty" });

            this.Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is empty" });
        }

        private bool EntriesIsCorrectrlyPopulated()
        {
            this.Email.Validate();
            this.Name.Validate();
            this.Password.Validate();

            return this.Name.IsValid && this.Email.IsValid && this.Password.IsValid;
        }

        private async Task GoToLogin()
        {
            await this.navigationService.InsertAsRoot<LoginViewModel>();
        }

        private async Task RegisterUser()
        {
            if (!EntriesIsCorrectrlyPopulated())
            {
                return;
            }
            IsBusy = true;

            User newUser = new()
            {
                Email = this.Email.Value,
                FirstName = this.Name.Value,
                HashedPassword = SecurePasswordHasher.Hash(this.Password.Value)
            };

            await this.userRepository.SaveAsync(newUser);

            Preferences.Set(Constants.IS_USER_LOGGED_IN, true);
            Preferences.Set(Constants.USER_ID, Email.Value);
            this.navigationService.GoToMainFlow();

            IsBusy = false;
        }
    }
}