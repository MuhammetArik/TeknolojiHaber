using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeknolojiHaber.Models;

namespace TeknolojiHaber.Data
{
    public class TeknolojiHaberContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public TeknolojiHaberContext (DbContextOptions<TeknolojiHaberContext> options)
            : base(options)
        {
        }

        public DbSet<Haber> Haberler { get; set; }

        public DbSet<Resim> Resimler { get; set; }

        public DbSet<Kategori> Kategoriler { get; set; }

        public DbSet<KategoriHaber> KategorilerVeHaberleri { get; set; }
       // public DbSet<KategoriHaber> KategorilerVeHaberler { get; set; }   //dotnet 3.1                                  

       /* protected override void OnModelCreating(ModelBuilder modelBuilder)  //dotnet 3.1
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<KategoriHaber>()
            .HasKey(t=>new{t.HaberId, t.KategoriId});
        } */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Haber>()
            .HasMany(x=>x.Kategorileri)
            .WithMany(x=>x.Haberleri)
            .UsingEntity<KategoriHaber>(
                j=> j.HasOne(x=>x.Kategori).WithMany(x=>x.KategoriHaberler).HasForeignKey(x=>x.KategoriId).OnDelete(DeleteBehavior.Restrict),
                j=> j.HasOne(x=>x.Haber).WithMany(x=>x.KategoriHaberler).HasForeignKey(x=>x.HaberId).OnDelete(DeleteBehavior.Cascade),
                j=> {
                    j.HasKey(x=> new {x.HaberId,x.KategoriId});
                    j.Property(x=>x.EklenmeTarihi).HasDefaultValueSql("CURRENT_TIMESTAMP");
                } 
            );
            base.OnModelCreating(modelBuilder);
        } 
    }
}
