using System.Reflection;

using Autofac;

using CryptoWallet.Common.Database;
using CryptoWallet.Common.Models;
using CryptoWallet.Modules.Loading;

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
            builder.RegisterType<Repository<User>>().As<IRepository<User>>();

            Container = builder.Build();

            MainPage = Container.Resolve<LoadingView>();
        }
    }
}