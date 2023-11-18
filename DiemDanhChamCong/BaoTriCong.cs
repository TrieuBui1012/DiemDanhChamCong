using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiemDanhChamCong
{
    class BaoTriCong
    {
        ChamCongContext db = new();

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
            db.Loaicongs.Add(c);
            db.SaveChanges();
        }

        public void deleteLoaiCong(string idloaicong)
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

            var query = db.Loaicongs.SingleOrDefault(t => t.IdloaiCong == long.Parse(idloaicong));
            if (query != null)
            {
                query.TenLoaiCong = tenloaicong;
                query.Ngay = DateOnly.Parse(ngay);
                query.HeSo = double.Parse(heso);
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Không tìm thấy đối tượng cần sửa");
            }
        }
    }
}
