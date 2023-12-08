using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiemDanhChamCong
{
    class XemChiTiet
    {
        public ChamCongContext db = new ChamCongContext();
        public class XemChiTietTT
        {
            public long Idcc { get; set; }

            public DateTime Vao { get; set; }

            public DateTime Ra { get; set; }

            public long? SoPhutMuon { get; set; }

            public long? SoPhutSom { get; set; }

            public string? MaNv { get; set; }

            public string LoaiCong { get; set; }

            public string LoaiCa{ get; set;}
            public string TenNv { get; set; }
        }
        public List<XemChiTietTT> LayDL( string manv , DateTime startdate, DateTime enddate)
        {
            var query = from t in db.Chamcongs
                        where t.MaNv == manv && startdate.Date <= t.Vao.Date && t.Vao.Date <= enddate.Date
                        select new XemChiTietTT
                        {
                            Idcc = t.Idcc,
                            Vao = t.Vao,
                            Ra = t.Ra,
                            SoPhutMuon = t.SoPhutMuon,
                            SoPhutSom = t.SoPhutSom,
                            MaNv = t.MaNv,
                            LoaiCong = (db.Loaicongs.SingleOrDefault(lc => lc.IdloaiCong == t.IdloaiCong) == null) ? "Không" : db.Loaicongs.SingleOrDefault(lc => lc.IdloaiCong == t.IdloaiCong).TenLoaiCong,
                            LoaiCa = (db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == t.IdloaiCa) == null) ? "Không" : db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == t.IdloaiCa).TenLoaiCa,
                            TenNv = db.Nhanviens.SingleOrDefault(ten => ten.MaNv == t.MaNv).HoTen
                        };
            return query.ToList<XemChiTietTT>();
        }
    }
}
