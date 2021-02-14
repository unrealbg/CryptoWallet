using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CryptoWallet.Common.Base;
using CryptoWallet.Common.Controllers;
using CryptoWallet.Common.Models;

using Microcharts;

using SkiaSharp;

namespace CryptoWallet.Modules.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private IWalletController walletController;

        public WalletViewModel(IWalletController walletController)
        {
            this.walletController = walletController;
        }

        public override async Task InitializeAsync(object parameter)
        {
            var assets = await this.walletController.GetCoins();
            BuildChart(assets);
        }

        private void BuildChart(List<Coin> assets)
        {

            SKColor whiteColor = SKColor.Parse("ffffff");
            List<ChartEntry> entries = new();
            List<Coin> colors = Coin.GetAvailableAssets();

            foreach (var asset in assets)
            {
                entries.Add(new ChartEntry((float)asset.DollarValue)
                {
                    TextColor = whiteColor,
                    ValueLabel = asset.Name,
                    Color = SKColor.Parse(colors.First(x => x.Symbol == asset.Symbol).HexColor)
                });
            }

            DonutChart chart = new() { Entries = entries};
            chart.BackgroundColor = whiteColor;
            this.PortfolioView = chart;
        }

        private Chart portfolioView;

        public Chart PortfolioView
        {
            get => portfolioView;
            set { SetProperty(ref portfolioView, value); }
        }

    }
}
