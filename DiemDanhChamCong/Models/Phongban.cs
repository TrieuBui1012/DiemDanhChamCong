using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Phongban
{
    public long Idpb { get; set; }

    public string? TenPb { get; set; }

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
