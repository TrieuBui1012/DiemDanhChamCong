using DiemDanhChamCong.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZXing;
using ZXing.Maxicode;
using ZXing.Windows.Compatibility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace DiemDanhChamCong
{
    class ThongKeData
    {
            public double? LateHours { get; set; }
            public double? EarlyHours { get; set; }

            public string? MaNV { get; set; }
            public string? TenNV { get; set; }

    }
    

}
