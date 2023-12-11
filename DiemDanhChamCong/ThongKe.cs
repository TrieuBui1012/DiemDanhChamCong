using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiemDanhChamCong
{
    class ThongKe
    {
        ChamCongContext db = new ChamCongContext();
        public List<ThongKeData> LayDL(DateTime startdate, DateTime enddate)
        {
            var query1 = from t in db.Chamcongs 
                         where startdate.Date <= t.Vao.Date && t.Vao.Date <= enddate.Date
                         group t by t.MaNv into NVGR
                         select new ThongKeData
                         {
                             MaNV = NVGR.Key,
                             TenNV = db.Nhanviens.SingleOrDefault(ten => ten.MaNv == NVGR.Key).HoTen,
                             LateHours = NVGR.Sum(x => (double?)(x.SoPhutMuon/60.0)),
                             EarlyHours = NVGR.Sum(x => (double?)(x.SoPhutSom/60.0)),
                         };
         return query1.ToList<ThongKeData>() ;              
        }
    }
}
