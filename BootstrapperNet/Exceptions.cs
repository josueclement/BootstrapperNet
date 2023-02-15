using System;

namespace BootstrapperNet
{
    /// <summary>
    /// Missing main window exception
    /// </summary>
    public class MissingMainWindowException : Exception
    {
        /// <summary>
        /// Constructor for <see cref="MissingMainWindowException"/>
        /// </summary>
        public MissingMainWindowException()
            : base("Main window is null")
        { }
    }

    /// <summary>
    /// Missing splash screen exception
    /// </summary>
    public class MissingSplashScreenException : Exception
    {
        /// <summary>
        /// Constructor for <see cref="MissingSplashScreenException"/>
        /// </summary>
        public MissingSplashScreenException()
            : base("Splash screen is null")
        { }
    }
}
