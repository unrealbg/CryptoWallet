using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace CryptoWallet.Common.Base
{
    public abstract class ExtendedBindableObject : BindableObject
    {
        protected bool SetProperty<T>(ref T backingStore, T value,
                  [CallerMemberName] string propertyName = "",
                  Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }
            backingStore = value;
            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
