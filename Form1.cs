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
            // Tạo bảng danh sách Khoa giả lập cho ComboBox
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
                string tenKhoa = cboKhoa.Text; // Lấy tên khoa hiển thị để đồng bộ với báo cáo

                // Tạo bảng dữ liệu Sinh viên giả lập khớp với DataSetStudent.xsd
                DataTable data = new DataTable("StudentTable");
                data.Columns.Add("MaSV", typeof(string));
                data.Columns.Add("HoTen", typeof(string));
                data.Columns.Add("MaKhoa", typeof(string));
                data.Columns.Add("TenKhoa", typeof(string));
                data.Columns.Add("DiemTB", typeof(double)); // Bổ sung cột Điểm trung bình

                // Thêm dữ liệu mẫu phù hợp với mã khoa chọn
                if (maKhoa == "KH01")
                {
                    data.Rows.Add("SV01", "Nguyễn Duy Hưng", "KH01", tenKhoa, 8.5);
                    data.Rows.Add("SV02", "Trần Văn An", "KH01", tenKhoa, 9.2);
                }
                else if (maKhoa == "KH02")
                {
                    data.Rows.Add("SV03", "Lê Thị Hồng", "KH02", tenKhoa, 7.4);
                }
                else
                {
                    data.Rows.Add("SV04", "Phạm Hoàng Long", "KH03", tenKhoa, 6.8);
                }

                // Khởi tạo báo cáo rptStudent
                rptStudent report = new rptStudent();
                report.SetDataSource(data);

                // Truyền giá trị vào các tham số trên báo cáo
                report.SetParameterValue("pTenKhoa", tenKhoa);
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