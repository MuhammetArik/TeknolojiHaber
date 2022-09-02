using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeknolojiHaber.Models;

namespace TeknolojiHaber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        //DI: Bağımşı Enjeksiyon
        public LoginController(SignInManager<AppUser> signInManager, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        // ...api/Login
        [HttpPost]
        public async Task<IActionResult> Post(KullaniciViewModel kullaniciViewModel)
        {
            var kullanici = await _userManager.FindByEmailAsync(kullaniciViewModel.Eposta);
            if(kullanici==null) return BadRequest(); //Diğer Kısımların İşlenmesine Gerek Yok

            var giris = await _signInManager.PasswordSignInAsync(kullanici, kullaniciViewModel.Sifre, false, false);
            if(giris.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                         new Claim(ClaimTypes.Name,"muhammet"),
                        //new Claim(ClaimTypes.Role,"Admin"),
                         //new Claim(ClaimTypes.Role,(await _userManager.GetRolesAsync(kullanici))[0]),
                    }),
                    //Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                foreach (var item in await _userManager.GetRolesAsync(kullanici))
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, item));
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);
                kullaniciViewModel.Token=tokenHandler.WriteToken(token);

                return Ok(kullaniciViewModel) ;
            }
            else
            {
                return BadRequest();  //400
            }
        }
    }
}