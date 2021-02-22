using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoWallet.Modules.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingView : ContentPage
    {
        public OnboardingView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<OnboardingViewModel>();
        }
    }
}