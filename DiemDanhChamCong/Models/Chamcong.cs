using System;
using System.Collections.Generic;

namespace DiemDanhChamCong.Models;

public partial class Chamcong
{
    public long Idcc { get; set; }

    public DateTime Vao { get; set; }

    public DateTime Ra { get; set; }

    public long? SoPhutMuon { get; set; }

    public long? SoPhutSom {  get; set; }

    public string? MaNv { get; set; }

    public long? IdloaiCong { get; set; }

    public long? IdloaiCa { get; set; }

    public virtual Loaica? IdloaiCaNavigation { get; set; }

    public virtual Loaicong? IdloaiCongNavigation { get; set; }

    public virtual Nhanvien? MaNvNavigation { get; set; }
}
