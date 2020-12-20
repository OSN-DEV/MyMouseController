using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace MyMouseController {
    public partial class MyMouseControllerMain : Component {

        #region Declaration
        private MainProc _proc = new MainProc();
        private readonly NotifyIcon _notifyIcon = new NotifyIcon();
        #endregion

        #region Constructor
        public MyMouseControllerMain() {
            InitializeComponent();
            this.setup();
        }

        public MyMouseControllerMain(IContainer container) {
            container.Add(this);
            InitializeComponent();
            this.setup();
        }
        #endregion

        #region Event
        private void cMenu_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem == this.cMenu_Exit) {
                this._proc.Dispose();
                this._proc = null;
                Application.Current.Shutdown();
            } else {
                if (cMenu_Start.Checked) {
                    this._proc.Stop();
                    this.showToast("Stop");
                } else {
                    this._proc.Start();
                    this.showToast("Start");
                }
            }
        }
        #endregion

        #region Public Method
        public void setup(HotKeyHelper helper) {
            helper.setup(this.cMenu.Handle);
            helper.Register(ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt, Key.I, (_, __) => {
                this.cMenu_Start.PerformClick();
            });
        }
        #endregion

        #region Private Method 
        private void setup() {
            _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            _notifyIcon.BalloonTipClosed += (s, e) => _notifyIcon.Visible = false;
        }

        private void showToast(string message) {
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(3000, "MyMouseController", message, ToolTipIcon.Info);

            var timer = new Timer();
            timer.Interval = 2000;
            timer.Start();
            timer.Tick += (s, e) => {
                ((Timer)s).Stop();
                _notifyIcon.Visible = false;
            };

        }
        #endregion
    }
}
