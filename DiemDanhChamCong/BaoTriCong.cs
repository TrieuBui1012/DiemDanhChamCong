using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DiemDanhChamCong
{
    class BaoTriCong
    {
        ChamCongContext db = new();

        public bool CheckDL(string idloaicong, string tenloaicong, string ngay, string heso)
        {
            string mess = "";
            if (idloaicong == "" || tenloaicong == "" || heso == "")  
            {
                mess += "\nVui lòng nhập đầy đủ dữ liệu";
            }
            if (!Regex.IsMatch(idloaicong, @"\d+")) 
            {
                mess += "\nMã loại công phải là số nguyên";
            }
            
            if(mess != "")
            {
                MessageBox.Show(mess, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public List<Loaicong> LayDL()
        {
            var query = from t in db.Loaicongs
                        select new Loaicong
                        {
                            IdloaiCong = t.IdloaiCong,
                            TenLoaiCong = t.TenLoaiCong,
                            Ngay = t.Ngay,
                            HeSo = t.HeSo,
                        };
            return query.ToList<Loaicong>();
        }

        public void addLoaiCong(Loaicong c)
        {
            var query = db.Loaicongs.SingleOrDefault(t => t.IdloaiCong.Equals(c.IdloaiCong));
            if (query != null)
            {
                MessageBox.Show("Đã tồn tại", "Thông báo", MessageBoxButton.OK);
            }
            else
            {
                db.Loaicongs.Add(c);
                db.SaveChanges();
            }
        }

        public void deleteLoaiCong(string idloaicong)
        {
            try
            {
                var query = db.Loaicongs.SingleOrDefault(t => t.IdloaiCong.Equals(long.Parse(idloaicong)));
                if (query != null)
                {
                    MessageBoxResult rs = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNoCancel);
                    if (rs == MessageBoxResult.Yes)
                    {
                        db.Loaicongs.Remove(query);
                        db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi xóa: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Xóa toàn bộ dữ liệu LOAICONG
            //var query = from t in db.Loaicongs
            //            select new Loaicong
            //            {
            //                IdloaiCong = t.IdloaiCong,
            //                TenLoaiCong = t.TenLoaiCong,
            //                Ngay = t.Ngay,
            //                HeSo = t.HeSo,
            //            };
            //foreach (var t in  query)
            //{
            //    db.Loaicongs.Remove(t);
            //    db.SaveChanges() ;
            //}
        }

        public void changeLoaiCong(string idloaicong,string tenloaicong, string ngay, string heso)
        {
            try
            {

                var query = db.Loaicongs.SingleOrDefault(t => t.IdloaiCong == long.Parse(idloaicong));
                if (query != null)
                {
                    if (CheckDL(idloaicong, tenloaicong, ngay, heso))
                    {
                        query.TenLoaiCong = tenloaicong;
                        query.Ngay = DateOnly.Parse(ngay);
                        query.HeSo = double.Parse(heso);
                        db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy đối tượng cần sửa");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi sửa: " + ex.Message);
            }
        }
    }
}
