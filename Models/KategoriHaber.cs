using System;

namespace TeknolojiHaber.Models
{
    public class KategoriHaber
    {
        public int HaberId { get; set; }

        public Haber Haber { get; set; }

        public int KategoriId { get; set; }

        public Kategori Kategori { get; set; }
        
        public DateTime EklenmeTarihi { get; set; }
    }
}