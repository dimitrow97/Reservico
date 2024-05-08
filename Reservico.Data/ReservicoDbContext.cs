using Microsoft.EntityFrameworkCore;
using Reservico.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<IdentityToken> IdentityTokens { get; set; }
        public virtual DbSet<IdentityOneTimeCode> IdentityOneTimeCodes { get; set; }
        public virtual DbSet<IdentityAuthorizationCode> IdentityAuthorizationCodes { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserClient> UserClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.HasKey(uc => new { uc.UserId, uc.ClientId });

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.UserClients)
                    .HasForeignKey(d => d.ClientId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClients)
                    .HasForeignKey(d => d.UserId);
            });
        }
    }
}