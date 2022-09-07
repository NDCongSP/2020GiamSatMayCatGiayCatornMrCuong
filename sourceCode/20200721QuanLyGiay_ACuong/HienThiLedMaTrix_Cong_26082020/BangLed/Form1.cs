using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BangLed
{
    public partial class Form1 : Form
    {
        Matrix _matrix = new Matrix();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // đường dẫn hình viền vùng
            //_matrix.pPath = Marshal.StringToHGlobalUni(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "100.png"));
         
        }
       
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát khỏi chương trình?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // khi nạp chỉ thay đổi những giá trị cần thiết(hiện đã chọn mặc định đẹp nhất vì thế nên thay đổi nội dung thôi)
            // Vùng 1 nếu muốn chuyển động thì cho thông số Area1ShowEffect =202 nhé.
            _matrix.ip = textIp.Text.Trim();
            if (checkBox1.Checked == true)// CÓ THỂ XEM CHIỀU DÀI TEXT ĐỂ LÀM ĐIỀU KIỆN CHUYỂN ĐỘNG HAY ĐỨNG IM. TEXT.LENG
            {
                _matrix.Area1ShowEffect = 202;
            }
            else
            {
                _matrix.Area1ShowEffect = 0;
            }
            // CHỈ THAY ĐỔI TEXT CỦA VÙNG 1,4,5,6,8,9,10,11,12 THÔI NHÉ.
            _matrix.Area1Text = textVung1.Text.Trim();
            //
            _matrix.Area4Text = textVung4.Text.Trim();
            // Còn lại
            _matrix.Area5Text = textVung5.Text.Trim();
            _matrix.Area6Text = textVung6.Text.Trim();
            _matrix.Area8Text = textVung8.Text.Trim();
            _matrix.Area9Text = textVung9.Text.Trim();
            _matrix.Area10Text = textVung10.Text.Trim();
            _matrix.Area11Text = textVung11.Text.Trim();
            _matrix.Area12Text = textVung12.Text.Trim();
            _matrix.Sendata();
            // NÊN DÙNG MỘT THREAD RIÊNG ĐỂ QUÉT LỆNH GỮI TỪ CHƯƠNG TRÌNH CHÍNH
            // TRÁNH TRƯỜNG HỢP MẤT KN ĐẾN BẢNG LED MATRIX LÀM ĐƠ TOÀN HỆ THỐNG, VÌ KHI MẤT KẾT NỐI LỆNH GỮI BỊ ĐƠ TẦM 10 GIÂY
            // CÓ THỂ DÙNG TRY CATH ĐỂ TỐI ƯU HỆ THỐNG NHÉ.
        }
    }
    
}
