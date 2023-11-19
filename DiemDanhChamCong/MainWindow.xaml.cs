using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

namespace DiemDanhChamCong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
    }
}
