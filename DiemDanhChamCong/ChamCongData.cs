using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZXing;
using ZXing.Maxicode;
using ZXing.Windows.Compatibility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DiemDanhChamCong
{
    class ChamCongData
    {
        public ChamCongContext db = new ChamCongContext();
        public class ChamCongDataTT
        {
            public long Idcc { get; set; }

            public DateTime Vao { get; set; }

            public DateTime Ra { get; set; }

            public long? SoPhutMuon { get; set; }

            public long? SoPhutSom { get; set; }

            public string? MaNv { get; set; }

            public string LoaiCong { get; set; }

            public string LoaiCa { get; set; }
        }
        public ChamCongData() { }

        public List<ChamCongDataTT> LayDL(long? IDLoaiCa)
        {
            var query = from t in db.Chamcongs
                        where (t.Vao.Date == DateTime.Now.Date) && (t.IdloaiCa == IDLoaiCa)
                        select new ChamCongDataTT
                        {
                            Idcc = t.Idcc,
                            Vao = t.Vao,
                            Ra = t.Ra,
                            SoPhutMuon = t.SoPhutMuon,
                            SoPhutSom = t.SoPhutSom,
                            MaNv = t.MaNv,
                            LoaiCong = (db.Loaicongs.SingleOrDefault(lc => lc.IdloaiCong == t.IdloaiCong) == null) ? "Không" : db.Loaicongs.SingleOrDefault(lc => lc.IdloaiCong == t.IdloaiCong).TenLoaiCong,
                            LoaiCa = (db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == t.IdloaiCa) == null) ? "Không" : db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == t.IdloaiCa).TenLoaiCa
                        };
            return query.ToList<ChamCongDataTT>();
        }

        public static Dictionary<string, DateTime> ScanQRImages(string directoryPath)
        {
            Dictionary<string, DateTime> qrCodesData = new Dictionary<string, DateTime>();

            if (Directory.Exists(directoryPath))
            {
                string[] imageFiles = Directory.GetFiles(directoryPath);

                foreach (var imageFile in imageFiles)
                {
                    try
                    {
                        using (Bitmap bitmap = new Bitmap(imageFile))
                        {
                            BarcodeReader barcodeReader = new BarcodeReader();
                            var result = barcodeReader.Decode(bitmap);

                            if (result != null)
                            {
                                string qrText = result.Text;
                                DateTime creationTime = File.GetCreationTime(imageFile);
                                qrCodesData.Add(qrText, creationTime);
                            }
                        }

                        File.Delete(imageFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi quét mã QR từ file {imageFile}: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show($"Đường dẫn chấm công {directoryPath} không tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return qrCodesData;
        }

        public TimeOnly LayGioRaLoaiCa(long? idLoaiCa)
        {
            var query = db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == idLoaiCa);
            return query.Ra;
        }

        public TimeOnly LayGioVaoLoaiCa(long? idLoaiCa)
        {
            var query = db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == idLoaiCa);
            return query.Vao;
        }

        public void ChamCongVao(string maNV, DateTime gioVao, long? idLoaiCa)
        {
            try
            {
                var query = db.Chamcongs.SingleOrDefault(t => ((idLoaiCa == t.IdloaiCa) && (maNV.Equals(t.MaNv)) && (gioVao.Date == t.Vao.Date)) );
                
                if (query == null)
                {
                    Chamcong chamcong = new Chamcong();
                    
                    chamcong.Vao = gioVao;
                    TimeOnly timeVaoCa = LayGioVaoLoaiCa(idLoaiCa);
                    TimeOnly timeRaCa = LayGioRaLoaiCa(idLoaiCa);
                    DateTime ra = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timeRaCa.Hour, timeRaCa.Minute, timeRaCa.Second);
                    chamcong.Ra = ra;
                    TimeOnly timeVao = new TimeOnly(gioVao.Hour, gioVao.Minute, gioVao.Second);
                    TimeSpan timeMuon = timeVao - timeVaoCa;

                    if (timeVao > timeVaoCa)
                    {
                        chamcong.SoPhutMuon = (long?)timeMuon.TotalMinutes;
                    }
                    else
                    {
                        chamcong.SoPhutMuon = 0;
                    }
                    chamcong.SoPhutSom = 0;
                    chamcong.MaNv = maNV;
                    DateOnly dateCur = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var cong = db.Loaicongs.SingleOrDefault(lc => lc.Ngay == dateCur);
                    if (cong == null)
                    {
                        chamcong.IdloaiCong = null;
                    }
                    else
                    {
                        chamcong.IdloaiCong = cong.IdloaiCong;
                    }
                    chamcong.IdloaiCa = idLoaiCa;
                    db.Chamcongs.Add(chamcong);
                    db.SaveChanges();

                }
                
            }
            catch
            {
                MessageBox.Show("Có lỗi khi chấm công vào!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public void ChamCongRa(string maNV, DateTime gioRa, long? idLoaiCa)
        {
            try
            {
                var query = db.Chamcongs.SingleOrDefault(t => ((idLoaiCa == t.IdloaiCa) && (maNV.Equals(t.MaNv)) && (gioRa.Date == t.Ra.Date)));
                
                if (query == null)
                {
                    MessageBox.Show($"Nhân viên có mã {maNV} không chấm công vào nhưng lại chấm công ra!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    query.Ra = gioRa;
                    TimeOnly timeRaCa = LayGioRaLoaiCa(idLoaiCa);
                    TimeOnly timeRa = new TimeOnly(gioRa.Hour, gioRa.Minute, gioRa.Second);
                    TimeSpan timeSom = timeRaCa - timeRa;
                    if (timeRa < timeRaCa)
                    {
                        query.SoPhutSom = (long?)timeSom.TotalMinutes;
                    }
                    db.SaveChanges();
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi khi chấm công ra!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public static List<ChamCongDataTT> dgvChamCongLSCC_Changed_Source(DateTime? ngay, long? idCa, string maNV, string strMaCC)
        {
            long? maCC = null;
            try
            {
                maCC = Convert.ToInt64(strMaCC);
            }
            catch
            {

            }

            bool isNotNullNgay = (ngay != null);
            bool isNotNullCa = (idCa != null);
            bool isNotNullMaNV = (maNV.Length != 0);
            bool isNotNullMaCC = (maCC != null);

            ChamCongContext db = new ChamCongContext();
            var query = from t in db.Chamcongs
                        where (!isNotNullNgay || (ngay == t.Vao.Date)) && (!isNotNullCa || (idCa == t.IdloaiCa)) && (!isNotNullMaNV || (t.MaNv.Contains(maNV))) && (!isNotNullMaCC || (maCC.Equals(t.Idcc)))
                        select new ChamCongDataTT
                        {
                            Idcc = t.Idcc,
                            Vao = t.Vao,
                            Ra = t.Ra,
                            SoPhutMuon = t.SoPhutMuon,
                            SoPhutSom = t.SoPhutSom,
                            MaNv = t.MaNv,
                            LoaiCong = (db.Loaicongs.SingleOrDefault(lc => lc.IdloaiCong == t.IdloaiCong) == null) ? "Không" : db.Loaicongs.SingleOrDefault(lc => lc.IdloaiCong == t.IdloaiCong).TenLoaiCong,
                            LoaiCa = (db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == t.IdloaiCa) == null) ? "Không" : db.Loaicas.SingleOrDefault(lc => lc.IdloaiCa == t.IdloaiCa).TenLoaiCa
                        };
            return query.ToList<ChamCongDataTT>();
        }
    }
}
