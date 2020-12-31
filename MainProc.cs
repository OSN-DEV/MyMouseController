using System;
using System.Runtime.InteropServices;
using static MyMouseController.WinApis;

namespace MyMouseController {
    public class MainProc : IDisposable {
        // 参考
        // https://github.com/ambyte/GlobalCaretPosition

        #region Declaration
        private bool _isStarted;
        private object _lock;
        private bool? _disposed;
        private IntPtr _hook;
        private GCHandle _handleHook;
        #endregion


        #region IDisposable
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructor
        public MainProc() {
            _isStarted = false;
            _lock = new object();
            _disposed = false;
            _handleHook = GCHandle.Alloc(new SetWinEventHookDelegate(HandleHook));
        }
        #endregion

        #region Public Method
        public void Start() {
            lock (_lock) {
                if (_isStarted) {
                    throw new InvalidOperationException("Already started");
                }

                _hook = WinApis.NativeMethods.SetWinEventHook(SetWinEventHookEventType.EVENT_SYSTEM_FOREGROUND,
                    SetWinEventHookEventType.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero,
                    (SetWinEventHookDelegate)_handleHook.Target,
                    0, 0,
                    SetWinEventHookFlag.WINEVENT_OUTOFCONTEXT | SetWinEventHookFlag.WINEVENT_SKIPOWNPROCESS);

                if (_hook == IntPtr.Zero) {
                    throw new Exception("Unable to set event hook:" + Marshal.GetLastWin32Error());
                }

                _isStarted = true;
            }
        }

        public void Stop() {
            lock (_lock) {
                if (!_isStarted) {
                    throw new InvalidOperationException("Already stopped");
                }

                if (!WinApis.NativeMethods.UnhookWinEvent(_hook)) {
                    throw new Exception("Unable to remove event hook:" + Marshal.GetLastWin32Error());
                }

                _isStarted = false;

            }
        }
        #endregion

        #region Private Method
        private void Dispose(bool disposing) {
            if (_disposed == false) {
                _disposed = null;

                if (disposing) {
                    // dispose managed resources
                }

                //dispose unmanaged resources
                lock (_lock) {
                    if (_isStarted) {
                        WinApis.NativeMethods.UnhookWinEvent(_hook);
                    }
                }
                _handleHook.Free();

                _disposed = true;
            }
        }

        private void HandleHook(IntPtr hook, SetWinEventHookEventType eventType,
            IntPtr window, int objectId, int childId, uint threadId, uint time) {
            if (_disposed != false) {
                return;
            }

            lock (_lock) {
                var rect = new WinApis.Rect();
                if (WinApis.NativeMethods.GetWindowRect(window, ref rect)) {
                    POINT pt;
                    if (WinApis.NativeMethods.GetCursorPos(out pt)) {
                        if (rect.Left <= pt.X && pt.X <= rect.Right &&
                            rect.Top <= pt.Y && pt.Y <= rect.Bottom) {
                            return;
                        }
                    }

                    var x = rect.Left + (rect.Right - rect.Left) / 2;
                    var y = rect.Top + (rect.Bottom - rect.Top) / 2;
                     WinApis.NativeMethods.SetCursorPos(x, y);
                }
            }
        }
        #endregion
    }
}
