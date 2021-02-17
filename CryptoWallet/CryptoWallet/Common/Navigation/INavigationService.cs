using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CryptoWallet.Common.Base;

namespace CryptoWallet.Common.Navigation
{
    public interface INavigationService
    {
        Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;
        
        Task PopAsync();

        Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;

        Task GoBackAsync();
    }
}
