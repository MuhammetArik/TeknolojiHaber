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
using System.Diagnostics;

namespace TeknolojiHaber.Controllers
{
    public class HomeController : Controller
    {
        private readonly TeknolojiHaberContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private string _dosyaYolu;
        public HomeController(TeknolojiHaberContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _dosyaYolu=Path.Combine(_hostEnvironment.WebRootPath,"resimler");
        if (!Directory.Exists(_dosyaYolu))
        {
            Directory.CreateDirectory(_dosyaYolu);
        }
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Haberler.Include(x=>x.Resimler).ToListAsync());
        }


        public IActionResult HaberIslemleri()
        {
            return View();
        }
        private bool HaberExists(int id)
        {
            return _context.Haberler.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
