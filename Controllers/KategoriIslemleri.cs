using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknolojiHaber.Data;
using TeknolojiHaber.Models;

namespace TeknolojiHaber.Controllers
{
    [Authorize(Roles="Admin,Editor")]

    public class KategoriIslemleri : Controller
    {

        private readonly TeknolojiHaberContext _context;

        private readonly UserManager<AppUser> _userManager;

         public KategoriIslemleri(TeknolojiHaberContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: KategoriIslemleri
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategoriler.ToListAsync());
        }

        // GET: KategoriIslemleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategoriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        [Authorize(Roles="Admin")]
        // GET: KategoriIslemleri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KategoriIslemleri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KategoriAdi,Aciklama")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategori);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["basarilimesaj"]=$"<{kategori.KategoriAdi}> Ekleme İşlemi Başarılı.";
                }
                catch (System.Exception)
                {
                    
                    TempData["basarisizmesaj"]=$"<{kategori.KategoriAdi}> Ekleme İşlemi Başarısız Oldu.";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // GET: KategoriIslemleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }
            return View(kategori);
        }

        // POST: KategoriIslemleri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KategoriAdi,Aciklama")] Kategori kategori)
        {
            if (id != kategori.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategori);
                    TempData["basarilimesaj"]=$"<{kategori.KategoriAdi}> Güncelleme İşlemi Başarılı.";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["basarisizmesaj"]=$"<{kategori.KategoriAdi}> Güncelleme İşlemi Başarısız Oldu.";
                    if (!KategoriExists(kategori.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // GET: KategoriIslemleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategoriler.Include(x=>x.Haberleri)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        // POST: KategoriIslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, IFormCollection elemanlar)
        {
            var kategori = await _context.Kategoriler
            .Include(x=>x.KategoriHaberler)
            .Include(x=>x.Haberleri)
            .SingleOrDefaultAsync(x=>x.Id==id);
            
            var haberlerSilinsin = elemanlar["cbHaberlerSilinsin"]=="on";

            if(haberlerSilinsin) _context.RemoveRange(kategori.Haberleri);

            _context.RemoveRange(kategori.KategoriHaberler);

            _context.Kategoriler.Remove(kategori);

                await _context.SaveChangesAsync();
                var haberMesaj = haberlerSilinsin?"Ve İçerisindeki "+kategori.Haberleri.Count+ " Adet Haber ":"";
                TempData["basarilimesaj"]=$"<{kategori.KategoriAdi}> Kategorisi {haberMesaj} Başarıyla Silindi.";


       
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriExists(int id)
        {
            return _context.Kategoriler.Any(e => e.Id == id);
        }
        [AllowAnonymous]
         public IActionResult KategoriYonetimiSPA()
        {
            return View();
        }
    }
}
