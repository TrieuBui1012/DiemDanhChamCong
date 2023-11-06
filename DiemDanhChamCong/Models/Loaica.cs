using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Loaica
{
    public long IdloaiCa { get; set; }

    public string? TenLoaiCa { get; set; }

    public byte[]? Vao { get; set; }

    public byte[]? Ra { get; set; }

    public long? HeSo { get; set; }

    public virtual ICollection<Chamcong> Chamcongs { get; set; } = new List<Chamcong>();
}
