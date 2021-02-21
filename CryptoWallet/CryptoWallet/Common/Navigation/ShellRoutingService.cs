using System.Threading.Tasks;

using CryptoWallet.Common.Base;

using Xamarin.Forms;

namespace CryptoWallet.Common.Navigation
{
    public class ShellRoutingService : INavigationService
    {
        public Task GoBackAsync()
        {
            return Shell.Current.GoToAsync("..");
        }

        public Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel
        {
            return GoToAsync<TViewModel>("//", parameters);
        }

        public Task PopAsync()
        {
            return Shell.Current.Navigation.PopAsync();
        }

        public Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel
        {
            return GoToAsync<TViewModel>("", parameters);
        }

        private Task GoToAsync<TViewModel>(string routePrefix, string parameters) where TViewModel : BaseViewModel
        {
            string route = routePrefix + typeof(TViewModel).Name;

            if (!string.IsNullOrWhiteSpace(parameters))
            {
                route += $"?{parameters}";
            }

            return Shell.Current.GoToAsync(route);
        }
    }
}