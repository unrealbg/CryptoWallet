using System.Threading.Tasks;

namespace CryptoWallet.Common.Base
{
    public abstract class BaseViewModel : ExtendedBindableObject
    {
        private bool _isBusy;

        private bool _isNotBusy = true;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    IsNotBusy = !_isBusy;
                }
            }
        }

        public bool IsNotBusy
        {
            get => _isNotBusy;
            set
            {
                if (SetProperty(ref _isNotBusy, value))
                {
                    IsBusy = !_isNotBusy;
                }
            }
        }

        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }
    }
}