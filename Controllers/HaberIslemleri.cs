using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeknolojiHaber.Data;
using TeknolojiHaber.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace TeknolojiHaber.Controllers
{
    public class HaberIslemleri : Controller
    {
        private readonly TeknolojiHaberContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private string _dosyaYolu;

        public HaberIslemleri(TeknolojiHaberContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _dosyaYolu=Path.Combine(_hostEnvironment.WebRootPath,"resimler");
        if (!Directory.Exists(_dosyaYolu))
        {
            Directory.CreateDirectory(_dosyaYolu);
        }
        }

        

        // GET: HaberIslemleri
        public async Task<IActionResult> Index()
        {
            ViewBag.KategoriAdi="Tüm Kategoriler";
            return View(await _context.Haberler.Include(x=>x.Resimler).ToListAsync());
        }

        public async Task<IActionResult> KategorininHaberleri (int? id)
        {
            var kategori = await _context.Kategoriler
            .Include(x=>x.Haberleri)
            .ThenInclude(x=>x.Resimler)
            .SingleOrDefaultAsync(x=>x.Id==id);

            

            ViewBag.Kategori= kategori;
            return View("index",kategori.Haberleri);

        }

        // GET: HaberIslemleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haber = await _context.Haberler.Include(x=>x.Resimler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haber == null)
            {
                return NotFound();
            }

            return View(haber);
        }

        [Authorize(Roles="Admin")]
        [Authorize(Roles="Editor")]
        // GET: HaberIslemleri/Create
        public async Task<IActionResult> Create(int? id)
        {
            var kategori = await _context.Kategoriler
            .Include(x=>x.Haberleri)
            .ThenInclude(x=>x.Resimler)
            .SingleOrDefaultAsync(x=>x.Id==id);

            

            ViewBag.Kategori= kategori;
            return View();
        }

        // POST: HaberIslemleri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

      public async Task<IActionResult> Create(int? id,[Bind("HaberBaslik,HaberBilgi,HaberTarih,IlgiDuzeyi,Dosyalar")] Haber haber)
        {
            if (ModelState.IsValid)
            {
                var dosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "resimler");

                if (!Directory.Exists(dosyaYolu))
                {
                    Directory.CreateDirectory(dosyaYolu);
                }

                foreach (var item in haber.Dosyalar)
                {
                    var tamDosyaAdi = Path.Combine(dosyaYolu, item.FileName);
                
                    using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi);
                    }

                    haber.Resimler.Add( new Resim{DosyaAdi=item.FileName} ); 
                }

                if(id!=null)haber.Kategorileri.Add(await _context.Kategoriler.FindAsync(id));
                
                _context.Add(haber);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["basarilimesaj"]=$"<{haber.HaberBaslik}> Ekleme İşlemi Başarılı.";
                    
                }
                catch (SystemException)
                {
                    if(id!=null)return RedirectToAction(nameof(KategorininHaberleri),new{id=id});
                    TempData["basarisizmesaj"]="Ekleme İşlemi Başarısız Oldu.";
                }
                
                if(id!=null) return RedirectToAction(nameof(KategorininHaberleri),new {id=id});

                return RedirectToAction(nameof(Index));
            }
            return View(haber);
        }

        [Authorize(Roles="Admin,Editor")]
        // GET: HaberIslemleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var haber = await _context.Haberler.FindAsync(id);
            var haber = await _context.Haberler.Include(x =>x.Resimler).SingleOrDefaultAsync(x =>x.Id == id);
            if (haber == null)
            {
                return NotFound();
            }
            return View(haber);
        }

        // POST: HaberIslemleri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HaberBaslik,HaberBilgi,HaberTarih,IlgiDuzeyi")] Haber haber)
        {
            if (id != haber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(haber);
                    TempData["basarilimesaj"]="Güncelleme İşlemi Başarılı.";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["basarisizmesaj"]="Güncelleme İşlemi Başarısız Oldu.";
                    if (!HaberExists(haber.Id))
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
            return View(haber);
        }

        [Authorize(Roles="Admin,Editor")]
        // GET: HaberIslemleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haber = await _context.Haberler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haber == null)
            {
                return NotFound();
            }

            return View(haber);
        }

        // POST: HaberIslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var haber = await _context.Haberler.FindAsync(id);
            _context.Haberler.Remove(haber);
            try
            {
                TempData["basarilimesaj"]="Silme İşlemi Başarılı.";
                await _context.SaveChangesAsync();
                foreach (var item in haber.Resimler)
                {
                    System.IO.File.Delete(Path.Combine(_dosyaYolu,item.DosyaAdi));
                }
            }
            catch (DbUpdateException)
            {
                TempData["basarisizmesaj"]="Silme İşlemi Başarısız Oldu.";
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles="Admin,Editor")]
        public async Task<IActionResult>ResimSil(int id)
        {
            var resim = await _context.Resimler.FindAsync(id);
            _context.Resimler.Remove(resim);
            await _context.SaveChangesAsync();
            System.IO.File.Delete(Path.Combine(_dosyaYolu,resim.DosyaAdi));
            return RedirectToAction(nameof(Edit), new {id = resim.HaberiId});
        }

        [Authorize(Roles="Admin,Editor")]
        [HttpPost]
        public async Task<IActionResult> ResimEkle(Haber haber)
        {
            var resimEklenecekHaber = await _context.Haberler.FindAsync(haber.Id);
            var dosyaYolu=Path.Combine(_hostEnvironment.WebRootPath,"resimler");
             if (!Directory.Exists(_dosyaYolu))
        {
            Directory.CreateDirectory(_dosyaYolu);
        }
            foreach (var item in haber.Dosyalar)
            {
                using (var dosyaAkisi=new FileStream(Path.Combine(dosyaYolu,item.FileName),FileMode.Create))
                {
                    await item.CopyToAsync(dosyaAkisi);
                }
                resimEklenecekHaber.Resimler.Add(new Resim {DosyaAdi=item.FileName});
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit),new{id=haber.Id});

        }


        public async Task<IActionResult> KategorileriniAyarla(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var haber=await _context.Haberler
            .Include(x=>x.Kategorileri)
            .SingleOrDefaultAsync(m=>m.Id==id);
            
            if(haber==null)
            {
                return NotFound();
            }
            return View(haber);
        }

        [Authorize(Roles="Admin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategorileriniAyarla(int? id,IFormCollection elemanlar)
        {
            var haber=await _context.Haberler
            .Include(x=>x.Kategorileri)
            .SingleOrDefaultAsync(m=>m.Id==id);

            haber.Kategorileri.Clear();

            foreach (var item in elemanlar["seciliKategoriler"])
            {
                haber.Kategorileri.Add(await _context.Kategoriler.FindAsync(int.Parse(item)));
            }

            await _context.SaveChangesAsync();
            TempData["basarilimesaj"] = $"<{haber.HaberBaslik}> Haberin Kategorileri Başarıyla Güncellendi.";
            return View(haber);
        }

        private bool HaberExists(int id)
        {
            return _context.Haberler.Any(e => e.Id == id);
        }
    }
}
