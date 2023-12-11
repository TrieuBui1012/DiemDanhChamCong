using DiemDanhChamCong.Models;
using System;

using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing.QrCode.Internal;
using static DiemDanhChamCong.ChamCongData;
using static System.Net.Mime.MediaTypeNames;

using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;



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
            TC_Loaded();
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
        private void QLBC_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> thang = new List<int>();
            for(int i = 1; i <= 12; i++)
            {
                thang.Add(i);
            }
            cbThangQLBC.ItemsSource = thang;
            List<int> nam = new List<int>();
            int curYear = DateTime.Now.Year;
            for(int i = 2000; i <= curYear; i++)
            {
                nam.Add(i);
            }
            cbNamQLBC.ItemsSource = nam;
            cbNamQLBC.SelectedItem = curYear;
        }
        private void btnTimQLBC_Click(object sender, RoutedEventArgs e)
        {
            QuanLyBangCong data = new QuanLyBangCong();
            if(cbThangQLBC.SelectedItem == null)
            {
                List<QuanLyBangCong.QuanLyBangCongData> listBC = data.LayDLBangCong(null, Convert.ToInt64(cbNamQLBC.SelectedItem), tbTimMaNVQLBC.Text.Trim());
                dgvQLBC.ItemsSource = listBC;

            }
            else
            {
                List<QuanLyBangCong.QuanLyBangCongData> listBC = data.LayDLBangCong(Convert.ToInt64(cbThangQLBC.SelectedItem), Convert.ToInt64(cbNamQLBC.SelectedItem), tbTimMaNVQLBC.Text.Trim());
                dgvQLBC.ItemsSource = listBC;
                
            }
        }
        private void Clear_dgvQLBC()
        {
            tbMaBCQLBC.Text = null;
            tbMaNVQLBC.Text = null;
            tbTenNVQLBC.Text = null;
            tbMuonQLBC.Text = null;
            tbSomQLBC.Text = null;
            tbCongQLBC.Text = null;
            tbLuongQLBC.Text = null;
        }
        private void dgvQLBC_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgvQLBC.SelectedItem != null)
            {
                try
                {
                    QuanLyBangCong.QuanLyBangCongData data = (QuanLyBangCong.QuanLyBangCongData)dgvQLBC.SelectedItem;
                    tbMaBCQLBC.Text = data.IDBC.ToString();
                    tbMaNVQLBC.Text = data.MaNV;
                    tbTenNVQLBC.Text = data.TenNV;
                    tbMuonQLBC.Text = data.Muon.ToString();
                    tbSomQLBC.Text = data.Som.ToString();
                    tbCongQLBC.Text = data.Cong.ToString();
                    tbLuongQLBC.Text = data.Luong.ToString();
                }
                catch
                {
                    MessageBox.Show("Có lỗi khi chọn hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSuaQLBC_Click(object sender, RoutedEventArgs e)
        {
            QuanLyBangCong data = new QuanLyBangCong();
            if(tbMuonQLBC.Text == null || tbSomQLBC.Text == null || tbCongQLBC.Text == null || tbLuongQLBC.Text == null)
            {
                MessageBox.Show("Các trường dữ liệu không được để trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(!Regex.IsMatch(tbMuonQLBC.Text, @"\d+"))
            {
                MessageBox.Show("Số giờ muộn bị nhập sai kiểu dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Regex.IsMatch(tbSomQLBC.Text, @"\d+"))
            {
                MessageBox.Show("Số giờ ra sớm bị nhập sai kiểu dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Regex.IsMatch(tbCongQLBC.Text, @"\d+"))
            {
                MessageBox.Show("Tổng công bị nhập sai kiểu dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Regex.IsMatch(tbLuongQLBC.Text, @"\d+"))
            {
                MessageBox.Show("Tổng lương bị nhập sai kiểu dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tbLuongQLBC.Text.Contains('.'))
            {
                MessageBox.Show("Tổng lương chỉ có thể là số nguyên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                data.Sua(Convert.ToInt64(tbMaBCQLBC.Text), Convert.ToDouble(tbMuonQLBC.Text), Convert.ToDouble(tbSomQLBC.Text), Convert.ToDouble(tbCongQLBC.Text), Convert.ToInt64(tbLuongQLBC.Text));
                if (cbThangQLBC.SelectedItem == null)
                {
                    List<QuanLyBangCong.QuanLyBangCongData> listBC = data.LayDLBangCong(null, Convert.ToInt64(cbNamQLBC.SelectedItem), tbTimMaNVQLBC.Text.Trim());
                    dgvQLBC.ItemsSource = listBC;
                }
                else
                {
                    List<QuanLyBangCong.QuanLyBangCongData> listBC = data.LayDLBangCong(Convert.ToInt64(cbThangQLBC.SelectedItem), Convert.ToInt64(cbNamQLBC.SelectedItem), tbTimMaNVQLBC.Text.Trim());
                    dgvQLBC.ItemsSource = listBC;
                }
                Clear_dgvQLBC();
            }
        }

        private void btnXoaQLBC_Click(object sender, RoutedEventArgs e)
        {
            QuanLyBangCong data = new QuanLyBangCong();
            if(dgvQLBC.SelectedItem == null)
            {
                MessageBox.Show("Bạn cần chọn 1 hàng trong danh sách bảng công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(MessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    data.Xoa(Convert.ToInt64(tbMaBCQLBC.Text));
                    if (cbThangQLBC.SelectedItem == null)
                    {
                        List<QuanLyBangCong.QuanLyBangCongData> listBC = data.LayDLBangCong(null, Convert.ToInt64(cbNamQLBC.SelectedItem), tbTimMaNVQLBC.Text.Trim());
                        dgvQLBC.ItemsSource = listBC;
                    }
                    else
                    {
                        List<QuanLyBangCong.QuanLyBangCongData> listBC = data.LayDLBangCong(Convert.ToInt64(cbThangQLBC.SelectedItem), Convert.ToInt64(cbNamQLBC.SelectedItem), tbTimMaNVQLBC.Text.Trim());
                        dgvQLBC.ItemsSource = listBC;
                    }
                    Clear_dgvQLBC();
                }
                
            }
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

        //Tính công
        
        TinhCong TC = new TinhCong();
        long selectedYear = 0;
        long selectedMonth;
        long soNgayNghiPhep;
        
        public void TC_Loaded()
        {
            for (int year = 2000; year <= DateTime.Now.Year; year++)
            {
                yearComboBox.Items.Add(new ComboBoxItem() { Content = year.ToString() });
            }
        }

        private void cbTC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (monthComboBox.SelectedIndex >= 0)
            {
                selectedMonth = monthComboBox.SelectedIndex + 1;
            }
            else 
            { 
                selectedMonth = DateTime.Now.Month;
                monthComboBox.SelectedIndex = DateTime.Now.Month + 1;
            }
            
            if (yearComboBox.SelectedIndex >= 0)
            {
                selectedYear = 2000 + yearComboBox.SelectedIndex;
            }
            else 
            {
                selectedYear = DateTime.Now.Year;
                //yearComboBox.SelectedIndex = DateTime.Now.Year - 2000;
                yearComboBox.SelectedIndex = DateTime.Now.Year - 2000;
            }
        }

        private void Button_TinhCong_Click(object sender, RoutedEventArgs e)
        {
            if(monthComboBox.SelectedIndex < 0 && yearComboBox.SelectedIndex < 0) 
            {
                selectedMonth = DateTime.Now.Month;
                monthComboBox.SelectedIndex = DateTime.Now.Month - 1;

                selectedYear = DateTime.Now.Year;
                yearComboBox.SelectedIndex = DateTime.Now.Year - 2000;
            }

            if(txt_ngaynghi.Text != "")
            {
                soNgayNghiPhep = long.Parse(txt_ngaynghi.Text);
            }
            else
            {
                soNgayNghiPhep = 0;
                txt_ngaynghi.Text = "0";
            }
            //List<TinhCongData> dt = TC.LayDL(selectedMonth, selectedYear, soNgayNghiPhep);
            //foreach(var i in dt)
            //{
            //    i.TongLuong = (long?)(((double)i.MucLuong / (double)(DateTime.DaysInMonth(Convert.ToInt32(selectedYear), Convert.ToInt32(selectedMonth)) - soNgayNghiPhep)) * i.TongCong) - (long?)(((double)i.MucLuong / (double)((DateTime.DaysInMonth(Convert.ToInt32(selectedYear), Convert.ToInt32(selectedMonth)) - soNgayNghiPhep) * 8) ) * (i.SoGioMuon + i.SoGioSom));
            //}

            dtg_TinhCong.ItemsSource = TC.LayDL(selectedMonth, selectedYear, soNgayNghiPhep);
            if(dtg_TinhCong.Items.Count <= 0) 
            {
                MessageBox.Show("Không tìm thấy dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Luu_Click(object sender, RoutedEventArgs e)
        {
            if(dtg_TinhCong.Items.Count > 0)
            {
                TC.Luu(selectedMonth, selectedYear, soNgayNghiPhep);
            }
            else 
            {
                MessageBox.Show("Chưa có dữ liệu để lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtg_TinhCong.Items.Count > 0)
            {
                ExportToExcel(dtg_TinhCong, selectedMonth, selectedYear);
            }
            else
            {
                MessageBox.Show("Chưa có dữ liệu để lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcel(DataGrid dataGrid, long month, long year)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = $"Bảng công tháng {month}-{year}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var document = SpreadsheetDocument.Create(saveFileDialog.FileName, SpreadsheetDocumentType.Workbook))
                    {
                        var workbookPart = document.AddWorkbookPart();
                        workbookPart.Workbook = new Workbook();

                        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet();

                        var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                        var sheet = new Sheet { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                        sheets.Append(sheet);

                        var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                        var columns = dataGrid.Columns;

                        // Add header row
                        var headerRow = new Row();
                        foreach (var column in columns)
                        {
                            headerRow.AppendChild(CreateCell(column.Header.ToString()));
                        }
                        sheetData.AppendChild(headerRow);

                        // Add data rows
                        foreach (var item in dataGrid.Items)
                        {
                            var row = new Row();
                            foreach (var column in columns)
                            {
                                var binding = (column as DataGridBoundColumn).Binding as System.Windows.Data.Binding;
                                var cellValue = item.GetType().GetProperty(binding.Path.Path).GetValue(item, null);
                                row.AppendChild(CreateCell(cellValue.ToString()));
                            }
                            sheetData.AppendChild(row);
                        }
                    }
                    MessageBox.Show("Đã lưu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private Cell CreateCell(string text)
        {
            return new Cell(new InlineString(new DocumentFormat.OpenXml.Spreadsheet.Text(text))); 
        }

        private void btnXuatBCQLBC_Click(object sender, RoutedEventArgs e)
        {
            if (dgvQLBC.Items.Count > 0)
            {
                ExportToExcel(dgvQLBC, Convert.ToInt64(cbThangQLBC.SelectedItem), Convert.ToInt64(cbNamQLBC.SelectedItem));
            }
            else
            {
                MessageBox.Show("Chưa có dữ liệu để lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClearLSCC_Click(object sender, RoutedEventArgs e)
        {
            dprNgayLSCC.SelectedDate = DateTime.Now.Date;
            cbCaLSCC.SelectedItem = null;
            tbMaCCLSCC.Text = null;
            tbMaNVLSCC.Text = null;
        }
    }
}
