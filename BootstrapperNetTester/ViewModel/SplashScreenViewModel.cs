using CommunityToolkit.Mvvm.ComponentModel;
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
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(40);
                ProgressValue = i;
                ProgressText = $"Working... {ProgressValue}%";
            }
            ProgressValue = 100;
        }
    }
}
