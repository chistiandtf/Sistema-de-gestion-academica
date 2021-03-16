
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCursos.Helper;
using AppCursos.Models;
using AppCursos.Models.vmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace AppCursos.Controllers
{

    [ApiController]
    [Route("usuarios")]
    public class UsuariosController:ControllerBase
    {


         private readonly CursosCTX _context;


         public UsuariosController(CursosCTX d ){

             _context = d ;

         }


        [HttpGet]

        public async Task<IActionResult> Get()
        {
            List<UsuarioVM> Usuarios = await _context.Usuarios.Select(x => new UsuarioVM()
            {
                IdUsuario = x.IdUsuario,
                Usuario = x.Usuario
            }).ToListAsync();
            return Ok(Usuarios);
        }

        [HttpPost]

        public async Task<IActionResult> Post(Usuarios Usuario)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if(await _context.Usuarios.Where(x=>x.Usuario == Usuario.Usuario).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, $"El usuario {Usuario.Usuario} ya existe."));
            }

            HashedPassword Password = HasHelper.Hash(Usuario.Clave);
            Usuario.Clave = Password.Password;
            Usuario.Sal = Password.Salt;
            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new {id=Usuario.IdUsuario}, new UsuarioVM(){
                IdUsuario = Usuario.IdUsuario,
                Usuario = Usuario.Usuario
            });
        }
    }
}