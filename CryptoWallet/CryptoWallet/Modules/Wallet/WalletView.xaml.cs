using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoWallet.Modules.Wallet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletView : ContentPage
    {
        public WalletView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<WalletViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as WalletViewModel).InitializeAsync(false);
        }
    }
}