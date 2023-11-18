using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiemDanhChamCong.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DiemDanhChamCong
{
    class BaoTriLoaiCa
    {
        ChamCongContext db = new();
        public List<Loaica> LayDL()
        {
            var query = from t in db.Loaicas
                        select new Loaica
                        {
                            IdloaiCa = t.IdloaiCa,
                            TenLoaiCa = t.TenLoaiCa,
                            Vao = t.Vao,
                            Ra = t.Ra,
                            HeSo = t.HeSo,
                        };
            return query.ToList<Loaica>();
        }

        public void addLoaiCa(Loaica c)
        {
            db.Loaicas.Add(c);
            db.SaveChanges();
        }

        public void deleteLoaiCa(string idloaica)
        {
            var query = db.Loaicas.SingleOrDefault(t => t.IdloaiCa.Equals(long.Parse(idloaica)));
            if (query != null)
            {
                MessageBoxResult rs = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNoCancel);
                if (rs == MessageBoxResult.Yes)
                {
                    db.Loaicas.Remove(query);
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy!", "Thông báo");
            }
        }

        public void changeLoaiCa(string idloaica, string tenloaica, string giovao, string giora, string heso)
        {
            var query = db.Loaicas.SingleOrDefault(t => t.IdloaiCa == long.Parse(idloaica));
            if (query != null)
            {
                query.TenLoaiCa = tenloaica;
                query.Vao = TimeOnly.Parse(giovao);
                query.Ra = TimeOnly.Parse(giora);
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
