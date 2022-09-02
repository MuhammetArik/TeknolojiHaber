using System.Collections.Generic;

namespace TeknolojiHaber.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        
        public string KategoriAdi { get; set; }

        public string Aciklama { get; set; }

        public List<Haber> Haberleri {get; set;}
        public List<KategoriHaber> KategoriHaberler {get; set;}
    }
}