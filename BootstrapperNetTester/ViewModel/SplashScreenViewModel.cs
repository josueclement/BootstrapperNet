using BootstrapperNet;
using BootstrapperNetTester.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace BootstrapperNetTester.ViewModel
{
    public class SplashScreenViewModel : ObservableObject
    {
        public double ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }
        private double _progressValue;

        public string? ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }
        private string? _progressText;

        public void DoSomethingAtStartup()
        {
            IHelloService? service = Bootstrapper.Current?.ServiceProvider?.GetService<IHelloService>();

            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(90);
                ProgressValue = i;
                ProgressText = $"Working... {ProgressValue}% - {service?.SayHello()}";
            }
            ProgressValue = 100;
        }
    }
}
