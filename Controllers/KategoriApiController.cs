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
    [Authorize(Roles="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriApiController : ControllerBase
    {
        private readonly TeknolojiHaberContext _context;

        public KategoriApiController(TeknolojiHaberContext context)
        {
            _context = context;
        }

        // GET: api/KategoriApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kategori>>> GetKategoriler()
        {
            return await _context.Kategoriler.ToListAsync();
        }

        // GET: api/KategoriApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kategori>> GetKategori(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);

            if (kategori == null)
            {
                return NotFound();
            }

            return kategori;
        }

        // PUT: api/KategoriApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKategori(int id, Kategori kategori)
        {
            if (id != kategori.Id)
            {
                return BadRequest();
            }

            _context.Entry(kategori).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategoriExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/KategoriApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kategori>> PostKategori(Kategori kategori)
        {
            _context.Kategoriler.Add(kategori);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKategori", new { id = kategori.Id }, kategori);
        }

        // DELETE: api/KategoriApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKategori(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            _context.Kategoriler.Remove(kategori);
            await _context.SaveChangesAsync();
        
            return NoContent();
        }

        private bool KategoriExists(int id)
        {
            return _context.Kategoriler.Any(e => e.Id == id);
        }
    }
}
