﻿namespace AutoMuter
{
    partial class AutoMuter
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIconMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenu;
            this.notifyIcon.Icon = global::AutoMuter.Properties.Resources.mono_mute_UjX_icon;
            this.notifyIcon.Text = "AutoMuter";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIconMenuItem1});
            this.notifyIconMenu.Name = "notifyIconMenu";
            this.notifyIconMenu.Size = new System.Drawing.Size(115, 26);
            this.notifyIconMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.NotifyIconMenu_ItemClicked);
            // 
            // notifyIconMenuItem1
            // 
            this.notifyIconMenuItem1.Name = "notifyIconMenuItem1";
            this.notifyIconMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.notifyIconMenuItem1.Text = "Exit (&E)";
            // 
            // AutoMuter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 282);
            this.Icon = global::AutoMuter.Properties.Resources.mono_mute_UjX_icon;
            this.Name = "AutoMuter";
            this.Text = "AutoMuter";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.AutoMuter_Load);
            this.Shown += new System.EventHandler(this.AutoMuter_Shown);
            this.SizeChanged += new System.EventHandler(this.AutoMuter_SizeChanged);
            this.notifyIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenuItem1;
    }
}