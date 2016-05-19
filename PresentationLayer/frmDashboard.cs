using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DataTransferObject;
using BusinessLogicLayer;
using System.Collections;

namespace PresentationLayer
{
    public partial class frmDashboard : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        // Cài đặt mẫu thiết kế hướng đối tượng Singleton
        private static frmDashboard _dashboard;

        public static frmDashboard getInstance(Account loginAccount)
        {
            if (_dashboard == null)
                _dashboard = new frmDashboard(loginAccount);

            return _dashboard;
        }

        // Tài khoản đã đăng nhập
        Account loggedAccount = new Account();

        // Danh sách các khu hiện tại
        ArrayList sectionList = null;

        // Danh sách các menu hiện tại
        ArrayList menuList = null;
        ArrayList menuList2 = null;

        // Danh sách các tài khoản
        ArrayList accountList = null;

        // Số lượng món gọi
        int SoLuongMonGoi = 1;

        // Bàn đã chọn
        Table selectedTable = null;
        SimpleButton btnSelectedTable = null;

        // Hóa đơn gần nhất (của bàn đã chọn)
        Invoice recentInvoice = null;

        int OldMenuID = 0;

        private frmDashboard()
        {
            InitializeComponent();
        }


        private frmDashboard(Account loginAccount)
        {
            InitializeComponent();
            loggedAccount = loginAccount;
        }


        // Sự kiện Form load
        private void Dashboard_Load(object sender, EventArgs e)
        {
            // Lấy và thiết lập danh sách các khu
            SetSectionList();

            // Lấy và thiết lập danh sách thực đơn
            SetMenuList();

            // Lấy danh sách tài khoản và đưa lên dataGridView
            accountList = AccountBus.GetAccountList();
            dgvAccount.DataSource = accountList;

            // Chỉnh độ rộng các cột của dataGridView
            CustomizeDataGridView();

            // Thiết lập thông tin tài khoản người đăng nhập
            setAccountInfo();
        }


        // Thiết lập thuộc tính độ rộng các cột của dataGidView
        private void CustomizeDataGridView()
        {
            dgvDSBan.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDSThucDon.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDSMon.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvThucDon.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDSMonCuaBan.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDSKhu.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAccount.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDSMonHD.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHoaDon.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }


        // Sự kiện đóng form
        private void frmDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        #region CÁC CÀI ĐẶT XỬ LÝ MÀN HÌNH CHÍNH

