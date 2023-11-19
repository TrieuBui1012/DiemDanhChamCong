using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool CheckDL(string idloaica, string tenloaica, string vao, string ra, string heso)
        {
            string mess = "";
            if (idloaica == "" || tenloaica == "" || vao == "" || ra == "" || heso == "")
            {
                mess += "\nVui lòng nhập đầy đủ dữ liệu";
            }
            if (!Regex.IsMatch(idloaica, @"\d+"))
            {
                mess += "\nMã loại ca phải là số nguyên";
            }

            if (mess != "")
            {
                MessageBox.Show(mess, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public void addLoaiCa(Loaica c)
        {
            var query = db.Loaicas.SingleOrDefault(t => t.IdloaiCa.Equals(c.IdloaiCa));
            if(query != null) 
            {
                MessageBox.Show("Đã tồn tại", "Thông báo", MessageBoxButton.OK);
            }
            else
            { 
                db.Loaicas.Add(c);
                db.SaveChanges();
            }
        }

        public void deleteLoaiCa(string idloaica)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi xóa: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void changeLoaiCa(string idloaica, string tenloaica, string giovao, string giora, string heso)
        {
            try
            { 
                var query = db.Loaicas.SingleOrDefault(t => t.IdloaiCa == long.Parse(idloaica));
                if (query != null)
                {
                    if(CheckDL(idloaica, tenloaica, giovao, giora, heso))
                    {
                        query.TenLoaiCa = tenloaica;
                        query.Vao = TimeOnly.Parse(giovao);
                        query.Ra = TimeOnly.Parse(giora);
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
