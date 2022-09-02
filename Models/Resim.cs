using System.ComponentModel.DataAnnotations;

namespace TeknolojiHaber.Models
{
    public class Resim
    {


        public int Id { get; set; } 
        public string DosyaAdi { get; set; }

        public int HaberiId { get; set; }
        
        [Required]
        public Haber Haberi { get; set; }  // HaberiId

    }
}