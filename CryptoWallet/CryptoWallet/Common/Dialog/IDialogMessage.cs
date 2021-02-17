using System.Threading.Tasks;

namespace CryptoWallet.Common.Dialog
{
    public interface IDialogMessage
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        Task<string> DisplayPrompt(string title, string message, string accept, string cancel);

        Task<string> DisplayActionSheet(string title, string destruction, params string[] buttons);
    }
}
