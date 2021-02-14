using System.Text;
using System.Threading.Tasks;

namespace CryptoWallet.Common.Base
{
    public abstract class BaseViewModel : ExtendedBindableObject
    {
        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        private bool _isBusy;
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

        private bool _isNotBusy = true;
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
    }
}
