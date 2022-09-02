using Microsoft.AspNetCore.Identity;

namespace TeknolojiHaber.Models
{
    public class AppRole : IdentityRole
    {
        public string Kullanici_Aciklama { get; set; }
    }
}