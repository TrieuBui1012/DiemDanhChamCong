using DiemDanhChamCong.Models;
using System;

using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing.QrCode.Internal;
using static DiemDanhChamCong.ChamCongData;

namespace DiemDanhChamCong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow : Window
    {
        ChamCongContext db = new ChamCongContext();


        public MainWindow()
        {
            InitializeComponent();
            HienThiDLLC();
            HienThiDLC();
        }

        BaoTriLoaiCa baotriloaica = new BaoTriLoaiCa();

        private void HienThiDLLC()
        {
            dtg_BaoTriLoaiCa.ItemsSource = baotriloaica.LayDL();
        }
        private void HienThiDLC()
        {
            dtg_BaoTriCong.ItemsSource = baotricong.LayDL();
        }


        //Bảo trì loại ca
        private void dtg_BaoTriLoaiCa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (dtg_BaoTriLoaiCa.SelectedItem != null)
            {
                try
                {
                    Type t = dtg_BaoTriLoaiCa.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txt_idLoaiCa.Text = p[0].GetValue(dtg_BaoTriLoaiCa.SelectedValue).ToString();
                    txt_tenLoaiCa.Text = p[1].GetValue(dtg_BaoTriLoaiCa.SelectedValue).ToString();
                    txt_gioVao.Text = p[2].GetValue(dtg_BaoTriLoaiCa.SelectedValue).ToString();
                    txt_gioRa.Text = p[3].GetValue(dtg_BaoTriLoaiCa.SelectedValue).ToString();
                    txt_heSo.Text = p[4].GetValue(dtg_BaoTriLoaiCa.SelectedValue).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi chọn hàng" + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_themloaica(object sender, RoutedEventArgs e)
        {
            try
            {
                if (baotriloaica.CheckDL(txt_idLoaiCa.Text, txt_tenLoaiCa.Text, txt_gioVao.Text, txt_gioRa.Text, txt_heSo.Text))
                {
                    Loaica c = new Loaica();

                    c.IdloaiCa = long.Parse(txt_idLoaiCa.Text);
                    c.TenLoaiCa = txt_tenLoaiCa.Text;

                    if (TimeOnly.TryParse(txt_gioVao.Text, out TimeOnly Vao) && TimeOnly.TryParse(txt_gioRa.Text, out TimeOnly Ra))
                    {
                        c.Vao = Vao;
                        c.Ra = Ra;
                        c.HeSo = double.Parse(txt_heSo.Text);
                        baotriloaica.addLoaiCa(c);
                        HienThiDLLC();
                        MessageBox.Show("Đã thêm thành công.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập đúng định dạng HH:mm.");
                    }
                    txt_idLoaiCa.Text = "";
                    txt_tenLoaiCa.Text = "";
                    txt_gioVao.Text = "";
                    txt_gioRa.Text = "";
                    txt_heSo.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_xoaloaica(Object sender, RoutedEventArgs e)
        {
            baotriloaica.deleteLoaiCa(txt_idLoaiCa.Text);
            HienThiDLLC();
            MessageBox.Show("Đã xóa thành công.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txt_idLoaiCa.Text = "";
            txt_tenLoaiCa.Text = "";
            txt_gioVao.Text = "";
            txt_gioRa.Text = "";
            txt_heSo.Text = "";
        }

        private void btn_sualoaica(Object sender, RoutedEventArgs e)
        {
            baotriloaica.changeLoaiCa(txt_idLoaiCa.Text,txt_tenLoaiCa.Text, txt_gioVao.Text, txt_gioRa.Text, txt_heSo.Text);
            HienThiDLLC();
            MessageBox.Show("Đã sửa thành công.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txt_idLoaiCa.Text = "";
            txt_tenLoaiCa.Text = "";
            txt_gioVao.Text = "";
            txt_gioRa.Text = "";
            txt_heSo.Text = "";
        }

        //Bảo trì loại công
        BaoTriCong baotricong = new BaoTriCong();
        private void dtg_BaoTriCong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_BaoTriCong.SelectedItem != null)
            {
                try
                {
                    Type t = dtg_BaoTriCong.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txt_idLoaiCong.Text = p[0].GetValue(dtg_BaoTriCong.SelectedValue).ToString();
                    txt_tenLoaiCong.Text = p[1].GetValue(dtg_BaoTriCong.SelectedValue).ToString();
                    txt_ngay.Text = p[2].GetValue(dtg_BaoTriCong.SelectedValue).ToString();
                    txt_heSoCong.Text = p[3].GetValue(dtg_BaoTriCong.SelectedValue).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi chọn hàng" + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void btn_themloaicong(object sender, RoutedEventArgs e)
        {
            try
            {
                if (baotricong.CheckDL(txt_idLoaiCong.Text, txt_tenLoaiCong.Text, txt_ngay.Text, txt_heSoCong.Text))
                {
                    Loaicong c = new Loaicong();


                    c.IdloaiCong = long.Parse(txt_idLoaiCong.Text);
                    c.TenLoaiCong = txt_tenLoaiCong.Text;

                    if (DateOnly.TryParse(txt_ngay.Text, out DateOnly ngay))
                    {
                        c.Ngay = ngay;
                    }
                    else
                    {
                        MessageBox.Show("Invalid time format. Please enter time in MM-dd format.");
                    }
                    c.HeSo = double.Parse(txt_heSoCong.Text);
                    baotricong.addLoaiCong(c);
                    HienThiDLC();
                    MessageBox.Show("Đã thêm thành công.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    txt_idLoaiCong.Text = "";
                    txt_tenLoaiCong.Text = "";
                    txt_ngay.Text = "";
                    txt_heSoCong.Text = "";
                }
            }   
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btn_xoaloaicong(Object sender, RoutedEventArgs e)
        {
            baotricong.deleteLoaiCong(txt_idLoaiCong.Text);
            HienThiDLC();
            MessageBox.Show("Đã xóa thành công.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txt_idLoaiCong.Text = "";
            txt_tenLoaiCong.Text = "";
            txt_ngay.Text = "";
            txt_heSoCong.Text = "";
        }

        private void btn_sualoaicong(Object sender, RoutedEventArgs e)
        {
            baotricong.changeLoaiCong(txt_idLoaiCong.Text, txt_tenLoaiCong.Text, txt_ngay.Text, txt_heSoCong.Text);
            HienThiDLC();
            MessageBox.Show("Đã sửa thành công.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txt_idLoaiCong.Text = "";
            txt_tenLoaiCong.Text = "";
            txt_ngay.Text = "";
            txt_heSoCong.Text = "";
        }
        private List<NhanVien1> LayDl()
        {
            var querry = from t in db.Nhanviens
                         select new NhanVien1
                         {
                             MaNv = t.MaNv,
                             HoTen = t.HoTen,
                             DiaChi = t.DiaChi,
                             GioiTinh = (t.GioiTinh ==1)?"Nam" : "Nữ",
                             Cccd = t.Cccd,
                             DienThoai = t.DienThoai,
                             Idluong = t.Idluong,
                             Idpb = t.Idpb,
                             NgaySinh = (t.NgaySinh.ToString("dd/MM/yyyy"))
                         };
            return querry.ToList<NhanVien1>();
        }
        private List<Phongban> LayDSPB()
        {
            var querry = from t in db.Phongbans
                         select new Phongban
                         {
                             Idpb = t.Idpb,
                             TenPb = t.TenPb,
                         };
            return querry.ToList<Phongban>();
        }
        private List<Luong> LayDSL()
        {
            var querry = from t in db.Luongs
                         select new Luong
                         {
                             Idluong = t.Idluong,
                             MucLuong = t.MucLuong,
                             TenLuong = t.TenLuong
                         };
            return querry.ToList<Luong>();
        }
        private void HienthiNV()
        {
            dgNVBTNV.ItemsSource = LayDl();
        }
        private void HienthiPB()
        {
            dgPB.ItemsSource = LayDSPB();
        }
        private void HienthiL()
        {
            dgLuong.ItemsSource = LayDSL();
        }
        private void btThemPhong_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (CheckDLPB())
                {
                    var querry = db.Phongbans.FirstOrDefault(pb => pb.Idpb.Equals(Convert.ToInt64(txtMaPhong.Text)));
                    if (querry != null)
                    {
                        MessageBox.Show("Mã phòng ban đã tồn tại ", "Thông báo ", MessageBoxButton.OK);

                    }
                    else
                    {
                        Phongban pb = new Phongban();
                        pb.Idpb = Convert.ToInt64(txtMaPhong.Text);
                        pb.TenPb = txtTenPB.Text;
                        db.Phongbans.Add(pb);
                        MessageBox.Show("Thêm thành công! ", "Thong bao", MessageBoxButton.OK);
                        db.SaveChanges();
                        HienthiPB();
                        XoaDLPB();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckDLNV())
                {
                    var querry = db.Nhanviens.FirstOrDefault(nv => nv.MaNv.Equals(txtMaNVBTNV.Text));
                    if(querry != null)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại ", "Thông báo ", MessageBoxButton.OK);
                        HienthiNV();
                    }
                    else{
                        Nhanvien nv = new Nhanvien();
                        nv.MaNv = txtMaNVBTNV.Text;
                        nv.HoTen = txtTenNVBTNV.Text;
                        nv.GioiTinh = (rbNam.IsChecked == true) ? 1 : 0;
                        nv.Cccd = txtCCCDBTNV.Text;
                        nv.DiaChi = txtDiaChiBTNV.Text;
                        nv.DienThoai = txtDienThoaiBTNV.Text;
                        nv.Idpb = Convert.ToInt64(txtMaPBBTNV.Text);
                        nv.Idluong = Convert.ToInt64(txtMaBangLuongBTNV.Text);
                        nv.NgaySinh = dpDateBTNV.SelectedDate.Value.Date;
                        db.Nhanviens.Add(nv);
                        MessageBox.Show("Thêm thành công! ", "Thong bao", MessageBoxButton.OK);
                        db.SaveChanges();
                        HienthiNV();
                        XoaDLNV();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " +ex.Message, "Thông báo", MessageBoxButton.OK,MessageBoxImage.Error); 
            }
        }

        private void btThemLuong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var querry = db.Luongs.FirstOrDefault(l => l.Idluong.Equals(Convert.ToInt64(txtMaLuong.Text)));
                if(querry != null)
                {
                    MessageBox.Show("Đã tồn tại mã lương ", "Thông báo ", MessageBoxButton.OK);
                }
                else
                {
                    Luong l = new Luong();
                    l.Idluong = Convert.ToInt64(txtMaLuong.Text);
                    l.TenLuong = txtTenLuong.Text;
                    l.MucLuong = Convert.ToInt64(txtMucLuong.Text);
                    db.Luongs.Add(l);
                    db.SaveChanges();
                    MessageBox.Show("Thêm thành công! ", "Thong bao", MessageBoxButton.OK);
                    HienthiL();
                    XoaDLL();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CheckDLNV()
        {
            string mess = "";
            if (txtMaNVBTNV.Text == "" || txtTenNVBTNV.Text == "" || txtCCCDBTNV.Text == "" || txtDiaChiBTNV.Text == "" || txtDienThoaiBTNV.Text == "" || (dpDateBTNV.SelectedDate) == null )
            {
                mess = "\nBạn cần nhập đủ trường dữ liệu";
            }
            if (!Regex.IsMatch(txtMaBangLuongBTNV.Text, @"\d+|^$"))
            {
                mess = "\nNhập sai loại dữ liệu mã bảng lương ";
            }
            if (!Regex.IsMatch(txtMaPBBTNV.Text, @"\d+|^$") )
            {
                mess = "\nNhập sai loại dữ liệu mã phòng ban ";
            }
            if(!Regex.IsMatch(txtDienThoaiBTNV.Text, @"\d+"))
            {
                mess = "\nSố điện thoại không chứa ký tự và ký tự đặc biêt";
            }
            if (!Regex.IsMatch(txtCCCDBTNV.Text, @"\d+"))
            {
                mess = "\nCăn cước công dân không chứa ký tự và ký tự đặc biêt";
            }
            if (mess != "")
            {
                MessageBox.Show(mess,"Thông báo",MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private bool CheckDLLuong()
        {
            string mess = "";
            if(txtMaLuong.Text == "" || txtMucLuong.Text == ""  || txtTenLuong.Text == "")
            {
                mess = "\nBạn cần nhập đủ trường dữ liệu";
            }
            if (!Regex.IsMatch(txtMaLuong.Text, @"\d+"))
            {
                mess = "\nNhập sai loại dữ liệu mã luong ";
            }
            if (!Regex.IsMatch(txtMucLuong.Text, @"\d+"))
            {
                mess = "\nNhập sai loại dữ liệu mức luong ";
            }
            if (mess != "")
            {
                MessageBox.Show(mess, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private bool CheckDLPB()
        {
            string mess = "";
            if (txtMaPhong.Text==""||txtTenPB.Text=="")
            {
                mess = "\nBạn cần nhập đủ trường dữ liệu";
            }
            if (!Regex.IsMatch(txtMaPhong.Text, @"\d+"))
            {
                mess = "\nNhập sai loại dữ liệu mã phòng ban";
            }
            if (mess != "")
            {
                MessageBox.Show(mess, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btChichXuatNV_Click(object sender, RoutedEventArgs e)
        {
            Nhanvien nv = LayDLNV();
            
            if (nv != null)
            {
                txtMaNVBTNV.Text = nv.MaNv;
                txtDienThoaiBTNV.Text = nv.DienThoai;
                txtTenNVBTNV.Text = nv.HoTen;
                txtDiaChiBTNV.Text = nv.DiaChi;
                txtCCCDBTNV.Text = nv.Cccd;
                dpDateBTNV.SelectedDate = nv.NgaySinh;
                txtMaPBBTNV.Text = Convert.ToString(nv.Idpb);
                txtMaBangLuongBTNV.Text = Convert.ToString(nv.Idluong);
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên với mã: " + txtMaNVBTNVs.Text);
            }

        }
        private Nhanvien LayDLNV()
        {
            var maNv = txtMaNVBTNVs.Text;
            var nhanVien = db.Nhanviens.FirstOrDefault(nv => nv.MaNv == maNv);
            return nhanVien;
        }

        private void btCapNhatNV_Click(object sender, RoutedEventArgs e)
        {
            Nhanvien nv = LayDLNV();
            if (CheckDLNV())
            {
                nv.HoTen = txtTenNVBTNV.Text;
                nv.DienThoai = txtDienThoaiBTNV.Text;
                nv.DiaChi = txtDiaChiBTNV.Text;


                if(txtMaPBBTNV.Text == "")
                {
                    nv.Idpb = null;
                }
                else
                {
                    nv.Idpb = Convert.ToInt64(txtMaPBBTNV.Text);
                }
                if (txtMaBangLuongBTNV.Text == "")
                {
                    nv.Idluong = null;
                }
                else
                {
                    nv.Idluong = Convert.ToInt64(txtMaBangLuongBTNV.Text);

                }
                nv.Cccd = txtCCCDBTNV.Text;
                nv.GioiTinh = (rbNam.IsChecked == true) ? 1 : 0;
                nv.NgaySinh = dpDateBTNV.SelectedDate.Value.Date;
                db.SaveChanges();
                HienthiNV();
                XoaDLNV();
            }
        }
        private void XoaDLNV()
        {
            txtMaNVBTNV.Text = "";
            txtTenNVBTNV.Text = "";
            txtDienThoaiBTNV.Text = "";
            txtDiaChiBTNV.Text = "";
            txtMaPBBTNV.Text = "";
            txtMaBangLuongBTNV.Text = "";
            txtCCCDBTNV.Text = "";
            dpDateBTNV.SelectedDate = null;
        }

        private void btXoaNV_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Nhanvien nv = LayDLNV();
                if(nv != null)
                {
                    MessageBoxResult rs = MessageBox.Show("Bạn có muốn xóa nhân viên này không ?", "Thong bao", MessageBoxButton.YesNoCancel);
                    if(rs == MessageBoxResult.Yes)
                    {
                        db.Nhanviens.Remove(nv);
                        db.SaveChanges();
                        HienthiNV();
                        XoaDLNV();
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Có lỗi khi xóa : " + ex.Message, "Thong bao", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btChichXuatPhong_Click(object sender, RoutedEventArgs e)
        {
            Phongban pb = LayDLPB();
            if(pb != null)
            {
                txtMaPhong.Text = Convert.ToString(pb.Idpb);
                txtTenPB.Text = pb.TenPb;
            }
            else
            {
                MessageBox.Show("Không tìm thấy phòng ban với mã: " + txtMaPBBTNV.Text);
            }

        }
        private Phongban LayDLPB()
        {
            long maPhong = Convert.ToInt64(txtMaPhongS.Text);
            var phongBan = db.Phongbans.FirstOrDefault(pb => pb.Idpb == maPhong);
            return phongBan;
        }

        private void btXoaPhong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Phongban pb = LayDLPB();
                if(pb != null)
                {
                    MessageBoxResult rs = MessageBox.Show("Bạn có muốn xóa phong ban này không ?", "Thong bao", MessageBoxButton.YesNoCancel);
                    if (rs == MessageBoxResult.Yes)
                    {
                        db.Phongbans.Remove(pb);
                        db.SaveChanges();
                        HienthiPB();
                        XoaDLPB();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Không thể xóa vì có nhân viên tồn tại trong phòng ban này !" , "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btCapNhatPhong_Click(object sender, RoutedEventArgs e)
        {
            Phongban pb = LayDLPB();
            if (CheckDLPB())
            {
                pb.TenPb = txtTenPB.Text;
                db.SaveChanges();
                XoaDLPB();
                HienthiPB();
            }
        }
        private void XoaDLPB()
        {
            txtMaPhong.Text = "";
            txtTenPB.Text = "";
        }

        private void btChichXuatLuong_Click(object sender, RoutedEventArgs e)
        {
            Luong l = LayDLL();
            if (l != null)
            {
                txtMaLuong.Text = l.Idluong.ToString();
                txtTenLuong.Text = l.TenLuong;
                txtMucLuong.Text = l.MucLuong.ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy lương với mã: " + txtMaLuong.Text);
            }
        }
        private Luong LayDLL()
        {
            var maLuong = txtMaLuongS.Text;
            var Luong = db.Luongs.FirstOrDefault(l => l.Idluong == Convert.ToInt64(maLuong));
            return Luong;
        }

        private void btCapNhatLuong_Click(object sender, RoutedEventArgs e)
        {
            Luong l = LayDLL();
            if (CheckDLLuong())
            {
                l.TenLuong = txtTenLuong.Text;
                l.MucLuong = Convert.ToInt64(txtMucLuong.Text);
                db.SaveChanges();
                XoaDLL();
                HienthiL();
            }
        }
        private void XoaDLL()
        {
            txtMucLuong.Text = "";
            txtMaLuong.Text = "";
            txtTenLuong.Text = "";
        }

        private void btXoaLuong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Luong l = LayDLL();
                if (l != null)
                {
                    MessageBoxResult rs = MessageBox.Show("Bạn có muốn xóa luơng này không ?", "Thông báo ", MessageBoxButton.YesNoCancel);
                    if (rs == MessageBoxResult.Yes)
                    {
                        db.Luongs.Remove(l);
                        db.SaveChanges();
                        HienthiL();
                        XoaDLL();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa vì có nhân viên đang áp dụng lương này !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            HienthiNV();
            HienthiL();
            HienthiPB();
        }

        private void HienThi_cbLoaiCaCC()
        {
            var query = from t in db.Loaicas
                        select t;
            cbLoaiCaCC.ItemsSource = query.ToList();
            cbLoaiCaCC.DisplayMemberPath = "TenLoaiCa";
            cbLoaiCaCC.SelectedValuePath = "IdloaiCa";
            cbLoaiCaCC.SelectedIndex = 0;
        }

        private void HienThi_dgvChamCongCC()
        {
            ChamCongData data = new ChamCongData();
            dgvChamCongCC.ItemsSource = data.LayDL((long?) cbLoaiCaCC.SelectedValue);
        }

        private void TabChamCong_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_cbLoaiCaCC();
            HienThi_dgvChamCongCC();
            DateOnly dateCur = DateOnly.FromDateTime(DateTime.Now);
            lblDateCurCC.Content = "Ngày hiện tại: " + dateCur.ToString("dd/MM/yyyy");
        }

        private void btnChamCongCC_Click(object sender, RoutedEventArgs e)
        {
            ChamCongData data = new ChamCongData();
            Dictionary<string, DateTime> qrCodesDataVao = ChamCongData.ScanQRImages("Vao");
            Dictionary<string, DateTime> qrCodesDataRa = ChamCongData.ScanQRImages("Ra");
            foreach(string str in qrCodesDataVao.Keys)
            {
                data.ChamCongVao(str, qrCodesDataVao[str], (long?)cbLoaiCaCC.SelectedValue);
            }
            foreach (string str in qrCodesDataRa.Keys)
            {
                data.ChamCongRa(str, qrCodesDataRa[str], (long?)cbLoaiCaCC.SelectedValue);
            }
            HienThi_dgvChamCongCC();
        }

        private void cbLoaiCaCC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienThi_dgvChamCongCC();
        }

        private void HienThi_cbCaLSCC()
        {
            var query = from t in db.Loaicas
                        select t;
            cbCaLSCC.ItemsSource = query.ToList();
            cbCaLSCC.DisplayMemberPath = "TenLoaiCa";
            cbCaLSCC.SelectedValuePath = "IdloaiCa";
            cbCaLSCC.SelectedIndex = -1;
        }

        private void TabLSCC_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_cbCaLSCC();
        }

        private void dgvChamCongLSCC_Changed()
        {
            dgvChamCongLSCC.ItemsSource = ChamCongData.dgvChamCongLSCC_Changed_Source(dprNgayLSCC.SelectedDate.Value.Date, (long?)cbCaLSCC.SelectedValue, tbMaNVLSCC.Text.Trim(), tbMaCCLSCC.Text.Trim());
        }

        private void dprNgayLSCC_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dgvChamCongLSCC_Changed();
        }

        private void cbCaLSCC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgvChamCongLSCC_Changed();

        }

        private void tbMaNVLSCC_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgvChamCongLSCC_Changed();
        }

        private void tbMaCCLSCC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbMaCCLSCC.Text.Trim().Length != 0)
            {
                try
                {
                    long? maCC = Convert.ToInt64(tbMaCCLSCC.Text.Trim());
                    if(maCC <= 0)
                    {
                        throw new Exception("Mã chấm công phải lớn hơn 0.");
                    }
                }
                catch
                {
                    MessageBox.Show("Mã chấm công phải là một số nguyên và lớn hơn 0!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            dgvChamCongLSCC_Changed();
        }
     
        public void LoadDgTK()
        {
            try
            {
                if(dp_StartDate.SelectedDate == null || dp_EndDate.SelectedDate == null)
                {
                    throw new Exception("Phải chọn cả ngày bắt đầu và ngày kết thúc !");
                }
                else
                {
                    DateTime startdate = dp_StartDate.SelectedDate.Value;
                    DateTime enddate = dp_EndDate.SelectedDate.Value;
                    if (startdate > enddate)
                    {
                        throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc !");

                    }
                    else
                    {
                        ThongKe tk = new ThongKe();
                        dg_ThongKe.ItemsSource = tk.LayDL(startdate, enddate);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Thông báo ",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void bt_XemChiTietTK_Click(object sender, RoutedEventArgs e)
        {

            if (dg_ThongKe.SelectedItem != null)
            {
                Window1 f = new Window1();
                f.startdate = dp_StartDate.SelectedDate.Value;
                f.enddate = dp_EndDate.SelectedDate.Value;
                f.Manv =((ThongKeData) dg_ThongKe.SelectedItem).MaNV;
                f.Show();
            }
            else
            {
                MessageBox.Show("Lỗi !","Thông báo",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void bt_TimKiemTK_Click(object sender, RoutedEventArgs e)
        {
            LoadDgTK();
        }

    }
}
