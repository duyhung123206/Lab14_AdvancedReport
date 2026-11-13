using System;
using System.Data;
using System.Linq;
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

        // 1. Chức năng xem báo cáo theo Khoa
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
                string tenKhoa = cboKhoa.Text;

                // Tạo bảng dữ liệu Sinh viên giả lập khớp với DataSetStudent.xsd
                DataTable data = new DataTable("StudentTable");
                data.Columns.Add("MaSV", typeof(string));
                data.Columns.Add("HoTen", typeof(string));
                data.Columns.Add("MaKhoa", typeof(string));
                data.Columns.Add("TenKhoa", typeof(string));
                data.Columns.Add("DiemTB", typeof(double));

                // Thêm dữ liệu mẫu toàn trường
                data.Rows.Add("SV01", "Nguyễn Duy Hưng", "KH01", "Công nghệ thông tin", 8.5);
                data.Rows.Add("SV02", "Trần Văn An", "KH01", "Công nghệ thông tin", 9.2);
                data.Rows.Add("SV03", "Lê Thị Hồng", "KH02", "Quản trị kinh doanh", 7.4);
                data.Rows.Add("SV04", "Phạm Hoàng Long", "KH03", "Kế toán kiểm toán", 6.8);

                // Lọc dữ liệu theo khoa được chọn trên ComboBox
                DataTable dataFiltered = data.Clone();
                var rows = data.AsEnumerable().Where(r => r.Field<string>("MaKhoa") == maKhoa);
                if (rows.Any())
                {
                    dataFiltered = rows.CopyToDataTable();
                }

                // Khởi tạo báo cáo rptStudent
                rptStudent report = new rptStudent();
                report.SetDataSource(dataFiltered);

                // Truyền giá trị vào các tham số trên báo cáo
                report.SetParameterValue("pTenKhoa", tenKhoa);
                report.SetParameterValue("pKhoangDiem", "Tất cả khoảng điểm");
                report.SetParameterValue("pNgayLap", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                // Hiển thị lên Viewer
                crystalReportViewer1.ReportSource = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Chức năng xem báo cáo theo khoảng điểm Min / Max
        private void btnXemTheoDiem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra định dạng số hợp lệ
                if (!decimal.TryParse(txtMin.Text, out decimal min) || !decimal.TryParse(txtMax.Text, out decimal max))
                {
                    MessageBox.Show("Vui lòng nhập điểm Min và Max hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra ràng buộc khoảng điểm [0, 10] và Min <= Max[cite: 15]
                if (min < 0 || max > 10 || min > max)
                {
                    MessageBox.Show("Khoảng điểm phải thỏa mãn: 0 <= Min <= Max <= 10!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tạo bảng dữ liệu mẫu đầy đủ
                DataTable data = new DataTable("StudentTable");
                data.Columns.Add("MaSV", typeof(string));
                data.Columns.Add("HoTen", typeof(string));
                data.Columns.Add("MaKhoa", typeof(string));
                data.Columns.Add("TenKhoa", typeof(string));
                data.Columns.Add("DiemTB", typeof(double));

                data.Rows.Add("SV01", "Nguyễn Duy Hưng", "KH01", "Công nghệ thông tin", 8.5);
                data.Rows.Add("SV02", "Trần Văn An", "KH01", "Công nghệ thông tin", 9.2);
                data.Rows.Add("SV03", "Lê Thị Hồng", "KH02", "Quản trị kinh doanh", 7.4);
                data.Rows.Add("SV04", "Phạm Hoàng Long", "KH03", "Kế toán kiểm toán", 6.8);

                // Lọc dữ liệu theo khoảng điểm [min, max][cite: 15]
                DataTable dataFiltered = data.Clone();
                var rows = data.AsEnumerable().Where(r => r.Field<double>("DiemTB") >= (double)min && r.Field<double>("DiemTB") <= (double)max);
                if (rows.Any())
                {
                    dataFiltered = rows.CopyToDataTable();
                }

                // Khởi tạo báo cáo rptStudent
                rptStudent report = new rptStudent();
                report.SetDataSource(dataFiltered);

                // Truyền tham số khoảng điểm và tiêu đề vào báo cáo[cite: 15]
                report.SetParameterValue("pKhoangDiem", $"Khoảng điểm từ {min} đến {max}");
                report.SetParameterValue("pTenKhoa", "BÁO CÁO THEO KHOẢNG ĐIỂM");
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