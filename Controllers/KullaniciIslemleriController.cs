using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknolojiHaber.Models;
using TeknolojiHaber.Data;


namespace TeknolojiHaber.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciIslemleriController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly TeknolojiHaberContext context;
        private readonly RoleManager<AppRole> roleManager;
        public KullaniciIslemleriController(UserManager<AppUser> userManager,TeknolojiHaberContext context, RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
            this.context = context;
            this.userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            var kullanicilar = await userManager.Users.ToListAsync();
            return View(kullanicilar);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(KullaniciViewModel kullanici)
        {
            try
            {
                await userManager.CreateAsync(new AppUser { UserName = kullanici.Eposta, Email = kullanici.Eposta, EmailConfirmed = true }, kullanici.Sifre);
                TempData["basarilimesaj"] = "Kullanıcı Ekleme İşlemi Başarılı.";
            }
            catch (System.Exception)
            {

                TempData["basarisizmesaj"] = "Kullanıcı Ekleme İşlemi Başarısız Oldu.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
                TempData["basarilimesaj"] = "Kullanıcı Silme İşlemi Başarılı.";
            }
            catch (System.Exception)
            {
                TempData["basarisizmesaj"] = "Kullanıcı Silme İşlemi Başarısız Oldu.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> KullanicininRolleri(string id)
        {
            var kullanici = await userManager.FindByIdAsync(id);
            var kullanicininRolleri = await userManager.GetRolesAsync(kullanici);
            ViewBag.KullaniciAdi = kullanici.UserName;
            return View(kullanicininRolleri);
        }

        [HttpPost]
        public async Task<IActionResult> KullanicininRolleri(string id, IFormCollection elemanlar)
        {
            var kullanici = await userManager.FindByIdAsync(id);

            var seciliRoller = elemanlar["SeciliRoller"]; //örneğin admin,manager


            await userManager.RemoveFromRolesAsync(kullanici, await userManager.GetRolesAsync(kullanici));
            await userManager.AddToRolesAsync(kullanici, seciliRoller.ToList());
            await context.SaveChangesAsync();

            TempData["basarilimesaj"] = $"{kullanici.UserName} Kullanıcısının Rolleri Başarıyla Ayarlandı";

            return RedirectToAction(nameof(Index));
        }

        }
    }