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
using System.Windows.Shapes;
using DiemDanhChamCong.Models;
namespace DiemDanhChamCong
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string Manv { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XemChiTiet t = new XemChiTiet();
            dg_ChiTietTK.ItemsSource = t.LayDL(Manv, startdate, enddate);
        }
    }
}
