﻿using System;
using System.Diagnostics;
using static MyMouseController.WinApis;
using System.Windows.Forms;

namespace MyMouseController {
    public class MainProc : IDisposable {
        // 参考
        // https://github.com/ambyte/GlobalCaretPosition

        #region Declaration
        private readonly object _lock;
        private bool? _disposed;
        private readonly int offset = 30;

        public enum MoveDirection {
            LeftTop,
            LeftMiddle,
            LeftBottom,
            CenterTop,
            CenterMiddle,
            CenterBottom,
            RightTop,
            RightMiddle,
            RightBottom
        };
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

        /// <summary>
        /// カーソルを別のモニタに移動
        /// </summary>
        /// <param name="isRight">ture:右のモニタに移動、false:左のモニタに移動</param>
        /// <remarks>Y座標が同じ場合はY座標が上の方を左と判断。モニタは２台の前提</remarks>
        public void MoveCursorToOtherScreen(bool isRight) {

            if (2 != Screen.AllScreens.Length) {
                return;
            }


            Screen leftScreen;
            Screen rightScreen;
            if (Screen.AllScreens[0].Bounds.Left == Screen.AllScreens[1].Bounds.Left) {
                if (Screen.AllScreens[0].Bounds.Top < Screen.AllScreens[1].Bounds.Top) {
                    leftScreen = Screen.AllScreens[1];
                    rightScreen = Screen.AllScreens[0];
                } else {
                    leftScreen = Screen.AllScreens[0];
                    rightScreen = Screen.AllScreens[1];
                }
            } else if (Screen.AllScreens[0].Bounds.Left < Screen.AllScreens[1].Bounds.Left) {
                leftScreen = Screen.AllScreens[0];
                rightScreen = Screen.AllScreens[1];
            } else {
                leftScreen = Screen.AllScreens[1];
                rightScreen = Screen.AllScreens[0];
            }
            Screen screen = isRight ? rightScreen : leftScreen;

            if (_disposed != false) {
                return;
            }

            lock (_lock) {
                Debug.WriteLine($"Rect {screen.Bounds.Left}:{screen.Bounds.Right}");

                // 既にカーソルがアクティブウィンドウ内にある場合は何もせず
                if (WinApis.NativeMethods.GetCursorPos(out POINT pt)) {
                    System.Diagnostics.Debug.WriteLine($"Mouse Pos {pt.X}:{pt.Y}");

                    if (screen.Bounds.Left <= pt.X && pt.X <= screen.Bounds.Right &&
                        screen.Bounds.Top <= pt.Y && pt.Y <= screen.Bounds.Bottom) {
                        Debug.WriteLine("Do not move");
                        return;
                    }
                }

                var x = screen.Bounds.Left + (screen.Bounds.Right - screen.Bounds.Left) / 2;
                var y = screen.Bounds.Top + (screen.Bounds.Bottom - screen.Bounds.Top) / 2;
                WinApis.NativeMethods.SetCursorPos(x, 0);
                WinApis.NativeMethods.SetCursorPos(x, y);
                System.Diagnostics.Debug.WriteLine($"Move Center x;{x}, y:{y}");
            }
        }

        /// <summary>
        /// 画面の特定の位置にカーソルを移動する
        /// </summary>
        /// <param name="direction"></param>
        public void SimpleMoveCursor(MoveDirection direction) {

            lock (_lock) {
                // カーソルのある画面を取得
                Screen targetScreen = null;
                foreach (var screen in Screen.AllScreens) {
                    if (WinApis.NativeMethods.GetCursorPos(out POINT pt)) {
                        System.Diagnostics.Debug.WriteLine($"Mouse Pos {pt.X}:{pt.Y}");

                        if (screen.Bounds.Left <= pt.X && pt.X <= screen.Bounds.Right &&
                            screen.Bounds.Top <= pt.Y && pt.Y <= screen.Bounds.Bottom) {
                            targetScreen = screen;
                            break;
                        }
                    }
                }

                if (null == targetScreen) {
                    return;
                }

                int x = 0;
                     int y = 0;
                switch(direction) {
                    case MoveDirection.LeftTop:
                        x = 0 + targetScreen.Bounds.Left + offset;
                        y = 0 + offset;
                        break;
                    case MoveDirection.LeftMiddle:
                        x = 0 + targetScreen.Bounds.Left + offset;
                        y = targetScreen.Bounds.Height / 2;
                        break;
                    case MoveDirection.LeftBottom:
                        x = 0 + targetScreen.Bounds.Left + offset;
                        y = targetScreen.Bounds.Height - offset;
                        break;
                    case MoveDirection.CenterTop:
                        x = targetScreen.Bounds.Width / 2 + targetScreen.Bounds.Left;
                        y = 0 + offset;
                        break;
                    case MoveDirection.CenterMiddle:
                        x = targetScreen.Bounds.Width / 2 + targetScreen.Bounds.Left;
                        y = targetScreen.Bounds.Height / 2;
                        break;
                    case MoveDirection.CenterBottom:
                        x = targetScreen.Bounds.Width / 2 + targetScreen.Bounds.Left;
                        y = targetScreen.Bounds.Height - offset;
                        break;
                    case MoveDirection.RightTop:
                        x = targetScreen.Bounds.Width - offset + targetScreen.Bounds.Left;
                        y = 0 + offset;
                        break;
                    case MoveDirection.RightMiddle:
                        x = targetScreen.Bounds.Width - offset + targetScreen.Bounds.Left;
                        y = targetScreen.Bounds.Height / 2;
                        break;
                    case MoveDirection.RightBottom:
                        x = targetScreen.Bounds.Width - offset + targetScreen.Bounds.Left;
                        y = targetScreen.Bounds.Height - offset;
                        break;
                }

                WinApis.NativeMethods.SetCursorPos(x, 0);
                WinApis.NativeMethods.SetCursorPos(x, y);
                System.Diagnostics.Debug.WriteLine($"Move Center x;{x}, y:{y}");
            }
        }

        /// <summary>
        /// マウスの左クリックイベントを発生させる
        /// </summary>
        public void MouseClick() {
            WinApis.NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            WinApis.NativeMethods.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
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
