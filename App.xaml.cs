using System.Windows;

namespace MyMouseController {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {
        #region Declaration
        private MyMouseControllerMain _controller;
        private HotKeyHelper _hotkey;
        #endregion

        #region Event
        /// <summary>
        /// System.Windows.Application.Startup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this._hotkey = new HotKeyHelper();
            this._controller = new MyMouseControllerMain();
            this._controller.Setup(this._hotkey);
        }

        /// <summary>
        /// System.Windows.Application.Exit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            this._hotkey.Dispose();
            this._controller.Dispose();
        }
        #endregion
    }
}
