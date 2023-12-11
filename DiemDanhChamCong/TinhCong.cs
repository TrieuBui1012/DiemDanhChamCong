using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DiemDanhChamCong
{
    class TinhCong
    {
        ChamCongContext db = new();
        public List<TinhCongData> LayDL(long thang, long nam, long ngaynghi)
        {

            var query1 = from t in db.Chamcongs
                         where thang == t.Vao.Month && nam == t.Vao.Year
                         select new TinhCongData
                         {
                             MaNv = t.MaNv,
                             
                             IdCC = t.Idcc,
                             TongCong = t.IdloaiCong == null ? db.Loaicas.SingleOrDefault(x => x.IdloaiCa == t.IdloaiCa).HeSo : (db.Loaicas.SingleOrDefault(x => x.IdloaiCa == t.IdloaiCa).HeSo * db.Loaicongs.SingleOrDefault(x => x.IdloaiCong == t.IdloaiCong).HeSo),
                             SoGioMuon = t.SoPhutMuon / 60.0,
                             SoGioSom = t.SoPhutSom / 60.0,
                         };
            List<TinhCongData> TCD  = query1.ToList();
            var query2 = from t in TCD
                         group t by t.MaNv into NVGR
                         select new TinhCongData
                         {
                             MaNv = NVGR.Key,
                             HoTen = db.Nhanviens.SingleOrDefault(ten => ten.MaNv == NVGR.Key).HoTen,
                             SoGioMuon = NVGR.Sum(x => x.SoGioMuon),
                             SoGioSom = NVGR.Sum(x => x.SoGioSom),
                             TongCong = NVGR.Sum(x => x.TongCong),
                             MucLuong = db.Luongs.SingleOrDefault(x => x.Idluong == db.Nhanviens.SingleOrDefault(x => x.MaNv == NVGR.Key).Idluong).MucLuong,
                             TongLuong = (long?)(((double)(db.Luongs.SingleOrDefault(x => x.Idluong == db.Nhanviens.SingleOrDefault(x => x.MaNv == NVGR.Key).Idluong).MucLuong) / (double)(DateTime.DaysInMonth(Convert.ToInt32(nam), Convert.ToInt32(thang)) - ngaynghi)) * NVGR.Sum(x => x.TongCong)) - (long?)(((double)(db.Luongs.SingleOrDefault(x => x.Idluong == db.Nhanviens.SingleOrDefault(x => x.MaNv == NVGR.Key).Idluong).MucLuong) / (double)((DateTime.DaysInMonth(Convert.ToInt32(nam), Convert.ToInt32(thang)) - ngaynghi) * 8)) * (NVGR.Sum(x => x.SoGioMuon) + NVGR.Sum(x => x.SoGioSom)))
                             //TongLuong = ((long?)(((double)(db.Luongs.SingleOrDefault(x => x.Idluong == db.Nhanviens.SingleOrDefault(x => x.MaNv == NVGR.Key).Idluong).MucLuong) / (double)(DateTime.DaysInMonth(Convert.ToInt32(nam), Convert.ToInt32(thang)) - ngaynghi)) * NVGR.Sum(x => x.TongCong)) - (long?)(((double)(db.Luongs.SingleOrDefault(x => x.Idluong == db.Nhanviens.SingleOrDefault(x => x.MaNv == NVGR.Key).Idluong).MucLuong) / (double)((DateTime.DaysInMonth(Convert.ToInt32(nam), Convert.ToInt32(thang)) - ngaynghi) * 8)) * (NVGR.Sum(x => x.SoGioMuon) + NVGR.Sum(x => x.TongCong))) )
                         };
            return query2.ToList<TinhCongData>();
        }

        public void Luu(long thang, long nam, long ngaynghi)
        {
            int dem = 0;
            foreach (var t in LayDL(thang, nam, ngaynghi))
            {
                Bangcong bc = new Bangcong();
                if (db.Bangcongs.FirstOrDefault(x => x.Nam == nam && x.Thang == thang && x.MaNv == t.MaNv) == null)
                {
                    bc.Thang = thang;
                    bc.Nam = nam;
                    bc.SoGioMuon = t.SoGioMuon;
                    bc.SoGioSom = t.SoGioSom;
                    bc.TongCong = t.TongCong;
                    bc.TongLuong = t.TongLuong;
                    bc.MaNv = t.MaNv;

                    db.Bangcongs.Add(bc);
                    db.SaveChanges();
                    dem++;
                }
            }
            if(dem > 0)
            {
                MessageBox.Show("Đã lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Đã lưu trước đó!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public List<TinhCongData> DLBC()
        {

            var query = from b in db.Bangcongs
                        select new TinhCongData
                        {
                            MaNv = b.MaNv,
                            HoTen = db.Nhanviens.SingleOrDefault(ten => ten.MaNv == b.MaNv).HoTen,
                            SoGioMuon = b.SoGioMuon,
                            SoGioSom = b.SoGioSom,
                            TongCong = b.TongCong,
                            TongLuong = b.TongLuong,
                        };
            List<TinhCongData> tcd = query.ToList();
            //MessageBox.Show($"{tcd.Count()}", "Thông báo", MessageBoxButton.OK);
            return query.ToList();
        }
    }
}
