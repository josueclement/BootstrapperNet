using System;
using System.Reflection;
using System.Windows;

namespace BootstrapperNetTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                new AppBootstrapper().Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Assembly.GetExecutingAssembly().GetName().Name} fatal error:\r\n\r\n {ex}");
                Environment.Exit(1);
            }

            base.OnStartup(e);
        }
    }
}
