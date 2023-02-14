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
            _splashScreen = new SplashScreen
            {
                DataContext = _splashScreenViewModel
            };
            SplashScreenAction = _splashScreenViewModel.DoSomethingAtStartup;
        }

        public override bool IsSplashScreenEnabled => true;
        public override TimeSpan SplashScreenDuration => TimeSpan.FromSeconds(4);

        protected override Window CreateMainWindow() => _mainWindow;
        protected override Window CreateSplashScreenWindow() => _splashScreen;


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
