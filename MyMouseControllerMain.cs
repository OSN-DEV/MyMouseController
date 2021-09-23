using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace MyMouseController {
    public partial class MyMouseControllerMain : Component {

        #region Declaration
        private MainProc _proc = new MainProc();
        #endregion

        #region Constructor
        public MyMouseControllerMain() {
            InitializeComponent();
        }

        public MyMouseControllerMain(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }
        #endregion

        #region Event
        private void MenuItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem == this.cMenu_Exit) {
                this._proc.Dispose();
                this._proc = null;
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 初期設定を行う
        /// </summary>
        /// <param name="helper">ホットキーヘルパー</param>
        public void Setup(HotKeyHelper helper) {
            helper.setup(this.cMenu.Handle);
            helper.Register(ModifierKeys.Control | ModifierKeys.Shift, Key.Z, (_, __) => {
                MoveCursor(true);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Shift, Key.X, (_, __) => {
                MoveCursor(false);
            });
        }
        #endregion

        #region Private Method 
        /// <summary>
        /// カーソルを移動
        /// </summary>
        /// <param name="isCenter">true:ウィンドウの中央に移動、false:ウィンドウの右上に移動</param>
        private void MoveCursor(bool isCenter) {
            this._proc.MoveCursor(isCenter);
        }
        #endregion
    }
}
