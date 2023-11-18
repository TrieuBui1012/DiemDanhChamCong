using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Nhanvien
{
    public string MaNv { get; set; } = null!;

    public string? HoTen { get; set; }

    public long? GioiTinh { get; set; }

    public DateTime NgaySinh { get; set; }

    public string? DienThoai { get; set; }

    public string? Cccd { get; set; }

    public string? DiaChi { get; set; }

    public long? Idluong { get; set; }

    public long? Idpb { get; set; }

    public virtual ICollection<Bangcong> Bangcongs { get; set; } = new List<Bangcong>();

    public virtual ICollection<Chamcong> Chamcongs { get; set; } = new List<Chamcong>();

    public virtual Luong? IdluongNavigation { get; set; }

    public virtual Phongban? IdpbNavigation { get; set; }
}
