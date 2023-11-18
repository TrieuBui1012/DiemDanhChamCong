using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Loaica
{
    public long IdloaiCa { get; set; }

    public string? TenLoaiCa { get; set; }

    public TimeOnly Vao { get; set; }

    public TimeOnly Ra { get; set; }

    public double? HeSo { get; set; }

    public virtual ICollection<Chamcong> Chamcongs { get; set; } = new List<Chamcong>();
}
