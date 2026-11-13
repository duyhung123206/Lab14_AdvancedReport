using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace Lab14_AdvancedReport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDanhSachKhoa();
        }

        private void LoadDanhSachKhoa()
        {
            // Tạo bảng danh sách Khoa giả lập
            DataTable dtKhoa = new DataTable("KhoaTable");
            dtKhoa.Columns.Add("MaKhoa", typeof(string));
            dtKhoa.Columns.Add("TenKhoa", typeof(string));

            dtKhoa.Rows.Add("KH01", "Công nghệ thông tin");
            dtKhoa.Rows.Add("KH02", "Quản trị kinh doanh");
            dtKhoa.Rows.Add("KH03", "Kế toán kiểm toán");

            cboKhoa.DataSource = dtKhoa;
            cboKhoa.DisplayMember = "TenKhoa";
            cboKhoa.ValueMember = "MaKhoa";
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboKhoa.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khoa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maKhoa = cboKhoa.SelectedValue.ToString();

                // Tạo bảng dữ liệu Sinh viên giả lập có đặt tên trùng khớp để Crystal Reports nhận diện
                DataTable data = new DataTable("StudentTable");
                data.Columns.Add("MaSV", typeof(string));
                data.Columns.Add("HoTen", typeof(string));
                data.Columns.Add("MaKhoa", typeof(string));

                // Thêm dữ liệu mẫu phù hợp với mã khoa chọn
                if (maKhoa == "KH01")
                {
                    data.Rows.Add("SV01", "Nguyễn Văn Hiếu", "KH01");
                    data.Rows.Add("SV02", "Trần Văn An", "KH01");
                }
                else if (maKhoa == "KH02")
                {
                    data.Rows.Add("SV03", "Lê Thị Hồng", "KH02");
                }
                else
                {
                    data.Rows.Add("SV04", "Phạm Hoàng Long", "KH03");
                }

                // Khởi tạo báo cáo rptStudent
                rptStudent report = new rptStudent();

                // Gán nguồn dữ liệu
                report.SetDataSource(data);

                // Truyền tham số vào báo cáo
                report.SetParameterValue("pTenKhoa", cboKhoa.Text);
                report.SetParameterValue("pNgayLap", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                // Hiển thị lên Viewer
                crystalReportViewer1.ReportSource = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}