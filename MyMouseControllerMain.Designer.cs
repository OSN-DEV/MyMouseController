namespace MyMouseController {
    partial class MyMouseControllerMain {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyMouseControllerMain));
            this.cNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenu.SuspendLayout();
            // 
            // cNotify
            // 
            this.cNotify.ContextMenuStrip = this.cMenu;
            this.cNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("cNotify.Icon")));
            this.cNotify.Text = "MyMouseController";
            this.cNotify.Visible = true;
            // 
            // cMenu
            // 
            this.cMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenu_Exit});
            this.cMenu.Name = "cMenu";
            this.cMenu.Size = new System.Drawing.Size(99, 54);
            this.cMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuItemClicked);
            // 
            // cMenu_Exit
            // 
            this.cMenu_Exit.Name = "cMenu_Exit";
            this.cMenu_Exit.Size = new System.Drawing.Size(98, 22);
            this.cMenu_Exit.Text = "Exit";
            this.cMenu.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon cNotify;
        private System.Windows.Forms.ContextMenuStrip cMenu;
        private System.Windows.Forms.ToolStripMenuItem cMenu_Exit;
    }
}
