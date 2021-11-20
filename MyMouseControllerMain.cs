using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using static MyMouseController.MainProc;
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
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.Left, (_, __) => {
                MoveCursorToOtherScreen(false);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.Right, (_, __) => {
                MoveCursorToOtherScreen(true);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.U, (_, __) => {
                SimpleMoveCursor(MoveDirection.LeftTop);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.I, (_, __) => {
                SimpleMoveCursor(MoveDirection.CenterTop);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.O, (_, __) => {
                SimpleMoveCursor(MoveDirection.RightTop);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.J, (_, __) => {
                SimpleMoveCursor(MoveDirection.LeftMiddle);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.K, (_, __) => {
                SimpleMoveCursor(MoveDirection.CenterMiddle);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.L, (_, __) => {
                SimpleMoveCursor(MoveDirection.RightMiddle);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.M, (_, __) => {
                SimpleMoveCursor(MoveDirection.LeftBottom);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.OemComma, (_, __) => {
                SimpleMoveCursor(MoveDirection.CenterBottom);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.OemPeriod, (_, __) => {
                SimpleMoveCursor(MoveDirection.RightBottom);
            });
            helper.Register(ModifierKeys.Control | ModifierKeys.Alt, Key.N, (_, __) => {
                this._proc.MouseClick();
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

        /// <summary>
        /// カーソルを別のモニタに移動
        /// </summary>
        /// <param name="isRight">ture:右のモニタに移動、false:左のモニタに移動</param>
        private void MoveCursorToOtherScreen(bool isRight) {
            this._proc.MoveCursorToOtherScreen(isRight);
        }

        /// <summary>
        /// 画面内のカーソル移動
        /// </summary>
        /// <param name="direction">移動箇所</param>
        private void SimpleMoveCursor(MoveDirection direction) {
            this._proc.SimpleMoveCursor(direction);
        }
        #endregion
    }
}
