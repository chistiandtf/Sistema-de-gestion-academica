using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCursos.Models;
using AppCursos.Helper;
using Microsoft.AspNetCore;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;



namespace AppCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly CursosCTX _context;

        Estudiante estudent = new Estudiante();

        public EstudiantesController(CursosCTX context)
        {
            _context = context;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiante()
        {
            return await _context.Estudiante.ToListAsync();
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiante.FindAsync(id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return estudiante;
        }

        // PUT: api/Estudiantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.IdEstudiante)
            {
                return BadRequest();
            }

            

            try
            {
                if (await _context.Estudiante.Where(x => x.Codigo == estudiante.Codigo).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, $"El código {estudiante.Codigo} ya existe."));
                }
                else {
                    _context.Entry(estudiante).State = EntityState.Modified;
                    // aqui hago las validaciones del modelo
                    if (!TryValidateModel(estudiante, nameof(Estudiante)))
                    {
                        return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
                    }
                    await _context.SaveChangesAsync();
                }

               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudianteExists(id))
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

        // POST: api/Estudiantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            //aqui valido
            if (!ModelState.IsValid)
            {

                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            else {

                if (await _context.Estudiante.Where(x => x.Codigo == estudiante.Codigo).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, $"El código {estudiante.Codigo} ya existe."));
                }

                estudiante.IdEstudiante = 0; // esto para que el id siempre se convierta en 0
            // asi si ponen un id en el json nod a error
            _context.Estudiante.Add(estudiante);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEstudiante", new { id = estudiante.IdEstudiante }, estudiante);
            }
            

           
        }


        // patch metodo

        [HttpPatch("CambiarCodigo/{id}")]
        public async Task<IActionResult> CambiarCodigo(int id, [FromQuery] string codigo)
        {

            if (string.IsNullOrWhiteSpace(codigo))
            {
                return BadRequest(ErrorHelper.Response(400, "El código está vacío."));
            }

            var E = await _context.Estudiante.FindAsync(id);
            if (E == null)
            {
                return NotFound();
            }

            if (await _context.Estudiante.Where(x => x.Codigo == codigo).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "El código  ya existe."));
            }
            else {
            E.Codigo = codigo;
                if (!TryValidateModel(E, nameof(Estudiante)))
                {
                    return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
                }
                await _context.SaveChangesAsync();
            return StatusCode(200, E);
            }

            
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _context.Estudiante.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            _context.Estudiante.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("Actulizar/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<Estudiante> _Estudiante)
        {
            var Estudiante = await _context.Estudiante.FindAsync(id);
            if (Estudiante == null)
            {
                return NotFound();
            }

            _Estudiante.ApplyTo(Estudiante, ModelState);
            if (!TryValidateModel(Estudiante, "Estudiante"))
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }
            await _context.SaveChangesAsync();
            return Ok(Estudiante);
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiante.Any(e => e.IdEstudiante == id);
        }
    }
}
