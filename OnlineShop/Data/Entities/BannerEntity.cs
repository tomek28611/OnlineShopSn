using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;
using OnlineShop.Data.Dto;

namespace OnlineShop.Data.Entities;

public partial class BannerEntity
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? SubTitle { get; set; }

    public string? ImageName { get; set; }

    public short? Priority { get; set; }

    public string? Link { get; set; }

    public string? Position { get; set; }

    public BannerDto ToDto()
    {
        return new BannerDto { Id = Id, Title = Title, SubTitle = SubTitle, ImageName = ImageName, Priority = Priority, Link = Link };
    }
}

public class BannerEntityConfiguration : IEntityTypeConfiguration<BannerEntity>
{
    public void Configure(EntityTypeBuilder<BannerEntity> builder)
    {

        builder.ToTable("Banner");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.ImageName).HasMaxLength(50);
        builder.Property(e => e.Link).HasMaxLength(100);
        builder.Property(e => e.Position).HasMaxLength(50);
        builder.Property(e => e.SubTitle).HasMaxLength(1000);
        builder.Property(e => e.Title).HasMaxLength(200);
    }
}

