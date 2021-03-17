using System.Threading.Tasks;
using AppCursos.Models;
using AppCursos.Models.vmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using AppCursos.Helper;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


namespace AppCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {

        private readonly CursosCTX ctx;
        private readonly IConfiguration config;

        public LoginController(CursosCTX _ctx, IConfiguration _config)
        {
            ctx = _ctx;
            config = _config;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(LoginVM Login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            Usuarios Usuario = await ctx.Usuarios.Where(x => x.Usuario == Login.Usuario).FirstOrDefaultAsync();
            if (Usuario == null)
            {
                return NotFound(ErrorHelper.Response(404, "Usuario no encontrado."));
            }

            if (HasHelper.CheckHash(Login.Clave, Usuario.Clave, Usuario.Sal))
            {
                object secretKey = config.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes((string)secretKey);

                ClaimsIdentity claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Login.Usuario));

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken createdToken = tokenHandler.CreateToken(tokenDescriptor);

                string bearer_token = tokenHandler.WriteToken(createdToken);
                return Ok(bearer_token);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
             var r = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            return Ok(r == null ? "" : r.Value);
        }

    }
}
