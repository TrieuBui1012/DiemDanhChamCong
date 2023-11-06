using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Luong
{
    public long Idluong { get; set; }

    public string? TenLuong { get; set; }

    public long? MucLuong { get; set; }

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
