using System;
using System.Diagnostics;
using static MyMouseController.WinApis;

namespace MyMouseController {
    public class MainProc : IDisposable {
        // 参考
        // https://github.com/ambyte/GlobalCaretPosition

        #region Declaration
        private readonly object _lock;
        private bool? _disposed;
        #endregion


        #region IDisposable
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructor
        public MainProc() {
            _lock = new object();
            _disposed = false;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// カーソルを移動
        /// </summary>
        /// <param name="isCenter">true:ウィンドウの中央に移動、false:ウィンドウの右上に移動</param>
        public void MoveCursor(bool isCenter) {
            IntPtr window = GetActiveProcess();

            if (_disposed != false) {
                return;
            }

            lock (_lock) {
                var rect = new WinApis.Rect();
                if (!WinApis.NativeMethods.GetWindowRect(window, ref rect)) {
                    return; 
                }
                System.Diagnostics.Debug.WriteLine($"Rect {rect.Left}:{rect.Right}");

                // 既にカーソルがアクティブウィンドウ内にある場合は何もせず
                if (WinApis.NativeMethods.GetCursorPos(out POINT pt)) {
                    System.Diagnostics.Debug.WriteLine($"Mouse Pos {pt.X}:{pt.Y}");

                    if (isCenter && rect.Left <= pt.X && pt.X <= rect.Right &&
                        rect.Top <= pt.Y && pt.Y <= rect.Bottom) {
                        System.Diagnostics.Debug.WriteLine("Do not move");
                        return;
                    }
                }

                if (isCenter) {
                    var x = rect.Left + (rect.Right - rect.Left) / 2;
                    var y = rect.Top + (rect.Bottom - rect.Top) / 2;
                    WinApis.NativeMethods.SetCursorPos(x, y);
                    System.Diagnostics.Debug.WriteLine($"Move Center x;{x}, y:{y}");
                } else {
                    var x = rect.Right - 25;
                    var y = rect.Top + 10;
                    WinApis.NativeMethods.SetCursorPos(x, y);
                    System.Diagnostics.Debug.WriteLine($"Move Right x;{x}, y:{y}");
                }
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 後処理
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing) {
            if (_disposed == false) {
                _disposed = null;

                if (disposing) {
                    // dispose managed resources
                }

                lock (_lock) {
                    //dispose unmanaged resources
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// アクティブウィンドウのハンドルを取得する
        /// </summary>
        /// <returns>アクティブウィンドウのハンドル</returns>
        private IntPtr GetActiveProcess() {
            //IntPtr hWnd = WinApis.NativeMethods.GetForegroundWindow();

            //WinApis.NativeMethods.GetWindowThreadProcessId(hWnd, out int id);
            //Process process = Process.GetProcessById(id);
            //System.Diagnostics.Debug.WriteLine($"process {process.ProcessName}");
            //return process.Handle;
            return WinApis.NativeMethods.GetForegroundWindow();
        }
        #endregion
    }
}
