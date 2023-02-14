using BootstrapperNet;
using BootstrapperNetTester.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;

namespace BootstrapperNetTester.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            TestTransientServiceCommand = new WpfCommand((param) => true, TestTransientService);
            TestSingletonServiceCommand = new WpfCommand((param) => true, TestSingletonService);
            TestExceptionCommand = new WpfCommand((param) => true, TestException);
        }

        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string? _title;

        public ICommand TestTransientServiceCommand { get; private set; }
        public ICommand TestSingletonServiceCommand { get; private set; }
        public ICommand TestExceptionCommand { get; private set; }

        private void TestTransientService(object? param)
        {
            IHelloService? service = Bootstrapper.Current?.ServiceProvider?.GetService<IHelloService>();
            if (service != null)
            {
                MessageBox.Show(service.SayHello());
            }
        }

        private void TestSingletonService(object? param)
        {
            ISimpleSingletonService? service = Bootstrapper.Current?.ServiceProvider?.GetService<ISimpleSingletonService>();
            if (service != null)
            {
                MessageBox.Show(service.SayHello());
            }
        }

        private void TestException(object? param)
        {
            throw new Exception();
        }
    }
}
