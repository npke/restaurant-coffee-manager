namespace PresentationLayer
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.picUsername = new DevExpress.XtraEditors.PictureEdit();
            this.picPassword = new DevExpress.XtraEditors.PictureEdit();
            this.lblUsername = new DevExpress.XtraEditors.LabelControl();
            this.lblPassword = new DevExpress.XtraEditors.LabelControl();
            this.lblInfo3 = new DevExpress.XtraEditors.LabelControl();
            this.chkRememberPassword = new DevExpress.XtraEditors.CheckEdit();
            this.lblInfo2 = new DevExpress.XtraEditors.LabelControl();
            this.lblInfo1 = new DevExpress.XtraEditors.LabelControl();
            this.picLogo = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRememberPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.EditValue = "";
            this.txtUsername.Location = new System.Drawing.Point(221, 73);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(196, 20);
            this.txtUsername.TabIndex = 10;
            // 
            // txtPassword
            // 
            this.txtPassword.EditValue = "";
            this.txtPassword.Location = new System.Drawing.Point(221, 120);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(196, 20);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(221, 172);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(90, 23);
            this.btnLogin.TabIndex = 12;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(332, 172);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 23);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "Thoát";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Appearance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTitle.Location = new System.Drawing.Point(193, 21);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 19);
            this.lblTitle.TabIndex = 16;
            this.lblTitle.Text = "PHẦN MỀM QUẢN LÝ QUÁN ĂN";
            this.lblTitle.UseMnemonic = false;
            // 
            // picUsername
            // 
            this.picUsername.EditValue = ((object)(resources.GetObject("picUsername.EditValue")));
            this.picUsername.Location = new System.Drawing.Point(195, 73);
            this.picUsername.Name = "picUsername";
            this.picUsername.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picUsername.Properties.Appearance.Options.UseBackColor = true;
            this.picUsername.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picUsername.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picUsername.Size = new System.Drawing.Size(20, 20);
            this.picUsername.TabIndex = 17;
            // 
            // picPassword
            // 
            this.picPassword.EditValue = ((object)(resources.GetObject("picPassword.EditValue")));
            this.picPassword.Location = new System.Drawing.Point(195, 120);
            this.picPassword.Name = "picPassword";
            this.picPassword.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picPassword.Properties.Appearance.Options.UseBackColor = true;
            this.picPassword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picPassword.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picPassword.Size = new System.Drawing.Size(20, 20);
            this.picPassword.TabIndex = 18;
            // 
            // lblUsername
            // 
            this.lblUsername.Appearance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblUsername.Location = new System.Drawing.Point(222, 58);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(72, 13);
            this.lblUsername.TabIndex = 20;
            this.lblUsername.Text = "Tên đăng nhập";
            // 
            // lblPassword
            // 
            this.lblPassword.Appearance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblPassword.Location = new System.Drawing.Point(222, 105);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(44, 13);
            this.lblPassword.TabIndex = 21;
            this.lblPassword.Text = "Mật khẩu";
            // 
            // lblInfo3
            // 
            this.lblInfo3.Appearance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblInfo3.Location = new System.Drawing.Point(377, 214);
            this.lblInfo3.Name = "lblInfo3";
            this.lblInfo3.Size = new System.Drawing.Size(66, 13);
            this.lblInfo3.TabIndex = 23;
            this.lblInfo3.Text = "Phiên bản 1.0";
            // 
            // chkRememberPassword
            // 
            this.chkRememberPassword.Location = new System.Drawing.Point(221, 147);
            this.chkRememberPassword.Name = "chkRememberPassword";
            this.chkRememberPassword.Properties.Caption = "Nhớ mật khẩu";
            this.chkRememberPassword.Size = new System.Drawing.Size(90, 19);
            this.chkRememberPassword.TabIndex = 24;
            // 
            // lblInfo2
            // 
            this.lblInfo2.Appearance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblInfo2.Location = new System.Drawing.Point(154, 214);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(202, 13);
            this.lblInfo2.TabIndex = 25;
            this.lblInfo2.Text = "Đồ án môn Phân tích && Thiết kế Phần mềm";
            // 
            // lblInfo1
            // 
            this.lblInfo1.Appearance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblInfo1.Location = new System.Drawing.Point(12, 214);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(125, 13);
            this.lblInfo1.TabIndex = 26;
            this.lblInfo1.Text = "Khoa Công nghệ thông tin";
            // 
            // picLogo
            // 
            this.picLogo.EditValue = ((object)(resources.GetObject("picLogo.EditValue")));
            this.picLogo.Location = new System.Drawing.Point(24, 20);
            this.picLogo.Name = "picLogo";
            this.picLogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Properties.Appearance.Options.UseBackColor = true;
            this.picLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picLogo.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picLogo.ShowToolTips = false;
            this.picLogo.Size = new System.Drawing.Size(158, 175);
            this.picLogo.TabIndex = 27;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(454, 240);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblInfo1);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.chkRememberPassword);
            this.Controls.Add(this.lblInfo3);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.picPassword);
            this.Controls.Add(this.picUsername);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLogin";
            this.ShowInTaskbar = true;
            this.Text = "Đăng nhập - Phần mềm quản lý quán ăn";
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRememberPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.PictureEdit picUsername;
        private DevExpress.XtraEditors.PictureEdit picPassword;
        private DevExpress.XtraEditors.LabelControl lblUsername;
        private DevExpress.XtraEditors.LabelControl lblPassword;
        private DevExpress.XtraEditors.LabelControl lblInfo3;
        private DevExpress.XtraEditors.CheckEdit chkRememberPassword;
        private DevExpress.XtraEditors.LabelControl lblInfo2;
        private DevExpress.XtraEditors.LabelControl lblInfo1;
        private DevExpress.XtraEditors.PictureEdit picLogo;
    }
}
