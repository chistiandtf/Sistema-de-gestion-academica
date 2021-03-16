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
    [Route("matriculas")]
    [ApiController]
    public class MatriculasController : ControllerBase
    {
        private readonly CursosCTX _context;

        public MatriculasController(CursosCTX context)
        {
            _context = context;
        }

        // GET: api/Matriculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatricula()
        {
            return await _context.Matricula.Include(x => x.Estudiante).Include(y => y.Periodo).ToListAsync();
        }

        // GET: api/Matriculas/5
        [HttpGet("buscar/{periodo}/{estudiante}")]
        public async Task<ActionResult<Matricula>> GetMatricula(int periodo, int estudiante)
        {
            var matricula = await _context.Matricula.Include(x => x.Estudiante).Include(x => x.Periodo).Where(x => x.IdEstudiante == estudiante && x.IdPeriodo == periodo).SingleOrDefaultAsync();

            if (matricula == null)
            {
                return NotFound(ErrorHelper.Response(404,"No se encuentra al Estudiante inscrito en el periodo"));
            }

            return Ok(matricula);
        }

        // PUT: api/Matriculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMatricula(int id, Matricula matricula)
        //{
        //    if (id != matricula.IdEstudiante)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(matricula).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MatriculaExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        
         [HttpPost("ingresarm/{periodo}/{estudiante}")]
        public async Task<IActionResult> Post(int periodo, int estudiante)
        {
            var Periodo = await _context.Periodo.AsNoTracking().Where(x => x.IdPeriodo == periodo).SingleOrDefaultAsync();
            if (Periodo == null)
            {
                return NotFound(ErrorHelper.Response(404, "Periodo no encontrado."));
            }

            if (!Periodo.Estado.Value)
            {
                return BadRequest(ErrorHelper.Response(400, "El periodo se encuentra cerrado."));
            }

            var Estudiante = await _context.Estudiante.AsNoTracking().Where(x => x.IdEstudiante == estudiante).SingleOrDefaultAsync();
            if (Estudiante == null)
            {
                return NotFound(ErrorHelper.Response(404, "Estudiante no encontrado."));
            }

            if (await _context.Matricula.Where(x => x.IdPeriodo == periodo && x.IdEstudiante == estudiante).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "El estudiante ya se encuentra matriculado en este periodo."));
            }

            _context.Matricula.Add(new Matricula()
            {
                IdPeriodo = periodo,
                IdEstudiante = estudiante,
                Fecha = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatricula), new { periodo = periodo, estudiante = estudiante }, null);

        }
        [HttpDelete("{periodo}/{estudiante}")]
        public async Task<IActionResult> Delete(int periodo, int estudiante)
        {
            var Matricula = await _context.Matricula.FindAsync(estudiante, periodo);
            if (Matricula == null)
            {
                return NotFound(ErrorHelper.Response(404, "El estudiante no se encuentra matriculado."));
            }

            if (!await _context.Periodo.Where(x => x.IdPeriodo == periodo && x.Estado == true).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "El periodo se encuentra cerrado, no puede eliminar la matrícula."));
            }

            if (await _context.InscripcionCurso.Where(x => x.IdPeriodo == periodo && x.IdEstudiante == estudiante).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "No se puede eliminar la matrícula porque el estudiante tiene cursos inscritos."));
            }

            _context.Matricula.Remove(Matricula);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        //private bool MatriculaExists(int id)
        //{
        //    return _context.Matricula.Any(e => e.IdEstudiante == id);
        //}
    }
}
