using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DiemDanhChamCong.Models;

public partial class ChamCongContext : DbContext
{
    public ChamCongContext()
    {
    }

    public ChamCongContext(DbContextOptions<ChamCongContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bangcong> Bangcongs { get; set; }

    public virtual DbSet<Chamcong> Chamcongs { get; set; }

    public virtual DbSet<Loaica> Loaicas { get; set; }

    public virtual DbSet<Loaicong> Loaicongs { get; set; }

    public virtual DbSet<Luong> Luongs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Phongban> Phongbans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=.\\Database\\ChamCong.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bangcong>(entity =>
        {
            entity.HasKey(e => e.Idbc);

            entity.ToTable("BANGCONG");

            entity.Property(e => e.Idbc).HasColumnName("IDBC");
            entity.Property(e => e.MaNv)
                .HasColumnType("CHAR(8)")
                .HasColumnName("MaNV");
            entity.Property(e => e.TenBc).HasColumnName("TenBC");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Bangcongs).HasForeignKey(d => d.MaNv);
        });

        modelBuilder.Entity<Chamcong>(entity =>
        {
            entity.HasKey(e => e.Idcc);

            entity.ToTable("CHAMCONG");

            entity.Property(e => e.Idcc).HasColumnName("IDCC");
            entity.Property(e => e.IdloaiCa).HasColumnName("IDLoaiCa");
            entity.Property(e => e.IdloaiCong).HasColumnName("IDLoaiCong");
            entity.Property(e => e.MaNv)
                .HasColumnType("CHAR(8)")
                .HasColumnName("MaNV");
            entity.Property(e => e.Ra).HasColumnType("DATETIME");
            entity.Property(e => e.Vao).HasColumnType("DATETIME");

            entity.HasOne(d => d.IdloaiCaNavigation).WithMany(p => p.Chamcongs).HasForeignKey(d => d.IdloaiCa);

            entity.HasOne(d => d.IdloaiCongNavigation).WithMany(p => p.Chamcongs).HasForeignKey(d => d.IdloaiCong);

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Chamcongs).HasForeignKey(d => d.MaNv);
        });

        modelBuilder.Entity<Loaica>(entity =>
        {
            entity.HasKey(e => e.IdloaiCa);

            entity.ToTable("LOAICA");

            entity.Property(e => e.IdloaiCa).HasColumnName("IDLoaiCa");
            entity.Property(e => e.Ra).HasColumnType("DATETIME");
            entity.Property(e => e.Vao).HasColumnType("DATETIME");
        });

        modelBuilder.Entity<Loaicong>(entity =>
        {
            entity.HasKey(e => e.IdloaiCong);

            entity.ToTable("LOAICONG");

            entity.Property(e => e.IdloaiCong).HasColumnName("IDLoaiCong");
            entity.Property(e => e.Ngay).HasColumnType("DATE");
        });

        modelBuilder.Entity<Luong>(entity =>
        {
            entity.HasKey(e => e.Idluong);

            entity.ToTable("LUONG");

            entity.Property(e => e.Idluong).HasColumnName("IDLuong");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.MaNv);

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.MaNv)
                .HasColumnType("CHAR(8)")
                .HasColumnName("MaNV");
            entity.Property(e => e.Cccd)
                .HasColumnType("CHAR(12)")
                .HasColumnName("CCCD");
            entity.Property(e => e.DienThoai).HasColumnType("CHAR(10)");
            entity.Property(e => e.Idluong).HasColumnName("IDLuong");
            entity.Property(e => e.Idpb).HasColumnName("IDPB");
            entity.Property(e => e.NgaySinh).HasColumnType("DATE");

            entity.HasOne(d => d.IdluongNavigation).WithMany(p => p.Nhanviens).HasForeignKey(d => d.Idluong);

            entity.HasOne(d => d.IdpbNavigation).WithMany(p => p.Nhanviens).HasForeignKey(d => d.Idpb);
        });

        modelBuilder.Entity<Phongban>(entity =>
        {
            entity.HasKey(e => e.Idpb);

            entity.ToTable("PHONGBAN");

            entity.Property(e => e.Idpb).HasColumnName("IDPB");
            entity.Property(e => e.TenPb).HasColumnName("TenPB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
