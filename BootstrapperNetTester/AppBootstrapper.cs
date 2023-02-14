using BootstrapperNet;
using BootstrapperNetTester.Services;
using BootstrapperNetTester.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace BootstrapperNetTester
{
    public class AppBootstrapper : WpfBootstrapper
    {
        private Window _mainWindow;
        private SplashScreenViewModel _splashScreenViewModel;
        private Window _splashScreen;

        public AppBootstrapper()
        {
            UnhandledException += AppBootstrapper_UnhandledException;
            _mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel { Title = "My main window" }
            };
            _splashScreenViewModel = new SplashScreenViewModel();
            _splashScreen = new SplashScreen { DataContext = _splashScreenViewModel };
        }

        public override Window? MainWindow => _mainWindow;
        public override bool IsSplashScreenEnabled => true;
        public override Window? SplashScreenWindow => _splashScreen;
        public override TimeSpan SplashScreenDuration => TimeSpan.FromSeconds(4);
        public override Action? SplashScreenAction => _splashScreenViewModel.DoSomethingAtStartup;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISimpleSingletonService, SimpleSingletonService>();
            services.AddTransient<IHelloService, HelloService>();

            // Must be called after adding services
            base.ConfigureServices(services);
        }

        private void AppBootstrapper_UnhandledException(object? sender, Exception e)
        {
            MessageBox.Show(e.ToString(), "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
