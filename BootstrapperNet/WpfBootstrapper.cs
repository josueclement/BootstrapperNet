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
        /// Main window
        /// </summary>
        public abstract Window? MainWindow { get; }

        /// <summary>
        /// Gets or sets wether the splash screen is enabled
        /// </summary>
        public abstract bool IsSplashScreenEnabled { get; }

        /// <summary>
        /// Splash screen window
        /// </summary>
        public virtual Window? SplashScreenWindow { get; }

        /// <summary>
        /// Action invoked when SplashScreenWindow is displayed
        /// </summary>
        public virtual Action? SplashScreenAction { get; }

        /// <summary>
        /// Gets or sets the splash screen duration
        /// </summary>
        public virtual TimeSpan SplashScreenDuration { get; } = TimeSpan.FromSeconds(2);

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
        /// Show main window
        /// </summary>
        /// <exception cref="MissingMainWindowException"></exception>
        protected void ShowMainWindow()
        {
            if (MainWindow == null)
                throw new MissingMainWindowException();

            MainWindow.Show();
            MainWindow.Closing += OnMainWindowClosing;
        }

        /// <summary>
        /// Show splash screen and main window
        /// </summary>
        protected void ShowSplashScreenAndWindow()
        {
            if (IsSplashScreenEnabled)
            {
                if (SplashScreenWindow == null)
                    throw new MissingSplashScreenException();

                SplashScreenWindow.Loaded += SplashScreenWindow_Loaded;
                SplashScreenWindow.Show();
            }
            else
            {
                ShowMainWindow();
            }
        }

        private async void SplashScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SplashScreenWindow!.Loaded -= SplashScreenWindow_Loaded;
            
            DateTime splashScreenEnd = DateTime.Now + SplashScreenDuration;

            if (SplashScreenAction != null)
                await Task.Run(SplashScreenAction);

            await Task.Run(() =>
            {
                while (DateTime.Now < splashScreenEnd)
                {
                    Thread.Sleep(200);
                }
            });

            SplashScreenWindow.Close();
            ShowMainWindow();
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
