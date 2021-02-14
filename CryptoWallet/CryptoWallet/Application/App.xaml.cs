using System.Reflection;

using Autofac;

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

            Container = builder.Build();

            MainPage = new AppShell();
        }

    }
}
