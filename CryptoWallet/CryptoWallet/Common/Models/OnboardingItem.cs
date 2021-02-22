namespace CryptoWallet.Common.Models
{
    public class OnboardingItem
    {
        public OnboardingItem(string imageUrl, string title, string details)
        {
            this.ImageUrl = imageUrl;
            this.Title = title;
            this.Details = details;
        }

        public string Details { get; set; }
        public string ImageUrl { get; set; }

        public string Title { get; set; }
    }
}