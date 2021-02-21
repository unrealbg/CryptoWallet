using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoWallet.Modules.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WithdrawnTransactionsView : ContentPage
    {
        public WithdrawnTransactionsView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<TransactionsVewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as TransactionsVewModel).InitializeAsync(Constants.TRANSACTION_WITHDRAWN);
        }
    }
}