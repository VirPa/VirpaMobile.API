using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class VirpaMobileContext : DbContext
    {
        public VirpaMobileContext()
        {
        }

        public VirpaMobileContext(DbContextOptions<VirpaMobileContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserSessions> AspNetUserSessions { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<FeedBidders> FeedBidders { get; set; }
        public virtual DbSet<FeedMessages> FeedMessages { get; set; }
        public virtual DbSet<Feeds> Feeds { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Followers> Followers { get; set; }
        public virtual DbSet<ReferenceData> ReferenceData { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<UserSkills> UserSkills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=virpastaging.calkmizbqboe.ap-southeast-1.rds.amazonaws.com;Initial Catalog=VirpaMobileAPI.v1;Persist Security Info=True;User ID=virpaadmin;Password=VtLyHj5ytn88LVRZKlUmNEnCjjLibfF4ljcWSQFFT8");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BackgroundSummary).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Fullname).HasMaxLength(256);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserSessions>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApiVersion).HasMaxLength(50);

                entity.Property(e => e.AppVersion).HasMaxLength(50);

                entity.Property(e => e.DeviceId).HasMaxLength(450);

                entity.Property(e => e.DeviceName).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<FeedBidders>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.FeedId).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<FeedMessages>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BidderId).HasMaxLength(450);

                entity.Property(e => e.FeedId).HasMaxLength(450);

                entity.Property(e => e.FeederId).HasMaxLength(450);

                entity.Property(e => e.Turn).HasMaxLength(450);
            });

            modelBuilder.Entity<Feeds>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Budget).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CodeName).HasMaxLength(450);

                entity.Property(e => e.Extension).HasMaxLength(50);

                entity.Property(e => e.FeedId).HasMaxLength(450);

                entity.Property(e => e.FilePath).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<Followers>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.FollowedId).HasMaxLength(450);

                entity.Property(e => e.FollowerId).HasMaxLength(450);
            });

            modelBuilder.Entity<ReferenceData>(entity =>
            {
                entity.Property(e => e.RefId).HasMaxLength(450);

                entity.Property(e => e.RefKey).HasMaxLength(450);

                entity.Property(e => e.Value).HasMaxLength(450);
            });

            modelBuilder.Entity<Skills>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<UserSkills>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.UserId).HasMaxLength(450);
            });
        }
    }
}