        // Thiết lập danh sách thực đơn
        private void SetMenuList()
        {
            try
            {
                menuList = MenuBus.GetMenuList();
                menuList2 = MenuBus.GetMenuList();

                dgvDSThucDon.DataSource = menuList;

                cmbDSThucDon.DataSource = menuList;
                cmbDSThucDon.DisplayMember = "Name";
                cmbDSThucDon.ValueMember = "ID";
                cmbDSThucDon.SelectedIndex = 0;

                cmbDSThucDon2.DataSource = menuList;
                cmbDSThucDon2.DisplayMember = "Name";
                cmbDSThucDon2.ValueMember = "ID";
                cmbDSThucDon2.SelectedIndex = 0;

                cmbDSThucDon3.DataSource = menuList2;
                cmbDSThucDon3.DisplayMember = "Name";
                cmbDSThucDon3.ValueMember = "ID";
                cmbDSThucDon3.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Không tải được danh sách thực đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thiết lập danh sách các khu trong combobox
        private void SetSectionList()
        {
            try
            {
                sectionList = SectionBus.GetSectionList();

                dgvDSKhu.DataSource = sectionList;


                cmbDSKhu.DataSource = sectionList;
                cmbDSKhu.DisplayMember = "Name";
                cmbDSKhu.ValueMember = "ID";
                cmbDSKhu.SelectedIndex = 0;

                cmbDSKhu2.DataSource = sectionList;
                cmbDSKhu2.DisplayMember = "Name";
                cmbDSKhu2.ValueMember = "ID";
                cmbDSKhu2.SelectedIndex = 0;


                cmbDSKhu3.DataSource = sectionList;
                cmbDSKhu3.DisplayMember = "Name";
                cmbDSKhu3.ValueMember = "ID";
                cmbDSKhu3.SelectedIndex = 0;

                cmbDSKhuHD.DataSource = sectionList;
                cmbDSKhuHD.DisplayMember = "Name";
                cmbDSKhuHD.ValueMember = "ID";
                cmbDSKhuHD.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Không tải được danh sách khu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Load danh sách các bàn trong một khu lên màn hình
        private void LoadTableList(int sectionID)
        {
            try
            {
                ArrayList tableListInSection = TableBus.GetTableListInSection(sectionID);

                for (int i = 0; i < tableListInSection.Count; i++)
                {
                    Table table = (Table)tableListInSection[i];

                    // Thiết lập các thuộc tính cho bàn (Dùng button để minh họa cho bàn)
                    SimpleButton btnTable = new SimpleButton();
                    btnTable.Name = table.ID.ToString();
                    btnTable.Text = table.Name;

                    Bitmap bitMap = new Bitmap("table2.png");
                    btnTable.Image = bitMap;
                    btnTable.ImageLocation = ImageLocation.TopCenter;

                    btnTable.Height = 75;
                    btnTable.Width = 85;

                    // Trạng thái bàn
                    // Chữ đỏ: Có khách
                    // Chữ xanh: Trống
                    if (!table.Status)
                    {
                        btnTable.ForeColor = Color.Red;
                    }
                    else
                    {
                        btnTable.ForeColor = Color.Blue;
                    }

                    // Thiết lập sự kiện
                    btnTable.Click += btnTable_Click;

                    // Thêm vào container để thể hiện trên màn hình
                    dsBanContainer.Controls.Add(btnTable);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Tải danh sách bàn thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Sự kiện khi click vào một bàn
        // -> Hiển thị thông tin hóa đơn bàn đó
        void btnTable_Click(object sender, EventArgs e)
        {
            try
            {
                btnSelectedTable = (SimpleButton)sender;
                selectedTable = TableBus.GetTable(int.Parse(btnSelectedTable.Name));
                lblTenBanDaChon.Text = selectedTable.Name;
                lblTenKhuCuaBan.Text = cmbDSKhu.GetItemText(cmbDSKhu.SelectedItem);
                lblNDGhiChu.Text = selectedTable.Description;

                txtSLMon.Text = "1";

                // Bàn chưa mở
                if (selectedTable.Status)
                {
                    btnDongMoBan.Text = "Mở bàn";
                    lblTongTien.Text = "0";
                    dgvDSMonCuaBan.DataSource = OrderBus.GetOrder(-1);
                    lblTrangThaiThanhToan.Text = "Chưa thanh toán";

                    txtTienKhachDua.Text = "0";
                    lblTienTraLai.Text = "0";
                    lblThoiGianMo.Text = "Chưa mở";
                }
                else // Bàn đã mở thì cập nhật hóa đơn bàn này
                {
                    UpdateInvoice();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi tải bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức cập nhật lại danh sách các món ăn của hóa đơn hiện tại được chọn
        private void UpdateInvoice()
        {
            try
            {
                // Lấy hóa đơn gần nhất
                Invoice lastestInvoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);
                lblThoiGianMo.Text = lastestInvoice.DateCreated.ToString();
                lblNDGhiChu.Text = selectedTable.Description;

                if (lastestInvoice == null)
                    return;

                // Lấy danh sách các món ăn của hóa đơn đó
                dgvDSMonCuaBan.DataSource = OrderBus.GetOrder(lastestInvoice.ID);

                // Tính tổng tiền các món
                int tongTien = 0;
                for (int i = 0; i < dgvDSMonCuaBan.RowCount; i++)
                {
                    tongTien += int.Parse(dgvDSMonCuaBan.Rows[i].Cells["ColThanhTien"].Value.ToString());
                }

                lblTongTien.Text = String.Format("{0:#,##0.##}", tongTien);

                btnDongMoBan.Text = "Đóng bàn";

                // Cập nhật các thông tin thanh toán
                if (lastestInvoice.Paid)
                    lblTrangThaiThanhToan.Text = "Đã thanh toán";
                else
                    lblTrangThaiThanhToan.Text = "Chưa thanh toán";

                txtTienKhachDua.Text = String.Format("{0:#,##0.##}", lastestInvoice.Cash);
                lblTienTraLai.Text = String.Format("{0:#,##0.##}", lastestInvoice.Charge);

                Invoice invoice = new Invoice();
                invoice.ID = lastestInvoice.ID;
                invoice.Total = tongTien;
                InvoiceBus.UpdateTotal(invoice);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi cập nhật thông tin hóa đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Khi người dùng chọn một khu mới thì load danh sách bàn trong khu đó
        private void cmbDSKhu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSection = 0;

                var selected = cmbDSKhu.SelectedValue;
                if (selected is Section)
                {
                    Section selectedSection = (DataTransferObject.Section)selected;
                    idSection = selectedSection.ID;
                }
                else
                    idSection = int.Parse(cmbDSKhu.SelectedValue.ToString());
                dsBanContainer.Controls.Clear();
                LoadTableList(idSection);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Tải danh sách bàn trong khu thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Khi người dùng chọn một thực đơn mới thì load danh sách món trong thực đơn đó
        private void cmbDSThucDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idMenu = 0;
                var selected = cmbDSThucDon.SelectedValue;
                if (selected is DataTransferObject.Menu)
                {
                    DataTransferObject.Menu selectedMenu = (DataTransferObject.Menu)selected;
                    idMenu = selectedMenu.ID;
                }
                else
                    idMenu = int.Parse(selected.ToString());

                DataTable itemList = ItemBus.GetItemListInMenu(idMenu);
                dgvThucDon.DataSource = itemList;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Không lấy được danh sách món của thực đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Tăng số lượng món khi chọn để thêm vào từ thực đơn
        private void btnTangSLMon_Click(object sender, EventArgs e)
        {
            SoLuongMonGoi++;
            txtSLMon.Text = SoLuongMonGoi.ToString();
        }


        // Giảm số lượng món khi chọn để thêm vào từ thực đơn
        private void btnGiamSLMon_Click(object sender, EventArgs e)
        {
            try
            {
                SoLuongMonGoi = Int16.Parse(txtSLMon.Text);

                if (SoLuongMonGoi == 1)
                {
                    MessageBox.Show("Số lượng món gọi không được nhỏ hơn 1", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SoLuongMonGoi--;
                    txtSLMon.Text = SoLuongMonGoi.ToString();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Số lượng món nhập không đúng.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Thêm số thứ tự của món trên DataGridView thực đơn 
        private void dgvThucDon_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvThucDon.RowCount; i++)
                dgvThucDon.Rows[i].Cells[0].Value = i + 1;
        }


        // Thêm số thứ tự của món trên DataGridView hóa đơn của bàn 
        private void dgvDSMonCuaBan_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvDSMonCuaBan.RowCount; i++)
                dgvDSMonCuaBan.Rows[i].Cells[0].Value = i + 1;
        }


        // Thêm món từ thực đơn vào hóa đơn với số lượng đã chọn
        private void btnThemMonVaoHD_Click(object sender, EventArgs e)
        {
            // Nếu chưa chọn bàn
            if (selectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món", "Chưa chọn bàn");
                return;
            }

            // Nếu bàn chưa được mở
            if (selectedTable.Status)
            {
                DialogResult moBan = MessageBox.Show(selectedTable.Name + " chưa được mở. Mở bàn và thêm món?", "Bàn chưa mở", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                // Mở bàn, tạo hóa đơn mới, cập nhật trạng thái bàn
                if (moBan == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        int sectionID = 0;

                        var selected = cmbDSKhu.SelectedValue;
                        if (selected is Section)
                        {
                            Section selectedSection = (DataTransferObject.Section)selected;
                            sectionID = selectedSection.ID;
                        }
                        else
                            sectionID = int.Parse(cmbDSKhu.SelectedValue.ToString());

                        selectedTable.Status = false;
                        TableBus.Update(selectedTable);
                        Invoice invoice = new Invoice();
                        
                        invoice.SectionID = sectionID;
                        invoice.ID = InvoiceBus.GetNextID();
                        invoice.TableID = selectedTable.ID;
                        invoice.DateCreated = DateTime.Now;

                        InvoiceBus.Insert(invoice);

                        btnSelectedTable.ForeColor = Color.Red;
                        btnDongMoBan.Text = "Đóng bàn";
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Lỗi thêm món vào hóa đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    return;
            }

            // Lấy hóa đơn mới nhất của bàn đó
            Invoice lastestInvoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);

            if (lastestInvoice.Paid)
            {
                MessageBox.Show("Bàn đã thanh toán, không thể thêm món.", "Lỗi thêm món", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Tạo order và thêm món vào hóa đơn
                Order order = new Order();

                order.ID = OrderBus.GetNextID();
                order.InvoiceID = lastestInvoice.ID;
                order.ItemID = int.Parse(dgvThucDon.SelectedRows[0].Cells["ID"].Value.ToString());
                order.Quantity = int.Parse(txtSLMon.Text);

                OrderBus.Insert(order);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi thêm món vào hóa đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Cập nhật lại danh sách món trong hóa đơn 
            UpdateInvoice();
        }


        // Xử lý đóng mở bàn
        private void btnDongMoBan_Click(object sender, EventArgs e)
        {
            if (selectedTable == null)
                return;

            // Nếu bàn đang trống (có thể mở)
            if (selectedTable.Status)
            {
                DialogResult moBan = MessageBox.Show("Mở " + selectedTable.Name + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                // Nếu đồng ý mở thì cập nhật trạng thái bàn và tạo một hóa đơn mới cho bàn đó
                if (moBan == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        int sectionID = 0;

                        var selected = cmbDSKhu.SelectedValue;
                        if (selected is Section)
                        {
                            Section selectedSection = (DataTransferObject.Section)selected;
                            sectionID = selectedSection.ID;
                        }
                        else
                            sectionID = int.Parse(cmbDSKhu.SelectedValue.ToString());

                        selectedTable.Status = false;
                        btnSelectedTable.ForeColor = Color.Red;
                        btnDongMoBan.Text = "Đóng bàn";

                        TableBus.Update(selectedTable);

                        Invoice invoice = new Invoice();

                        invoice.SectionID = sectionID;
                        invoice.ID = InvoiceBus.GetNextID();
                        invoice.TableID = selectedTable.ID;
                        invoice.DateCreated = DateTime.Now;

                        InvoiceBus.Insert(invoice);

                        // Lấy hóa đơn gần nhất
                        Invoice lastestInvoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);
                        lblThoiGianMo.Text = lastestInvoice.DateCreated.ToString();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Không mở được bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else // Nếu bàn đang mở (có thể đóng)
            {
                try
                {
                    Invoice selectedTableInvoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);
                    DialogResult dongBan;


                    if (selectedTableInvoice.Paid)
                        dongBan = MessageBox.Show("Đóng " + selectedTable.Name + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    else
                        dongBan = MessageBox.Show(selectedTable.Name + " chưa thanh toán. Đóng bàn này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                
                    // Nếu đồng ý đóng thì cập nhật lại trạng thái bàn
                    if (dongBan == System.Windows.Forms.DialogResult.Yes)
                    {
                        selectedTable.Status = true;
                        btnSelectedTable.ForeColor = Color.Blue;
                        btnDongMoBan.Text = "Mở bàn";
                        TableBus.Update(selectedTable);
                        dgvDSMonCuaBan.DataSource = OrderBus.GetOrder(-1);
                        lblTongTien.Text = "0";
                        lblTrangThaiThanhToan.Text = "Chưa thanh toán";

                        txtTienKhachDua.Text = "0";
                        lblTienTraLai.Text = "0";
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Không đóng được bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Thay đổi số lượng món trong hóa đơn
        private void AdjustItemQuantityInInvoice(int Quantity)
        {
            try
            {
                // Lấy hóa đơn mới nhất của bàn đang chọn
                Invoice lastestInvoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);

                if (lastestInvoice.Paid)
                {
                    MessageBox.Show("Bàn đã thanh toán. Không thể thực hiện thêm, sửa món.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tạo một order mới 
                // Nếu tăng số lượng món thì Quantity = 1, ngược lại = -1;
                Order order = new Order();

                order.ID = OrderBus.GetNextID();
                order.InvoiceID = lastestInvoice.ID;
                order.ItemID = int.Parse(dgvDSMonCuaBan.SelectedRows[0].Cells["IDMonTrongHD"].Value.ToString());
                order.Quantity = Quantity;

                // Lấy vị trí dòng của món đang chọn trong dataGridView
                int selectedRowIndex = (int)dgvDSMonCuaBan.SelectedRows[0].Cells[0].Value - 1;

                // Lấy số lượng của món đó tại thời điểm hiện tại
                int recentQuantity = int.Parse(dgvDSMonCuaBan.SelectedRows[0].Cells["SLM"].Value.ToString());

                // Trong trường hợp số lượng món hiện tại = 1 và giảm số lượng thì hỏi xác nhận xóa món ra khỏi hóa đơn
                if (recentQuantity + order.Quantity == 0)
                {
                    DialogResult xoaMon = MessageBox.Show("Bạn muốn xóa món này ra khỏi thực đơn?", "Xóa món",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    // Đồng ý -> Xóa món đó khỏi hóa đơn
                    if (xoaMon == System.Windows.Forms.DialogResult.Yes)
                    {
                        OrderBus.Delete(order);
                        UpdateInvoice();
                    }
                }
                else // Trường hợp còn lại thì cập nhật số lượng món (tăng, giảm)
                {
                    OrderBus.Insert(order);
                    UpdateInvoice();
                    dgvDSMonCuaBan.Rows[selectedRowIndex].Selected = true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Thay đổi số lượng món thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Giảm số lượng món đã chọn trong hóa đơn
        private void btnGiamSLMonTrongHD_Click(object sender, EventArgs e)
        {
            AdjustItemQuantityInInvoice(-1);
        }


        // Tăng số lượng món đã chọn trong hóa đơn
        private void btnTangSLMonTrongHD_Click(object sender, EventArgs e)
        {
            AdjustItemQuantityInInvoice(1);
        }


        // Click chọn nhanh số lượng món
        private void btnSL_Click(object sender, EventArgs e)
        {
            SimpleButton btnSL = (SimpleButton)sender;
            txtSLMon.Text = btnSL.Text;
        }


        // Xóa một món đã chọn trong hóa đơn
        private void btnXoaMonTrongHD_Click(object sender, EventArgs e)
        {
            try
            {
                Invoice lastestInvoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);

                // Kiểm tra nếu xóa món trong hóa đơn đã thanh toán
                if (lastestInvoice.Paid)
                {
                    MessageBox.Show("Bàn đã thanh toán, không xóa món.", "Lỗi xóa món", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult xoaMon = MessageBox.Show("Bạn muốn xóa món này ra khỏi thực đơn?", "Xóa món",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Đồng ý -> Xóa món đó khỏi hóa đơn
                if (xoaMon == System.Windows.Forms.DialogResult.Yes)
                {
                    Order order = new Order();

                    order.InvoiceID = lastestInvoice.ID;
                    order.ItemID = int.Parse(dgvDSMonCuaBan.SelectedRows[0].Cells["IDMonTrongHD"].Value.ToString());
                    order.Quantity = 0;

                    OrderBus.Delete(order);
                    UpdateInvoice();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Không xóa được món.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Xử lý thanh toán
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Invoice invoice = InvoiceBus.GetLastestInvoice(selectedTable.ID);

            // Nếu bàn đã thanh toán thì dừng lại
            if (invoice.Paid)
            {
                MessageBox.Show("Bàn đã được thanh toán", "Bàn đã thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SimpleButton btnTT = (SimpleButton)sender;

            if (btnTT.Text == "Thanh toán")
            {
                txtTienKhachDua.Enabled = true;
                txtTienKhachDua.Focus();
                btnTT.Text = "Xác nhận";
            }
            else
            {
                try
                {
                    // Tính tổng tiền và tiền khách đưa
                    int tongTien = 0;
                    for (int i = 0; i < dgvDSMonCuaBan.RowCount; i++)
                    {
                        tongTien += int.Parse(dgvDSMonCuaBan.Rows[i].Cells["ColThanhTien"].Value.ToString());
                    }

                    int khachDua = int.Parse(txtTienKhachDua.Text);

                    // Nếu khách đưa không đủ tiền
                    if (khachDua < tongTien)
                    {
                        MessageBox.Show("Tiền khách đưa không đủ thanh toán!", "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Tính tiền trả lại
                    int traLai = tongTien - khachDua;

                    txtTienKhachDua.Text = String.Format("{0:#,##0.##}", khachDua);
                    lblTienTraLai.Text = String.Format("{0:#,##0.##}", traLai);
                    txtTienKhachDua.Enabled = false;
                    btnThanhToan.Text = "Thanh toán";
                    lblTrangThaiThanhToan.Text = "Đã thanh toán";


                    // Cập nhật trong cơ sở dữ liệu
                    if (invoice == null)
                        return;

                    invoice.Total = tongTien;
                    invoice.Cash = khachDua;
                    invoice.Charge = traLai;
                    invoice.Paid = true;

                    InvoiceBus.Update(invoice);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi thanh toán.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý phím bấm Enter khi thanh toán
        private void txtTienKhachDua_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SimpleButton btnTT = new SimpleButton();
                btnTT.Text = "Xác nhận";
                btnThanhToan_Click(btnTT, e);
            }
        }


        // In hóa đơn
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }


        // Cài đặt phiếu thanh toán
        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Thông tin nhà hàng
            g.DrawString("Nhà hàng TRẦU CAU", new Font("Segoe UI", 18, FontStyle.Bold), Brushes.Black, new Point(30, 50));
            g.DrawString("276 Trần Hưng Đạo, P4, Quận 5, TP Hồ Chí Minh", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, 80));
            g.DrawString("(08) 432 987 - 0974 713 735", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, 100));

            // Tiêu đề hóa đơn
            g.DrawString("HÓA ĐƠN THANH TOÁN", new Font("Segoe UI", 18, FontStyle.Bold), Brushes.Black, new Point(270, 170));

            int h1, h2, h3, h4;
            h1 = 230;
            h2 = h1 + 50;
            h3 = h2 - 10;
            h4 = h2 + 30;

            g.DrawString(lblTenBanDaChon.Text + " - " + lblTenKhuCuaBan.Text, new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, new Point(30, h1));
            g.DrawString("In lúc: " + DateTime.Now.ToString("H:mm:ss - dd/MM/yyyy"), new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, new Point(380, h1));

            g.DrawLine(new Pen(Color.Black), new Point(30, h3), new Point(820, h3));

            // Header các cột
            g.DrawString("Tên món", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, h2));
            g.DrawString("SL", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(380, h2));
            g.DrawString("Đơn giá", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(500, h2));
            g.DrawString("Thành tiền", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(640, h2));

            g.DrawLine(new Pen(Color.Black), new Point(30, h4), new Point(820, h4));

            h4 += 10;
            double tongTien = 0;

            // In từng món trong hóa đơn
            for (int i = 0; i < dgvDSMonCuaBan.RowCount; i++)
            {
                g.DrawString((i + 1).ToString() + ". " + dgvDSMonCuaBan.Rows[i].Cells[2].Value.ToString(), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, h4 + 30 * i));
                g.DrawString(dgvDSMonCuaBan.Rows[i].Cells[3].Value.ToString(), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(380, h4 + 30 * i));

                int donGia = int.Parse(dgvDSMonCuaBan.Rows[i].Cells[5].Value.ToString());
                Rectangle rect = new Rectangle(470, h4 + 30 * i, 90, 22);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Far;

                g.DrawString(string.Format("{0:#,##0.##}", donGia), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, rect, stringFormat);

                int thanhTien = int.Parse(dgvDSMonCuaBan.Rows[i].Cells[6].Value.ToString());
                rect = new Rectangle(620, h4 + 30 * i, 100, 22);
                g.DrawString(string.Format("{0:#,##0.##}", thanhTien), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, rect, stringFormat);

                tongTien += thanhTien;
            }

            g.DrawLine(new Pen(Color.Black), new Point(30, h4 + dgvDSMonCuaBan.RowCount * 30), new Point(820, h4 + dgvDSMonCuaBan.RowCount * 30));
            
            string strTongTien = string.Format("{0:#,##0.##}", tongTien);
            // In thông tin tổng cộng hóa đơn
            g.DrawString(strTongTien + " VNĐ", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(500, h4 + dgvDSMonCuaBan.RowCount * 30 + 10));
            tongTien += tongTien * 0.1;
            strTongTien = string.Format("{0:#,##0.##}", tongTien);

            g.DrawString("VAT 10%", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(500, h4 + dgvDSMonCuaBan.RowCount * 30 + 40));
            g.DrawString("Tổng cộng: " + strTongTien + " VNĐ", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(380, h4 + dgvDSMonCuaBan.RowCount * 30 + 70));
            g.DrawString("Cảm ơn & hẹn gặp lại quý khách!", new Font("Segoe UI", 14, FontStyle.Italic), Brushes.Black, new Point(270, h4 + dgvDSMonCuaBan.RowCount * 30 + 150));

        }


        // Xử lý khi chọn lại tab màn hình chính
        private void xtraTab_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabManHinhChinh)
            {
                SetSectionList();
                SetMenuList();
            }
        }

        #endregion


        #region CÀI ĐẶT QUẢN LÝ KHU VÀ BÀN

        // Load danh sách các bàn khi chọn khu
        private void cmbDSKhu2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSection = 0;

                // Lấy ID khu đang chọn trong combobox
                var selected = cmbDSKhu2.SelectedValue;
                if (selected is Section)
                {
                    Section selectedSection = (DataTransferObject.Section)selected;
                    idSection = selectedSection.ID;
                }
                else
                    idSection = int.Parse(cmbDSKhu2.SelectedValue.ToString());

                // Lấy thông tin từ CSDL và gán vào DataGridView
                ArrayList tableList = TableBus.GetTableListInSection(idSection);
                dgvDSBan.DataSource = tableList;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Tải danh sách bàn thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        // Thêm số thứ tự của khu trong danh sách các khu
        private void dgvDSKhu_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvDSKhu.RowCount; i++)
            {
                dgvDSKhu.Rows[i].Cells[0].Value = i + 1;
            }

            try
            {
                txtTenKhu.Text = dgvDSKhu.SelectedRows[0].Cells["TenKhu"].Value.ToString();
                txtMoTaKhu.Text = dgvDSKhu.SelectedRows[0].Cells["MoTaKhu"].Value.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi binding dữ liệu khu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Binding thông tin khu lên các control
        private void dgvDSKhu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtTenKhu.Text = dgvDSKhu.SelectedRows[0].Cells["TenKhu"].Value.ToString();
                txtMoTaKhu.Text = dgvDSKhu.SelectedRows[0].Cells["MoTaKhu"].Value.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi binding dữ liệu khu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Xử lý tìm khu
        private void btnTimKhu_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList searchResult = SectionBus.GetSectionListByName(txtTimKiemKhu.Text);

                if (searchResult.Count == 0)
                    MessageBox.Show("Không có kết quả nào được tìm thấy", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    dgvDSKhu.DataSource = searchResult;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Xảy ra lỗi khi tìm khu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Thêm khu
        private void btnThemKhu_Click(object sender, EventArgs e)
        {
            // Enabled các control để điền thông tin khu mới
            if (btnThemKhu.Text == "Thêm khu")
            {
                txtTenKhu.Enabled = true;
                txtMoTaKhu.Enabled = true;

                txtTenKhu.Text = "";
                txtMoTaKhu.Text = "";

                // Disabled các button sửa và xóa khu
                btnThemKhu.Text = "Thêm";
                btnSuaKhu.Enabled = btnXoaKhu.Enabled = false;
            }
            else
            {
                try
                {
                    // Kiểm tra tên khu trống
                    if (txtTenKhu.Text == "")
                    {
                        MessageBox.Show("Tên khu không được để rỗng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo khu mới
                    Section section = new Section();
                    section.Name = txtTenKhu.Text;
                    section.Description = txtMoTaKhu.Text;
                    section.ID = SectionBus.GetNextID();

                    // Chèn thông tin khu mới vào cơ sở dữ liệu
                    SectionBus.Insert(section);
                    dgvDSKhu.DataSource = SectionBus.GetSectionList();
                    sectionList = SectionBus.GetSectionList();

                    // Tạo 10 bàn mới cho khu này
                    TableBus.Insert(section.ID, 10, "Bàn", 1);
                    MessageBox.Show("Khu " + section.Name + " được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Nạp lại danh sách bàn mới
                    SetSectionList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Thêm khu thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    txtTenKhu.Enabled = false;
                    txtMoTaKhu.Enabled = false;

                    btnThemKhu.Text = "Thêm khu";
                    btnSuaKhu.Enabled = btnXoaKhu.Enabled = true;
                }
            }
        }


        // Xử lý thay đổi thông tin khu
        private void btnSuaKhu_Click(object sender, EventArgs e)
        {
            if (btnSuaKhu.Text == "Sửa khu")
            {
                txtTenKhu.Enabled = true;
                txtMoTaKhu.Enabled = true;

                btnSuaKhu.Text = "Lưu";
                btnThemKhu.Enabled = btnXoaKhu.Enabled = false;
            }
            else
            {
                try
                {
                    txtTenKhu.Enabled = false;
                    txtMoTaKhu.Enabled = false;
                    btnThemKhu.Enabled = btnXoaKhu.Enabled = true;

                    btnSuaKhu.Text = "Sửa khu";

                    Section section = new Section();

                    section.Name = txtTenKhu.Text;
                    section.Description = txtMoTaKhu.Text;
                    section.ID = int.Parse(dgvDSKhu.SelectedRows[0].Cells["IDKhu"].Value.ToString());

                    SectionBus.Update(section);

                    MessageBox.Show("Khu " + section.Name + " được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SetSectionList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Có lỗi xảy ra khi sửa thông tin khu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý xóa khu
        private void btnXoaKhu_Click(object sender, EventArgs e)
        {
            DialogResult confirmDelete;

            confirmDelete = MessageBox.Show("Xóa khu " + dgvDSKhu.SelectedRows[0].Cells["TenKhu"].Value.ToString() + "?\nTất cả các bàn của khu này sẽ bị xóa.",
                                            "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmDelete == DialogResult.Yes)
            {
                try
                {
                    Section section = new Section();

                    section.Name = txtTenKhu.Text;
                    section.Description = txtMoTaKhu.Text;
                    section.ID = int.Parse(dgvDSKhu.SelectedRows[0].Cells["IDKhu"].Value.ToString());

                    Table table = new Table();
                    table.SectionID = section.ID;

                    
                    // Xóa các bàn trong khu
                    TableBus.DeleteBySectionID(table);

                    // Xóa khu
                    SectionBus.Delete(section);

                    dgvDSKhu.DataSource = SectionBus.GetSectionList();

                    SetSectionList();

                    MessageBox.Show("Xóa khu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi xóa khu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Thêm số thứ tự và thông tin trạng thái của bàn
        private void dgvDSBan_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvDSBan.RowCount; i++)
                {
                    dgvDSBan.Rows[i].Cells[0].Value = i + 1;

                    if (dgvDSBan.Rows[i].Cells["Status"].Value.ToString() == "True")
                        dgvDSBan.Rows[i].Cells["TrangThai"].Value = "Bàn trống";
                    else
                        dgvDSBan.Rows[i].Cells["TrangThai"].Value = "Có khách";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi tải thông tin bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                txtTenBanSX.Text = dgvDSBan.SelectedRows[0].Cells["TenBan"].Value.ToString();
                txtMoTaBanSX.Text = dgvDSBan.SelectedRows[0].Cells["MoTaBan"].Value.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi binding thông tin bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Binding thông tin bàn lên các control
        private void dgvDSBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtTenBanSX.Text = dgvDSBan.SelectedRows[0].Cells["TenBan"].Value.ToString();
                txtMoTaBanSX.Text = dgvDSBan.SelectedRows[0].Cells["MoTaBan"].Value.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi binding thông tin bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Tìm bàn
        private void btnTimBan_Click(object sender, EventArgs e)
        {
            try
            {
                int idSection = int.Parse(cmbDSKhu2.SelectedValue.ToString());

                string nameToLookUp = txtTenBan.Text;
                ArrayList searchResult = TableBus.GetTableByName(nameToLookUp, idSection);

                // Xuất kết quả tìm kiếm
                if (searchResult.Count == 0)
                    MessageBox.Show("Không có kết quả nào được tìm thấy", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    dgvDSBan.DataSource = searchResult;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi tìm bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Xử lý thêm bài mới
        private void btnThemBan_Click(object sender, EventArgs e)
        {
            // Xác định ID khu sẽ thêm bàn
            int sectionID = 0;

            var selected = cmbDSKhu3.SelectedValue;
            if (selected is Section)
            {
                Section selectedSection = (DataTransferObject.Section)selected;
                sectionID = selectedSection.ID;
            }
            else
                sectionID = int.Parse(cmbDSKhu3.SelectedValue.ToString());

            try
            {

                // Thông tin số lượng, tên bàn mới thêm vào
                int quantity = int.Parse(numSLBanThem.Text);
                int from = int.Parse(txtFrom.Text);
                string tableName = txtTenBanThem.Text;
            
                // Thêm bàn
                TableBus.Insert(sectionID, quantity, tableName, from);

                MessageBox.Show("Thêm bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvDSBan.DataSource = TableBus.GetTableListInSection(sectionID);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Thêm bàn thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Cập nhật thông tin một bàn
        private void btnSuaBan_Click(object sender, EventArgs e)
        {
            if (btnSuaBan.Text == "Sửa")
            {
                // Enabled các control để nhập liệu
                txtTenBanSX.Enabled = true;
                txtMoTaBanSX.Enabled = true;
                btnSuaBan.Text = "Cập nhật";

                // Disabled các button thêm, xóa
                btnXoaBan.Enabled = btnThemBan.Enabled = false;
            }
            else
            {
                try
                {
                    // Tạo chứa thông tin bàn cần cập nhật
                    Table table = new Table();

                    table.ID = int.Parse(dgvDSBan.SelectedRows[0].Cells["IDBan"].Value.ToString());
                    table.SectionID = int.Parse(dgvDSBan.SelectedRows[0].Cells["SectionID"].Value.ToString());
                    table.Status = (bool)dgvDSBan.SelectedRows[0].Cells["Status"].Value;
                    table.Name = txtTenBanSX.Text;
                    table.Description = txtMoTaBanSX.Text;

                    // Cập nhật xuống cơ sở dữ liệu
                    TableBus.Update(table);

                    MessageBox.Show("Thông tin " + table.Name + " được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnSuaBan.Text = "Sửa";
                    txtTenBanSX.Enabled = false;
                    txtMoTaBanSX.Enabled = false;

                    // Cập nhật thông tin mới
                    SetSectionList();
                    dgvDSBan.DataSource = TableBus.GetTableListInSection(table.SectionID);
                    btnXoaBan.Enabled = btnThemBan.Enabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Cập nhật thông tin bàn thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý xóa bàn
        private void btnXoaBan_Click(object sender, EventArgs e)
        {
            string tenBan = dgvDSBan.SelectedRows[0].Cells["TenBan"].Value.ToString();

            string confirmMessage = "Xóa " + dgvDSBan.SelectedRows.Count.ToString() + " bàn đã chọn?\nTất cả hóa đơn liên quan sẽ bị xóa!!!";

            if (dgvDSBan.SelectedRows.Count == 1)
                confirmMessage = "Xóa " + tenBan + "?\nTất cả hóa đơn của bàn này sẽ bị xóa!!!";

            if (MessageBox.Show(confirmMessage, "Xóa bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    for (int i = 0; i < dgvDSBan.SelectedRows.Count; i++)
                    {
                        // Xóa các hóa đơn của bàn đó
                        Invoice invoice = new Invoice();
                        invoice.TableID = int.Parse(dgvDSBan.SelectedRows[i].Cells["IDBan"].Value.ToString());
                        InvoiceBus.DeleteByTable(invoice);

                        // Xóa bàn
                        Table table = new Table();
                        table.ID = int.Parse(dgvDSBan.SelectedRows[i].Cells["IDBan"].Value.ToString());
                        table.SectionID = int.Parse(dgvDSKhu.SelectedRows[0].Cells["IDKhu"].Value.ToString());
                        // Thông báo kết quả
                        TableBus.Delete(table);
                    }

                    MessageBox.Show("Xóa bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    int sectionID = int.Parse(cmbDSKhu2.SelectedValue.ToString());
                    dgvDSBan.DataSource = TableBus.GetTableListInSection(sectionID);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Xóa bàn thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion


        #region QUẢN LÝ THỰC ĐƠN VÀ MÓN

        // Thêm số thứ tự các thực đơn trong DataGridView
        private void dgvDSThucDon_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvDSThucDon.RowCount; i++)
                dgvDSThucDon.Rows[i].Cells[0].Value = i + 1;
            
            txtTenTD.Text = dgvDSThucDon.SelectedRows[0].Cells["TenTD"].Value.ToString();
            txtMoTaTD.Text = dgvDSThucDon.SelectedRows[0].Cells["MoTaTD"].Value.ToString();
        }


        // Xử lý thêm hóa đơn
        private void btnThemTD_Click(object sender, EventArgs e)
        {
            if (btnThemTD.Text == "Thêm mới")
            {
                btnThemTD.Text = "Thêm";
                txtTenTD.Enabled = true;
                txtMoTaTD.Enabled = true;
                txtTenTD.Text = "";
                txtMoTaTD.Text = "";
                
                btnSuaTD.Enabled = btnXoaTD.Enabled = false;
            }
            else
            {
                try
                {
                    // Kiểm tra dữ liệu vào trống?
                    if (txtTenTD.Text == "")
                    {
                        MessageBox.Show("Tên thực đơn không được để rỗng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo thực đơn mới
                    DataTransferObject.Menu menu = new DataTransferObject.Menu();
                    menu.ID = MenuBus.GetNextID();
                    menu.Name = txtTenTD.Text;
                    menu.Description = txtMoTaTD.Text;

                    // Thêm vào CSDL và thông báo kết quả
                    MenuBus.Insert(menu);

                    MessageBox.Show("Thực đơn " + menu.Name + " thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật lại danh sách thực đơn mới
                    SetMenuList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi thêm thực đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnThemTD.Text = "Thêm mới";
                    txtTenTD.Enabled = false;
                    txtMoTaTD.Enabled = false;
                    btnXoaTD.Enabled = btnSuaTD.Enabled = true;
                }
            }
        }


        // Xử lý cập nhật thông tin thực đơn
        private void btnSuaTD_Click(object sender, EventArgs e)
        {
            if (btnSuaTD.Text == "Sửa")
            {
                btnSuaTD.Text = "Lưu";
                txtTenTD.Enabled = true;
                txtMoTaTD.Enabled = true;
                btnThemTD.Enabled = btnXoaTD.Enabled = false;
            }
            else
            {
                try
                {
                    // Kiểm tra tên thực đơn rỗng
                    if (txtTenTD.Text == "")
                    {
                        MessageBox.Show("Tên thực đơn không được để rỗng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo đối tượng chứa các thông tin thực đơn cần cập nhật
                    DataTransferObject.Menu menu = new DataTransferObject.Menu();
                    menu.Name = txtTenTD.Text;
                    menu.Description = txtMoTaTD.Text;
                    menu.ID = int.Parse(dgvDSThucDon.SelectedRows[0].Cells[1].Value.ToString());

                    // Cập nhật trong CSDL
                    MenuBus.Update(menu);

                    MessageBox.Show("Thực đơn " + menu.Name + " cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnSuaTD.Text = "Sửa";
                    txtTenTD.Enabled = false;
                    txtMoTaTD.Enabled = false;
                    SetMenuList();
                    btnThemTD.Enabled = btnXoaTD.Enabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi cập nhật thông tin thực đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý xóa thực đơn
        private void btnXoaTD_Click(object sender, EventArgs e)
        {
            string tenThucDon = dgvDSThucDon.SelectedRows[0].Cells["TenTD"].Value.ToString();
            if (MessageBox.Show("Xóa thực đơn " + tenThucDon + "?\nTất cả các món của thực đơn này sẽ bị xóa!!!", "Xóa thực đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Xóa các món ăn thuộc thực đơn đó
                    ItemMenu itemMenu = new ItemMenu();
                    itemMenu.MenuID = int.Parse(dgvDSThucDon.SelectedRows[0].Cells["IDTD"].Value.ToString());
                    ItemMenuBus.DeleteByMenuID(itemMenu);

                    // Xóa thực đơn
                    DataTransferObject.Menu menu = new DataTransferObject.Menu();
                    menu.ID = int.Parse(dgvDSThucDon.SelectedRows[0].Cells["IDTD"].Value.ToString());

                    // Thông báo kết quả
                    MenuBus.Delete(menu);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetMenuList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi khi xóa thực đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý tìm kiếm thực đơn
        private void btnTimKiemTD_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList searchResult = MenuBus.GetMenuByName(txtTenTDTimKiem.Text);

                if (searchResult.Count == 0)
                    MessageBox.Show("Không có kết quả nào được tìm thấy", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else dgvDSThucDon.DataSource = searchResult;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi khi tìm kiếm thực đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Tải danh sách các món ăn lên DataGridView khi chọn thực đơn
        private void cmbDSThucDon2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int menuID = 0;

            var selected = cmbDSThucDon2.SelectedValue;
            if (selected is DataTransferObject.Menu)
            {
                DataTransferObject.Menu selectedMenu = (DataTransferObject.Menu)selected;
                menuID = selectedMenu.ID;
            }
            else
                menuID = int.Parse(cmbDSThucDon2.SelectedValue.ToString());

            DataTable itemList = ItemBus.GetItemListInMenu(menuID);
            dgvDSMon.DataSource = itemList;

            cmbDSThucDon3.SelectedValue = cmbDSThucDon2.SelectedValue;
        }


        // Thêm số thứ tự trong DataGridView danh sách món của thực đơn
        private void dgvDSMon_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvDSMon.RowCount; i++)
                dgvDSMon.Rows[i].Cells[0].Value = i + 1;

            txtTenMon.Text = dgvDSMon.SelectedRows[0].Cells["NameItem"].Value.ToString();
            txtGiaMon.Text = dgvDSMon.SelectedRows[0].Cells["PriceItem"].Value.ToString();
            txtDonViTinhMon.Text = dgvDSMon.SelectedRows[0].Cells["UnitItem"].Value.ToString();
            txtMoTaMon.Text = dgvDSMon.SelectedRows[0].Cells["DescriptionItem"].Value.ToString();
        }


        // Binding thông tin thực đơn lên các control
        private void dgvDSThucDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTenTD.Text = dgvDSThucDon.SelectedRows[0].Cells["TenTD"].Value.ToString();
            txtMoTaTD.Text = dgvDSThucDon.SelectedRows[0].Cells["MoTaTD"].Value.ToString();
        }


        // Binding thông tin món lên các control
        private void dgvDSMon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTenMon.Text = dgvDSMon.SelectedRows[0].Cells["NameItem"].Value.ToString();
            txtGiaMon.Text = dgvDSMon.SelectedRows[0].Cells["PriceItem"].Value.ToString();
            txtDonViTinhMon.Text = dgvDSMon.SelectedRows[0].Cells["UnitItem"].Value.ToString();
            txtMoTaMon.Text = dgvDSMon.SelectedRows[0].Cells["DescriptionItem"].Value.ToString();
        }


        // Xử lý tìm kiếm món
        private void btnTimKiemMon_Click(object sender, EventArgs e)
        {
            int menuID = 0;

            var selected = cmbDSKhu2.SelectedValue;
            if (selected is Section)
            {
                Section selectedSection = (DataTransferObject.Section)selected;
                menuID = selectedSection.ID;
            }
            else
                menuID = int.Parse(cmbDSKhu2.SelectedValue.ToString());

            ArrayList searchResult = ItemBus.GetItemByName(txtTenMonTK.Text, menuID);

            if (searchResult.Count == 0)
                MessageBox.Show("Không có kết quả nào được tìm thấy", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                dgvDSMon.DataSource = searchResult;
        }


        // Xử lý thêm món mới
        private void btnThemMon_Click(object sender, EventArgs e)
        {
            if (btnThemMon.Text == "Thêm món")
            {
                btnThemMon.Text = "Thêm";
                txtTenMon.Enabled = true;
                txtGiaMon.Enabled = true;
                txtMoTaMon.Enabled = true;
                txtDonViTinhMon.Enabled = true;
                cmbDSThucDon3.Enabled = true;
                btnSuaMon.Enabled = btnXoaMon.Enabled = false;

                txtTenMon.Text = "";
                txtGiaMon.Text = "";
                txtMoTaMon.Text = "";
                txtDonViTinhMon.Text = "";
            }
            else
            {
                if (txtTenMon.Text == "" || txtGiaMon.Text == "" || txtDonViTinhMon.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đủ các thông tin tên, giá, đơn vị tính của món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnThemMon.Text = "Thêm món";
                txtTenMon.Enabled = false;
                txtGiaMon.Enabled = false;
                txtMoTaMon.Enabled = false;
                txtDonViTinhMon.Enabled = false;
                cmbDSThucDon3.Enabled = false;

                try
                {
                    Item item = new Item();

                    item.ID = ItemBus.GetNextID();
                    item.Name = txtTenMon.Text;
                    item.Price = int.Parse(txtGiaMon.Text);
                    item.Unit = txtDonViTinhMon.Text;
                    item.Description = txtMoTaMon.Text;

                    ItemBus.Insert(item);

                    ItemMenu itemMenu = new ItemMenu();
                    itemMenu.ItemID = item.ID;
                    itemMenu.MenuID = int.Parse(cmbDSThucDon3.SelectedValue.ToString());

                    ItemMenuBus.Insert(itemMenu);

                    MessageBox.Show("Món " + item.Name + " được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    int sectionID = 0;

                    var selected = cmbDSKhu2.SelectedValue;
                    if (selected is Section)
                    {
                        Section selectedSection = (DataTransferObject.Section)selected;
                        sectionID = selectedSection.ID;
                    }
                    else
                        sectionID = int.Parse(cmbDSKhu2.SelectedValue.ToString());

                    DataTable itemList = ItemBus.GetItemListInMenu(int.Parse(cmbDSThucDon2.SelectedValue.ToString()));
                    dgvDSMon.DataSource = itemList;
                    btnSuaMon.Enabled = btnXoaMon.Enabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi thêm món.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý sửa thông tin món
        private void btnSuaMon_Click(object sender, EventArgs e)
        {
            if (btnSuaMon.Text == "Sửa món")
            {
                btnSuaMon.Text = "Lưu";
                txtTenMon.Enabled = true;
                txtGiaMon.Enabled = true;
                txtMoTaMon.Enabled = true;
                txtDonViTinhMon.Enabled = true;
                cmbDSThucDon3.Enabled = true;
                OldMenuID = int.Parse(cmbDSThucDon3.SelectedValue.ToString());

                btnThemMon.Enabled = btnXoaMon.Enabled = false;
            }
            else
            {
                if (txtTenMon.Text == "" || txtGiaMon.Text == "" || txtDonViTinhMon.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đủ các thông tin tên, giá, đơn vị tính của món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnSuaMon.Text = "Sửa món";
                txtTenMon.Enabled = false;
                txtGiaMon.Enabled = false;
                txtMoTaMon.Enabled = false;
                txtDonViTinhMon.Enabled = false;
                cmbDSThucDon3.Enabled = false;

                try
                {
                    Item item = new Item();
                    item.ID = int.Parse(dgvDSMon.SelectedRows[0].Cells["IDItem"].Value.ToString());
                    item.Name = txtTenMon.Text;
                    item.Price = int.Parse(txtGiaMon.Text);
                    item.Unit = txtDonViTinhMon.Text;
                    item.Description = txtMoTaMon.Text;

                    ItemBus.Update(item);

                    ItemMenu itemMenu = new ItemMenu();
                    itemMenu.ItemID = item.ID;
                    itemMenu.MenuID = int.Parse(cmbDSThucDon3.SelectedValue.ToString());
                    itemMenu.OldMenuID = OldMenuID;

                    ItemMenuBus.Update(itemMenu);

                    MessageBox.Show("Món " + item.Name + " được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DataTable itemList = ItemBus.GetItemListInMenu(int.Parse(cmbDSThucDon2.SelectedValue.ToString()));
                    dgvDSMon.DataSource = itemList;
                    btnThemMon.Enabled = btnXoaMon.Enabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi cập nhật thông tin món.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý xóa món
        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            DialogResult confirmDelete;
            string confirmMessage = "Xóa các món đã chọn?";

            if (dgvDSMon.SelectedRows.Count == 1)
                confirmMessage = "Xóa " + dgvDSMon.SelectedRows[0].Cells["NameItem"].Value.ToString() + " ra khỏi thực đơn?";

            confirmDelete = MessageBox.Show(confirmMessage, "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmDelete == DialogResult.Yes)
            {
                try
                {
                    for (int i = 0; i < dgvDSMon.SelectedRows.Count; i++)
                    {
                        ItemMenu itemMenu = new ItemMenu();
                        itemMenu.ItemID = int.Parse(dgvDSMon.SelectedRows[i].Cells["IDItem"].Value.ToString());
                        ItemMenuBus.DeleteByItemID(itemMenu);

                        Item item = new Item();
                        item.ID = int.Parse(dgvDSMon.SelectedRows[i].Cells["IDItem"].Value.ToString());
                        ItemBus.Delete(item);
                    }
                    
                    MessageBox.Show("Xóa món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DataTable itemList = ItemBus.GetItemListInMenu(int.Parse(cmbDSThucDon2.SelectedValue.ToString()));
                    dgvDSMon.DataSource = itemList;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi khi xóa món.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion


        #region QUẢN LÝ HÓA ĐƠN

        // Tra cứu hóa đơn
        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            try
            {
                Invoice invoice = GetSearchInvoice();

                DataTable searchResult = InvoiceBus.GetInvoice(invoice);


                if (searchResult.Rows.Count < 1)
                    MessageBox.Show("Không có kết quả nào được tìm thấy", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    dgvHoaDon.DataSource = searchResult;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi lấy danh sách bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Invoice GetSearchInvoice()
        {
            int sectionID = 0;
            int tableID = 0;

            var selected1 = cmbDSKhuHD.SelectedValue;
            if (selected1 is Section)
            {
                Section selectedSection = (DataTransferObject.Section)selected1;
                sectionID = selectedSection.ID;
            }
            else
                sectionID = int.Parse(cmbDSKhuHD.SelectedValue.ToString());

            var selected2 = cmbDSBanHD.SelectedValue;
            if (selected2 is Table)
            {
                Table selectedTable = (DataTransferObject.Table)selected2;
                tableID = selectedTable.ID;
            }
            else
                tableID = int.Parse(cmbDSBanHD.SelectedValue.ToString());

            Invoice invoice = new Invoice();
            invoice.ID = 0;
            invoice.TableID = tableID;
            invoice.SectionID = sectionID;
            invoice.DateCreated = dtHoaDon.Value.Date;
            invoice.Total = int.Parse(txtMinTotal.Text);

            if (chkAllDay.Checked == true)
                invoice.DateCreated = Convert.ToDateTime("01/01/0001 00:00:00");

            if (chkAllSection.Checked == true)
                invoice.SectionID = 0;

            if (chkAllTable.Checked == true)
                invoice.TableID = 0;
            return invoice;
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int invoiceID = int.Parse(dgvHoaDon.SelectedRows[0].Cells["IDHD"].Value.ToString());
            dgvDSMonHD.DataSource = OrderBus.GetOrder(invoiceID);
        }

        private void dgvDSMonHD_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvDSMonHD.RowCount; i++)
                dgvDSMonHD.Rows[i].Cells[0].Value = i + 1;

        }

        private void dgvHoaDon_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvHoaDon.RowCount; i++)
            {
                dgvHoaDon.Rows[i].Cells[0].Value = i + 1;

                if (dgvHoaDon.Rows[i].Cells["TrangThaiHD"].Value.ToString() == "True")
                    dgvHoaDon.Rows[i].Cells["InvoiceStatus"].Value = "Đã thanh toán";
                else
                    dgvHoaDon.Rows[i].Cells["InvoiceStatus"].Value = "Chưa thanh toán";
            }
        }

        // Xem hóa đơn
        private void btnXemHD_Click(object sender, EventArgs e)
        {
            try
            {
                printPreviewDialog.Document = printHoaDon;
                printPreviewDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi khi in hóa đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printHoaDon_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Thông tin nhà hàng
            g.DrawString("Nhà hàng TRẦU CAU", new Font("Segoe UI", 18, FontStyle.Bold), Brushes.Black, new Point(30, 50));
            g.DrawString("276 Trần Hưng Đạo, P4, Quận 5, TP Hồ Chí Minh", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, 80));
            g.DrawString("(08) 432 987 - 0974 713 735", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, 100));

            // Tiêu đề hóa đơn
            g.DrawString("HÓA ĐƠN THANH TOÁN", new Font("Segoe UI", 18, FontStyle.Bold), Brushes.Black, new Point(270, 170));

            int h1, h2, h3, h4;
            h1 = 230;
            h2 = h1 + 50;
            h3 = h2 - 10;
            h4 = h2 + 30;

            string tenBan = dgvHoaDon.SelectedRows[0].Cells["TenBanHD"].Value.ToString();
            string tenKhu = dgvHoaDon.SelectedRows[0].Cells["TenKhuHD"].Value.ToString();
            string thoiGian = dgvHoaDon.SelectedRows[0].Cells["ThoiGianTao"].Value.ToString();

            g.DrawString(tenBan + " - " + tenKhu, new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, new Point(30, h1));
            g.DrawString("Mở lúc: " + thoiGian, new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, new Point(380, h1));

            g.DrawLine(new Pen(Color.Black), new Point(30, h3), new Point(820, h3));

            // Header các cột
            g.DrawString("Tên món", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, h2));
            g.DrawString("SL", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(380, h2));
            g.DrawString("Đơn giá", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(500, h2));
            g.DrawString("Thành tiền", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(640, h2));

            g.DrawLine(new Pen(Color.Black), new Point(30, h4), new Point(820, h4));

            h4 += 10;

            double tongTien = 0;
            // In từng món trong hóa đơn
            for (int i = 0; i < dgvDSMonHD.RowCount; i++)
            {
                g.DrawString((i + 1).ToString() + ". " + dgvDSMonHD.Rows[i].Cells[2].Value.ToString(), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(30, h4 + 30 * i));
                g.DrawString(dgvDSMonHD.Rows[i].Cells[3].Value.ToString(), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, new Point(380, h4 + 30 * i));

                int donGia = int.Parse(dgvDSMonHD.Rows[i].Cells[5].Value.ToString());
                Rectangle rect = new Rectangle(470, h4 + 30 * i, 90, 22);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Far;

                g.DrawString(string.Format("{0:#,##0.##}", donGia), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, rect, stringFormat);

                int thanhTien = int.Parse(dgvDSMonHD.Rows[i].Cells[6].Value.ToString());
                rect = new Rectangle(620, h4 + 30 * i, 100, 22);
                g.DrawString(string.Format("{0:#,##0.##}", thanhTien), new Font("Segoe UI", 12, FontStyle.Regular), Brushes.Black, rect, stringFormat);
                tongTien += thanhTien;
            }

            g.DrawLine(new Pen(Color.Black), new Point(30, h4 + dgvDSMonHD.RowCount * 30), new Point(820, h4 + dgvDSMonHD.RowCount * 30));

            string strTongTien = string.Format("{0:#,##0.##}", tongTien);

            g.DrawString(strTongTien + " VNĐ", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(500, h4 + dgvDSMonHD.RowCount * 30 + 10));
            tongTien += tongTien * 0.1;

            strTongTien = string.Format("{0:#,##0.##}", tongTien);

            g.DrawString("VAT 10%", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(500, h4 + dgvDSMonHD.RowCount * 30 + 40));
            g.DrawString("Tổng cộng: " + strTongTien + " VNĐ", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(380, h4 + dgvDSMonHD.RowCount * 30 + 70));
            g.DrawString("Cảm ơn & hẹn gặp lại quý khách!", new Font("Segoe UI", 14, FontStyle.Italic), Brushes.Black, new Point(270, h4 + dgvDSMonHD.RowCount * 30 + 150));

            // In thông tin tổng cộng hóa đơn
            //g.DrawString("Tổng cộng: " + strTongTien + " VNĐ", new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new Point(380, h4 + dgvDSMonHD.RowCount * 30 + 10));
            //g.DrawString("Cảm ơn & hẹn gặp lại quý khách!", new Font("Segoe UI", 14, FontStyle.Italic), Brushes.Black, new Point(270, h4 + dgvDSMonHD.RowCount * 30 + 60));
        }

        // In hóa đơn
        private void btnInHD_Click(object sender, EventArgs e)
        {
            try
            {
                printPreviewDialog.Document = printHoaDon;
                printPreviewDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi khi in hóa đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa hóa đơn
        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            string soHoaDon = " ";
            if (dgvHoaDon.SelectedRows.Count > 1)
                soHoaDon = " các ";

            if (MessageBox.Show("Xóa" + soHoaDon + "hoá đơn này?", "Xóa hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Invoice invoice = new Invoice();
                    for (int i = 0; i < dgvHoaDon.SelectedRows.Count; i++)
                    {
                        invoice.ID = int.Parse(dgvHoaDon.SelectedRows[i].Cells["IDHD"].Value.ToString());
                        InvoiceBus.Delete(invoice);
                    }

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DataTable searchResult = InvoiceBus.GetInvoice(GetSearchInvoice());

                    dgvHoaDon.DataSource = searchResult;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion


        #region QUẢN LÝ DỮ LIỆU VÀ TÀI KHOẢN

        // Số thứ tự danh sách tài khoản
        private void dgvAccount_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dgvAccount.RowCount; i++)
                dgvAccount.Rows[i].Cells[0].Value = i + 1;

            txtAddress.Text = dgvAccount.SelectedRows[0].Cells["DiaChi"].Value.ToString();
            txtUsername.Text = dgvAccount.SelectedRows[0].Cells["TenDangNhap"].Value.ToString();
            txtPhone.Text = dgvAccount.SelectedRows[0].Cells["DienThoai"].Value.ToString();
            txtFullname.Text = dgvAccount.SelectedRows[0].Cells["HoTen"].Value.ToString();

            if ((bool)dgvAccount.SelectedRows[0].Cells["LaQuanLy"].Value)
                chkAdmin.Checked = true;
            else
                chkAdmin.Checked = false;

            dtBirthday.Value = Convert.ToDateTime(dgvAccount.SelectedRows[0].Cells["NgaySinh"].Value.ToString());
        }


        // Thiết lập thông tin tài khoản đăng nhập
        private void setAccountInfo()
        {
            try
            {
                txtUsernameEdit.Text = loggedAccount.Username;
                txtFullnameEdit.Text = loggedAccount.Fullname;
                txtPhoneEdit.Text = loggedAccount.Phone;
                txtAddressEdit.Text = loggedAccount.Address;
                dtBirthdayEdit.Value = loggedAccount.Birthday;

                // Với tài khoản thu ngân ẩn trang thông tin quản lý
                if (!loggedAccount.IsAdmin)
                {
                    grpCtrAccount.Visible = grpCtrlAccountList.Visible = false;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi thông tin tài khoản người dùng.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Binding thông tin tài khoản lên các control
        private void dgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAddress.Text = dgvAccount.SelectedRows[0].Cells["DiaChi"].Value.ToString();
            txtUsername.Text = dgvAccount.SelectedRows[0].Cells["TenDangNhap"].Value.ToString();
            txtPhone.Text = dgvAccount.SelectedRows[0].Cells["DienThoai"].Value.ToString();
            txtFullname.Text = dgvAccount.SelectedRows[0].Cells["HoTen"].Value.ToString();

            if ((bool)dgvAccount.SelectedRows[0].Cells["LaQuanLy"].Value)
                chkAdmin.Checked = true;
            else
                chkAdmin.Checked = false;

            dtBirthday.Value = Convert.ToDateTime(dgvAccount.SelectedRows[0].Cells["NgaySinh"].Value.ToString());
        }


        // Xử lý thêm tài khoản mới
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (btnAddAccount.Text == "Thêm mới")
            {
                btnAddAccount.Text = "Thêm";
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
                txtFullname.Enabled = true;
                txtAddress.Enabled = true;
                txtPhone.Enabled = true;
                chkAdmin.Enabled = true;
                dtBirthday.Enabled = true;

                txtUsername.Text = "";
                txtPassword.Text = "";
                txtFullname.Text = "";
                txtAddress.Text = "";
                txtPhone.Text = "";
                chkAdmin.Checked = false;
                dtBirthday.Value = Convert.ToDateTime("1/1/1990");

                btnEditAccount.Enabled = btnDeleteAccount.Enabled = false;
            }
            else
            {
                if (txtUsername.Text == "" || txtPassword.Text == "" || txtFullname.Text == "")
                {
                    MessageBox.Show("Tên đăng nhập, mật khẩu và họ tên không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnAddAccount.Text = "Thêm mới";
                txtUsername.Enabled = false;
                txtFullname.Enabled = false;
                txtPassword.Enabled = false;
                txtAddress.Enabled = false;
                txtPhone.Enabled = false;
                chkAdmin.Enabled = false;
                dtBirthday.Enabled = false;

                try
                {
                    Account account = new Account();

                    account.ID = AccountBus.GetNextID();
                    account.Username = txtUsername.Text;
                    account.Password = AccountBus.GetMD5Hash(txtPassword.Text);
                    account.Fullname = txtFullname.Text;
                    account.Address = txtAddress.Text;
                    account.Phone = txtPhone.Text;
                    account.IsAdmin = chkAdmin.Checked;
                    account.Birthday = dtBirthday.Value;

                    AccountBus.Insert(account);
                    MessageBox.Show("Tài khoản " + account.Username + " được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dgvAccount.DataSource = AccountBus.GetAccountList();

                    btnEditAccount.Enabled = btnDeleteAccount.Enabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Thêm tài khoản thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Cập nhật thông tin tài khoản cũ
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            if (btnEditAccount.Text == "Chỉnh sửa")
            {
                btnEditAccount.Text = "Lưu";
                txtFullname.Enabled = true;
                txtAddress.Enabled = true;
                txtPhone.Enabled = true;
                chkAdmin.Enabled = true;
                dtBirthday.Enabled = true;
                chkResetPassword.Enabled = true;
                chkResetPassword.Checked = false;

                btnAddAccount.Enabled = btnDeleteAccount.Enabled = false;
            }
            else
            {
                if (txtUsername.Text == "" || txtFullname.Text == "")
                {
                    MessageBox.Show("Tên đăng nhập và họ tên không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnEditAccount.Text = "Chỉnh sửa";

                txtFullname.Enabled = false;
                txtAddress.Enabled = false;
                txtPhone.Enabled = false;
                chkAdmin.Enabled = false;
                dtBirthday.Enabled = false;
                chkResetPassword.Enabled = false;

                try
                {
                    Account account = new Account();

                    account.ID = int.Parse(dgvAccount.SelectedRows[0].Cells[1].Value.ToString());
                    account.Username = txtUsername.Text;

                    if (chkResetPassword.Checked)
                        account.Password = "#reset";
                    else
                        account.Password = "";

                    account.Fullname = txtFullname.Text;
                    account.Address = txtAddress.Text;
                    account.Phone = txtPhone.Text;
                    account.IsAdmin = chkAdmin.Checked;
                    account.Birthday = dtBirthday.Value;

                    AccountBus.Update(account);
                    MessageBox.Show("Tài khoản " + account.Username + " được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dgvAccount.DataSource = AccountBus.GetAccountList();

                    chkResetPassword.Checked = false;
                    btnAddAccount.Enabled = btnDeleteAccount.Enabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin tài khoản.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xóa một tài khoản
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string confirmMessage = "Xóa tài khoản " + dgvAccount.SelectedRows[0].Cells[2].Value.ToString() + "?";
            DialogResult confirmDelete = MessageBox.Show(confirmMessage, "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmDelete == DialogResult.Yes)
            {
                try
                {
                    Account account = new Account();
                    account.ID = int.Parse(dgvAccount.SelectedRows[0].Cells[1].Value.ToString());
                    account.Username = dgvAccount.SelectedRows[0].Cells[2].Value.ToString();
                    AccountBus.Delete(account);
                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvAccount.DataSource = AccountBus.GetAccountList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lỗi khi xóa tài khoản.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Đổi mật khẩu tài khoản đang đăng nhập
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (btnChangePassword.Text == "Đổi mật khẩu")
            {
                txtOldPassword.Enabled = true;
                txtNewPassword.Enabled = true;
                txtConfirmNewPassword.Enabled = true;
                btnChangePassword.Text = "Xác nhận";
            }
            else
            {
                // Kiểm tra mật khẩu cũ
                if (AccountBus.GetMD5Hash(txtOldPassword.Text) != loggedAccount.Password)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra hai mật khẩu giống nhau
                if (txtNewPassword.Text != txtConfirmNewPassword.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp. Vui lòng kiểm tra lại.", "Lỗi xác nhận mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    loggedAccount.Password = AccountBus.GetMD5Hash(txtNewPassword.Text);
                    AccountBus.Update(loggedAccount);
                    MessageBox.Show("Mật khẩu được đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnChangePassword.Text = "Đổi mật khẩu";
                    txtOldPassword.Enabled = false;
                    txtNewPassword.Enabled = false;
                    txtConfirmNewPassword.Enabled = false;

                    txtOldPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmNewPassword.Text = "";
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Thay đổi mật khẩu thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Cập nhật thông tin cá nhân tài khoản đang đăng nhập
        private void btnUpdateEdit_Click(object sender, EventArgs e)
        {
            if (btnUpdateEdit.Text == "Thay đổi")
            {
                btnUpdateEdit.Text = "Lưu";
                txtFullnameEdit.Enabled = true;
                txtAddressEdit.Enabled = true;
                txtPhoneEdit.Enabled = true;
                dtBirthdayEdit.Enabled = true;
            }
            else
            {
                btnUpdateEdit.Text = "Thay đổi";
                txtFullnameEdit.Enabled = false;
                txtAddressEdit.Enabled = false;
                txtPhoneEdit.Enabled = false;
                dtBirthdayEdit.Enabled = false;

                try
                {
                    loggedAccount.Fullname = txtFullnameEdit.Text;
                    loggedAccount.Address = txtAddressEdit.Text;
                    loggedAccount.Phone = txtPhoneEdit.Text;
                    loggedAccount.Birthday = dtBirthdayEdit.Value;

                    AccountBus.Update(loggedAccount);
                    MessageBox.Show("Thông tin tài khoản được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvAccount.DataSource = AccountBus.GetAccountList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Cập nhật thông tin tài khoản thất bại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Xử lý chọn thư mục lưu trữ dữ liệu
        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dataFolder = new FolderBrowserDialog();
            dataFolder.ShowDialog();
            if (dataFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDataFolderPath.Text = dataFolder.SelectedPath;
                btnBackup.Enabled = true;
            }
        }


        // Xử lý sao lưu dữ liệu
        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dlgDataFolder = new FolderBrowserDialog();
                if (dlgDataFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtDataFolderPath.Text = dlgDataFolder.SelectedPath;
                    DataBus.Backup(txtDataFolderPath.Text);
                    MessageBox.Show("Sao lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi sao lưu dữ liệu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Xử lý phục hồi dữ liệu
        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog dlgRestoreFile = new OpenFileDialog();
                dlgRestoreFile.Filter = "Backup Files(*.bak)|*.bak|All files(*.*)|*.*";
                dlgRestoreFile.FilterIndex = 0;

                if (dlgRestoreFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtDataFolderPath.Text = dlgRestoreFile.FileName;
                    DataBus.Restore(txtDataFolderPath.Text);
                    MessageBox.Show("Phục hồi dữ liệu thành công!\nVui lòng khởi động lại chương trình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi khôi phục dữ liệu.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        // Đăng xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            _dashboard = null;
            frmLogin loginScreen = new frmLogin();
            loginScreen.Show();
            this.Hide();
        }


        // Lấy danh sách bàn
        private void cmbDSKhuHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSection = 0;

                // Lấy ID khu đang chọn trong combobox
                var selected = cmbDSKhuHD.SelectedValue;
                if (selected is Section)
                {
                    Section selectedSection = (DataTransferObject.Section)selected;
                    idSection = selectedSection.ID;
                }
                else
                    idSection = int.Parse(cmbDSKhuHD.SelectedValue.ToString());

                // Lấy thông tin từ CSDL và gán vào DataGridView
                ArrayList tableList = TableBus.GetTableListInSection(idSection);
                cmbDSBanHD.DataSource = tableList;
                cmbDSBanHD.DisplayMember = "Name";
                cmbDSBanHD.ValueMember = "ID";
                cmbDSBanHD.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi lấy danh sách bàn.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
