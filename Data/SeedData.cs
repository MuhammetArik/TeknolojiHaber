using TeknolojiHaber.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace TeknolojiHaber.Data
{
    public class SeedData
    {

        public async static void Initialize(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Çekirdek_Veriler_Yazılıyor...");
            using (var context = new TeknolojiHaberContext(serviceProvider.GetRequiredService<DbContextOptions<TeknolojiHaberContext>>()))
            {
                context.Database.Migrate();
                if (context.Haberler.Any())
                {
                    return;
                }

                var roleManager=serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                await roleManager.CreateAsync(new AppRole{Name="Admin"});
                await roleManager.CreateAsync(new AppRole{Name="Editor"});
                await roleManager.CreateAsync(new AppRole{Name="User"});

                var userManager=serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var admin=new AppUser{ UserName="admin@gmail.com", Email="admin@gmail.com",EmailConfirmed=true};
                var editor=new AppUser{ UserName="editor@gmail.com", Email="editor@gmail.com",EmailConfirmed=true};
                var user=new AppUser{ UserName="user@gmail.com", Email="user@gmail.com",EmailConfirmed=true};
                var test=new AppUser{ UserName="test@gmail.com", Email="test@gmail.com",EmailConfirmed=true};

                await userManager.CreateAsync(admin,"Admin123");
                await userManager.CreateAsync(editor,"Editor123");
                await userManager.CreateAsync(user,"User123");
                await userManager.CreateAsync(test,"Test123");

                await userManager.AddToRolesAsync(admin, new List<string>{"Admin","Editor"});
                await userManager.AddToRoleAsync(editor,"Editor");
                await userManager.AddToRoleAsync(user,"User");
                await userManager.AddToRoleAsync(test,"Admin");

                var haberler=new Haber[]
                {
                    new Haber
                    {
                        HaberBaslik = "İntel Ekran Kartı",
                        HaberBilgi = "Rakipleri Karşısında İyi Olacakmı",
                        HaberTarih = DateTime.Parse("09-05-2021"),
                        IlgiDuzeyi = Convert.ToDecimal(9.9),
                        Resimler = new List<Resim>
                            {
                                new Resim{DosyaAdi="resim2.jpg"}
                            },
                    },
                    new Haber
                    {
                        HaberBaslik = "PlayStation 5",
                        HaberBilgi = "Oyunlarının Türkiye Fiyatları Belli Oldu",
                        HaberTarih = DateTime.Parse("09-05-2021"),
                        IlgiDuzeyi = Convert.ToDecimal(9.9),
                        Resimler = new List<Resim>()
                            {
                                new Resim{DosyaAdi="resim3.jpg"},
                                new Resim{DosyaAdi="resim4.jpg"}
                            },
                    }
                };

                context.Haberler.AddRange(haberler); 
                var kategoriler = new Kategori[] 
                { 
                    new Kategori {KategoriAdi= "Ocak"},
                    new Kategori {KategoriAdi= "Şubat"},
                    new Kategori {KategoriAdi= "Mart"},
                    new Kategori {KategoriAdi= "Nisan"},
                    new Kategori {KategoriAdi= "Mayıs"},
                    new Kategori {KategoriAdi= "Haziran"},
                    new Kategori {KategoriAdi= "Temmuz"}, 
                    new Kategori {KategoriAdi= "Ağustos"}, 
                    new Kategori {KategoriAdi= "Eylül"},
                    new Kategori {KategoriAdi= "Ekim"},
                    new Kategori {KategoriAdi= "Kasım"}, 
                    new Kategori {KategoriAdi= "Aralık"},
                }; 

                context.Kategoriler.AddRange(kategoriler); 
                var kategorilerVeHaberleri = new KategoriHaber[] 
                { 
                    new KategoriHaber{Haber= haberler[0], Kategori = kategoriler[10]},
                }; 
                context.KategorilerVeHaberleri.AddRange(kategorilerVeHaberleri); 


                context.SaveChanges();
                
            }
        }
    }
}


