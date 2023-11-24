using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Bangcong
{
    public long Idbc { get; set; }

    public string? TenBc { get; set; }

    public long? Thang { get; set; }

    public long? Nam { get; set; }

    public double? SoGioSom { get; set; }

    public double? SoGioMuon { get; set; }

    public double? TongCong { get; set; }

    public long? TongLuong { get; set; }

    public string? MaNv { get; set; }

    public virtual Nhanvien? MaNvNavigation { get; set; }
}
