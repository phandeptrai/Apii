using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WebApplication3.Models;

public partial class PostifyContext : DbContext
{
    public PostifyContext()
    {
    }

    public PostifyContext(DbContextOptions<PostifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=Postify;user=root;sslmode=None;allowpublickeyretrieval=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => new { e.FollowingUserId, e.FollowedUserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("follows");

            entity.HasIndex(e => e.FollowedUserId, "followed_user_id");

            entity.Property(e => e.FollowingUserId)
                .HasColumnType("int(11)")
                .HasColumnName("following_user_id");

            entity.Property(e => e.FollowedUserId)
                .HasColumnType("int(11)")
                .HasColumnName("followed_user_id");

            entity.Property(e => e.CreatedAt)
                .HasMaxLength(255)
                .HasColumnName("created_at");

            // Cấu hình lại đúng chiều
            entity.HasOne(d => d.FollowingUser)
                .WithMany(p => p.Following)  // người này đang follow ai
                .HasForeignKey(d => d.FollowingUserId)
                .HasConstraintName("follows_ibfk_1");

            entity.HasOne(d => d.FollowedUser)
                .WithMany(p => p.Followers)  // những người đang follow người này
                .HasForeignKey(d => d.FollowedUserId)
                .HasConstraintName("follows_ibfk_2");
        });


        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Body)
                .HasColumnType("text")
                .HasColumnName("body");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(255)
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("posts_ibfk_1");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.IdProvince).HasName("PRIMARY");

            entity.ToTable("provinces");

            entity.Property(e => e.IdProvince)
                .HasColumnType("int(11)")
                .HasColumnName("idProvince");
            entity.Property(e => e.NameProvince)
                .HasMaxLength(255)
                .HasColumnName("nameProvince");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.ProvinceId, "fk_province");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Birthday)
                .HasMaxLength(6)
                .HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(255)
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.ProvinceId)
                .HasColumnType("int(11)")
                .HasColumnName("province_id");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Province).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("fk_province");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
