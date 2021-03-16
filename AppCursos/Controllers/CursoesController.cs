using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCursos.Models;
using AppCursos.Helper;

namespace AppCursos.Controllers
{
    [Route("Cursos")]
    [ApiController]
    public class CursoesController : ControllerBase
    {
        private readonly CursosCTX _context;

        public CursoesController(CursosCTX context)
        {
            _context = context;
        }

        // GET: api/Cursoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCurso()
        {
            return await _context.Curso.ToListAsync();
        }

        // GET: api/Cursoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Curso.FindAsync(id);

            if (curso == null)
            {
                return NotFound(ErrorHelper.Response(404,"El curso  ha sido encontrado"));
            }

            return Ok(curso);
        }
        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string b,[FromQuery] bool? estado)
        {
            // constain es like en sql
            //Console.WriteLine(b);

            if (!string.IsNullOrWhiteSpace(b))
            {
                return Ok(await _context.Curso.Where(x => (x.Descripcion.Contains(b) || x.Codigo.Contains(b)) && (x.Estado == (estado == null ? x.Estado : estado.Value))).ToListAsync());


            }
            else { 
            
            
            }
            // x.estado == (estado == null) if estado es igual null entonces ? muestrame el estado por defecto si no : muestrame el esttado dado
            return Ok(await _context.Curso.Where(x =>x.Estado==(estado==null ? x.Estado:estado.Value)).ToListAsync());
            

        }


        // PUT: api/Cursoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.IdCurso)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                if (await _context.Curso.Where(x => x.Codigo == curso.Codigo).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, $"El código {curso.Codigo} ya existe."));
                }
                else {

                    _context.Entry(curso).State = EntityState.Modified;
                    // validacion de modelo
                    if (!TryValidateModel(curso, nameof(Curso)))
                    {
                        return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
                    }
                    await _context.SaveChangesAsync();
                }


                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound(ErrorHelper.Response(404,"Curso no encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cursoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));

            }

            else {
                if (await _context.Curso.Where(x => x.IdCurso == curso.IdCurso).AnyAsync())
                {

                    return BadRequest(ErrorHelper.Response(400, $"El código {curso.IdCurso} ya existe."));
                }

                if (await _context.Curso.Where(x => x.Codigo == curso.Codigo).AnyAsync()) {

                     return BadRequest(ErrorHelper.Response(400, $"El código {curso.Codigo} ya existe."));
                }
                curso.IdCurso = 0;
                curso.Estado = curso.Estado ?? true; // esto es por defecto
                _context.Curso.Add(curso);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCurso", new { id = curso.IdCurso }, curso);

            }
            
        }

        // DELETE: api/Cursoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Curso.Include(x => x.InscripcionCurso).Where(x => x.IdCurso == id).SingleOrDefaultAsync();
            if (curso == null)
            {
                return NotFound(ErrorHelper.Response(404,"El Curso no existe"));
            }
            if (curso.InscripcionCurso.Count > 0) {

                return BadRequest(ErrorHelper.Response(400, "No se puede eliminar un curso que ya esta en uso"));
            }

            _context.Curso.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoExists(int id)
        {
            return _context.Curso.Any(e => e.IdCurso == id);
        }
    }
}
