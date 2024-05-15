using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Reservico.Data.Entities;

namespace Reservico.Data
{
    public partial class ReservicoDbContext : DbContext
    {
        public ReservicoDbContext()
        {
        }

        public ReservicoDbContext(DbContextOptions<ReservicoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Changelog> Changelogs { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<IdentityAuthorizationCode> IdentityAuthorizationCodes { get; set; }
        public virtual DbSet<IdentityToken> IdentityTokens { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClient> UserClients { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Changelog>(entity =>
            {
                entity.ToTable("changelog");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Checksum)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("checksum");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.InstalledBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("installed_by");

                entity.Property(e => e.InstalledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("installed_on")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.Name, "NameIndex");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.Property(e => e.City).HasMaxLength(256);

                entity.Property(e => e.Country).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Postcode).HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityAuthorizationCode>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(456);

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<IdentityToken>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.RefreshToken)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "EmailIndex");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ClientId });

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.UserClients)
                    .HasForeignKey(d => d.ClientId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClients)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
