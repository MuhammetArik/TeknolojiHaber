using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TeknolojiHaber.Models
{
    public class AppUser : IdentityUser
    {
        public string Kullanici_Aciklama { get; set; }
    }
}