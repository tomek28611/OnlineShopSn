using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Models.Db;

public partial class OnlineShopContext : DbContext
{
    public OnlineShopContext()
    {
    }

    public OnlineShopContext(DbContextOptions<OnlineShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductGalery> ProductGaleries { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OnlineShop;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.ToTable("Banner");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.SubTitle).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.FullDesc).HasMaxLength(4000);
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Tags).HasMaxLength(1000);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("TItle");
            entity.Property(e => e.VideoUrl).HasMaxLength(300);
        });

        modelBuilder.Entity<ProductGalery>(entity =>
        {
            entity.ToTable("ProductGalery");

            entity.Property(e => e.ImageName).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentText).HasMaxLength(1000);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.CopyRight)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.FaceBook)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.GooglePlus)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Instagram)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Twitter)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Youtube)
                .HasMaxLength(100)
                .IsFixedLength();
        });




        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<OnlineShop.Models.Db.Menus> Menus { get; set; } = default!;
}
