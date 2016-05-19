using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using System.Data.SqlClient;
using System.Security.Cryptography;
using DataTransferObject;
using BusinessLogicLayer;

namespace PresentationLayer
{
    public partial class frmLogin : SplashScreen
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        // Xử lý sự kiện đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Tạo tài khoản với thông tin đăng nhập từ người dùng
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            username = username.ToUpper();
            password = AccountBus.GetMD5Hash(password);

            Account account = new Account(username, password);

            try
            {
                // Kiểm tra thông tin tài khoản
                Account loginAccount = AccountBus.Login(account);
                if (loginAccount != null)
                {
                    // Mở form quản lý
                    this.Hide();
                    //frmDashboard Dashboard = new frmDashboard(loginAccount);
                    frmDashboard Dashboard = frmDashboard.getInstance(loginAccount);
                    Dashboard.Show();
                }
                else
                {
                    // Thông báo lỗi đăng nhập
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.\nVui lòng kiểm tra lại.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Thoát chương trình
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // Đăng nhập khi bấm phím enter 
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}