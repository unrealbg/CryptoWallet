﻿using System.Threading.Tasks;

using CryptoWallet.Common.Base;

namespace CryptoWallet.Common.Navigation
{
    public interface INavigationService
    {
        Task GoBackAsync();

        void GoToLoginFlow();

        void GoToMainFlow();

        Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;

        Task PopAsync();

        Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;
    }
}