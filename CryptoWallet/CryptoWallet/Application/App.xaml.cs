using System.Reflection;

using Autofac;

using CryptoWallet.Common.Database;
using CryptoWallet.Common.Models;

using Xamarin.Forms;

namespace CryptoWallet
{
    public partial class App : Application
    {
        public static IContainer Container;

        public App()
        {
            InitializeComponent();
            ContainerBuilder builder = new();
            Assembly dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess).AsImplementedInterfaces().AsSelf();

            builder.RegisterType<Repository<Transaction>>().As<IRepository<Transaction>>();

            Container = builder.Build();

            MainPage = new AppShell();
        }

    }
}
