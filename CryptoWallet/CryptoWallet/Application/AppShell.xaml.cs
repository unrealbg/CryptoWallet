using Autofac;

using CryptoWallet.Modules.AddTransaction;

using Xamarin.Forms;

namespace CryptoWallet
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AppShellViewModel>();

            Routing.RegisterRoute("AddTransactionViewModel", typeof(AddTransactionView));
        }
    }
}