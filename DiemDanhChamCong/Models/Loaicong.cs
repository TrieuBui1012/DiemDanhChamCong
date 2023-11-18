using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Loaicong
{
    public long IdloaiCong { get; set; }

    public string? TenLoaiCong { get; set; }

    public DateOnly Ngay { get; set; }

    public double? HeSo { get; set; }

    public virtual ICollection<Chamcong> Chamcongs { get; set; } = new List<Chamcong>();
}
