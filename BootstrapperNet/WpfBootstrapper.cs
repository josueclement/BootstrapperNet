using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BootstrapperNet
{
    /// <summary>
    /// Bootstrapper for Wpf applications
    /// </summary>
    public abstract class WpfBootstrapper : Bootstrapper
    {
        #region Properties

        /// <summary>
        /// Gets or sets wether the splash screen is enabled
        /// </summary>
        public virtual bool IsSplashScreenEnabled { get; set; }

        /// <summary>
        /// Gets or sets the splash screen duration
        /// </summary>
        public virtual TimeSpan SplashScreenDuration { get; set; } = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Splash screen window
        /// </summary>
        public Window? SplashScreenWindow { get; protected set; }

        /// <summary>
        /// Main window
        /// </summary>
        public Window? MainWindow { get; protected set; }

        /// <summary>
        /// Action invoked when SplashScreenWindow is displayed
        /// </summary>
        public virtual Action? SplashScreenAction { get; set; }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public override void Run(bool registerUnhandledExceptions = true)
        {
            RaiseStartingEvent();
            ConfigureServices();

            if (registerUnhandledExceptions)
                RegisterUnhandledExceptions();

            ShowSplashScreenAndWindow();
            RaiseStartedEvent();
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
            base.Shutdown();

            Application.Current.Shutdown();
        }

        #endregion

        #region Unhandled exceptions methods

        /// <inheritdoc/>
        protected override void RegisterUnhandledExceptions()
        {
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            base.RegisterUnhandledExceptions();
        }

        /// <inheritdoc/>
        protected override void UnregisterUnhandledExceptions()
        {
            Application.Current.DispatcherUnhandledException -= OnDispatcherUnhandledException;

            base.UnregisterUnhandledExceptions();
        }

        /// <summary>
        /// Method called when <see cref="Application.DispatcherUnhandledException"/> is raised
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Dispatcher unhandled exception args</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            RaiseUnhandledExceptionEvent(e.Exception);
        }

        #endregion

        #region Splash screen and Main window

        /// <summary>
        /// Create a new splash screen window<br />
        /// Override this method to create your own splash screen window
        /// </summary>
        /// <returns>Splash screen window</returns>
        protected virtual Window CreateSplashScreenWindow() => new Window();

        /// <summary>
        /// Create a new main window<br />
        /// Override this method to create your own main window
        /// </summary>
        /// <returns>Main window</returns>
        protected abstract Window CreateMainWindow();

        /// <summary>
        /// Show splash screen and main window
        /// </summary>
        protected virtual void ShowSplashScreenAndWindow()
        {
            MainWindow = CreateMainWindow();

            if (IsSplashScreenEnabled)
            {
                SplashScreenWindow = CreateSplashScreenWindow();
                SplashScreenWindow.Loaded += SplashScreenWindow_Loaded;
                SplashScreenWindow.Show();
            }
            else
            {
                MainWindow.Show();
                MainWindow.Closing += OnMainWindowClosing;
            }
        }

        private async void SplashScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SplashScreenWindow!.Loaded -= SplashScreenWindow_Loaded;

            if (SplashScreenAction != null)
                await Task.Run(SplashScreenAction);

            DateTime splashScreenEnd = DateTime.Now + SplashScreenDuration;

            await Task.Run(() =>
            {
                while (DateTime.Now < splashScreenEnd)
                {
                    Thread.Sleep(200);
                }
            });

            SplashScreenWindow.Close();
            MainWindow!.Show();
            MainWindow.Closing += OnMainWindowClosing;
        }

        /// <summary>
        /// Occurs when the main window is closing
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Cancel event args</param>
        protected virtual void OnMainWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainWindow != null)
                MainWindow.Closing -= OnMainWindowClosing;

            Shutdown();
        }

        #endregion
    }
}
