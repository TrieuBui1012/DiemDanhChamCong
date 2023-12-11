using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiemDanhChamCong
{
    public class QuanLyBangCong
    {
        ChamCongContext db = new ChamCongContext();
        public class QuanLyBangCongData
        {
            public long IDBC {  get; set; }
            public string MaNV { get; set; }
            public string TenNV { get; set; }
            public long? Thang { get; set; }
            public long? Nam { get; set; }
            public double? Muon { get; set; }
            public double? Som { get; set; }
            public double? Cong { get; set; }
            public long? Luong { get; set; }
        }
        public List<QuanLyBangCongData> LayDLBangCong(long? thang, long nam, string? manv)
        {
            var query = from t in db.Bangcongs
                        where((thang == null || t.Thang == thang) && (t.Nam == nam) && (manv == null || manv.Length == 0 || t.MaNv.Equals(manv)))
                        select new QuanLyBangCongData
                        {
                            IDBC = t.Idbc,
                            MaNV = t.MaNv,
                            TenNV = db.Nhanviens.SingleOrDefault(x => x.MaNv == t.MaNv).HoTen,
                            Thang = t.Thang,
                            Nam = t.Nam,
                            Muon = t.SoGioMuon,
                            Som = t.SoGioSom,
                            Cong = t.TongCong,
                            Luong = t.TongLuong
                        };
            return query.ToList<QuanLyBangCongData>();
        }
        public void Sua(long idbc, double? muon, double? som, double? cong, long? luong)
        {
            Bangcong? b = db.Bangcongs.SingleOrDefault(x => x.Idbc == idbc);
            if(b != null)
            {
                b.SoGioMuon = muon;
                b.SoGioSom = som;
                b.TongCong = cong;
                b.TongLuong = luong;
                db.SaveChanges();
            }
        }
        public void Xoa(long idbc)
        {
            Bangcong? b = db.Bangcongs.SingleOrDefault(x => x.Idbc == idbc);
            if (b != null)
            {
                db.Bangcongs.Remove(b);
                db.SaveChanges();
            }
        }
    }
}
