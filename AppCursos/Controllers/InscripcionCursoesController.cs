using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCursos.Models;
using AppCursos.Helper;
using AppCursos.Models.vmodels;

namespace AppCursos.Controllers
{
    [Route("inscribir")]
    [ApiController]
    public class InscripcionCursoesController : ControllerBase
    {
        private readonly CursosCTX _context;

        public InscripcionCursoesController(CursosCTX context)
        {
            _context = context;
        }

        // GET: api/InscripcionCursoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionVM>>> GetInscripcionCurso()
        {
            return await _context.InscripcionCurso
                    .Include(x => x.Curso).Include(x => x.Matricula).Include(x => x.Periodo)
                    .Select(x => new InscripcionVM()
                    {
                        IdEstudiante = x.IdEstudiante,
                        Codigo = x.Estudiante.Codigo,
                        Curso = x.Curso,
                        Periodo = x.Periodo
                    })
                    .ToListAsync();
        }

        
        [HttpGet("{periodo}/{estudiante}")]
        public async Task<IActionResult> CursosEstudiante(int periodo, int estudiante)
        {
            var Inscripciones = await _context.InscripcionCurso
            .Where(x => x.IdPeriodo == periodo && x.IdEstudiante == estudiante)
            .Select(x => new CursoVM() 
            {
                Codigo = x.Curso.Codigo,
                Nombre = x.Curso.Descripcion,
                FechaInscripcion = x.Fecha.Value
            }).ToListAsync();
            return Ok(Inscripciones);
        }

         [HttpGet("{periodo}/{estudiante}/{curso}")]
        public async Task<IActionResult> Get(int periodo, int estudiante, string curso)
        {
            var Inscripcion = await _context.InscripcionCurso
            .Include(x=>x.Curso).Include(x=>x.Matricula).Include(x=>x.Periodo)
            .Where(x=>x.IdEstudiante == estudiante && x.Curso.Codigo == curso && x.IdPeriodo == periodo)
            .Select(x=>
            new InscripcionVM(){
                IdEstudiante = x.IdEstudiante,
                Codigo = x.Estudiante.Codigo,
                Curso = x.Curso,
                Periodo = x.Periodo
            })
            .SingleOrDefaultAsync();

            if(Inscripcion == null)
            {
                return NotFound(ErrorHelper.Response(404, $"El curso {curso} no se encuentra inscrito."));
            }

            return Ok(Inscripcion);
        }


        [HttpPost("{periodo}/{estudiante}/{curso}")]
        public async Task<IActionResult> Post(int periodo, int estudiante, string curso)
        {
            if (!await _context.Periodo.Where(x => x.IdPeriodo == periodo).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, $"El periodo {periodo} se encuentra cerrado o no existe."));
            }

            var Estudiante = await _context.Estudiante.Where(x => x.IdEstudiante == estudiante).AsNoTracking().SingleOrDefaultAsync();
            if (Estudiante == null)
            {
                return NotFound(ErrorHelper.Response(404, $"El estudiante {estudiante} no existe."));
            }

            var Curso = await _context.Curso.Where(x => x.Codigo == curso).AsNoTracking().SingleOrDefaultAsync();
            if (Curso == null)
            {
                return NotFound(ErrorHelper.Response(404, $@"El curso {curso} no existe."));
            }

            if (!await _context.Matricula.Where(x => x.IdEstudiante == estudiante && x.IdPeriodo == periodo).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, $"El estudiante {estudiante} no se encuentra matriculado en el periodo {periodo}."));
            }

            if (await _context.InscripcionCurso.Where(x => x.IdEstudiante == estudiante && x.IdCurso == Curso.IdCurso && x.IdPeriodo == periodo).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, $"El curso {curso} ya se encuentra inscrito."));
            }

            _context.InscripcionCurso.Add(new InscripcionCurso()
            {
                IdEstudiante = estudiante,
                IdCurso = Curso.IdCurso,
                IdPeriodo = periodo,
                Fecha = DateTime.Now
            });
            await _context.SaveChangesAsync();

            var Inscripcion = new InscripcionVM()
            {
                IdEstudiante = estudiante,
                Codigo = Estudiante.Codigo,
                Curso = Curso,
                Periodo = await _context.Periodo.Where(x => x.IdPeriodo == periodo).AsNoTracking().SingleOrDefaultAsync()
            };
            return CreatedAtAction(nameof(Get), new { periodo = periodo, curso = curso, estudiante = estudiante }, Inscripcion);
        }

        //        // PUT: api/InscripcionCursoes/5
        //        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //        [HttpPut("{id}")]
        //        public async Task<IActionResult> PutInscripcionCurso(int id, InscripcionCurso inscripcionCurso)
        //        {
        //            if (id != inscripcionCurso.IdEstudiante)
        //            {
        //                return BadRequest();
        //            }

        //            _context.Entry(inscripcionCurso).State = EntityState.Modified;

        //            try
        //            {
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!InscripcionCursoExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }

        //            return NoContent();
        //        }

        //        // POST: api/InscripcionCursoes
        //        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //        [HttpPost]
        //        public async Task<ActionResult<InscripcionCurso>> PostInscripcionCurso(InscripcionCurso inscripcionCurso)
        //        {
        //            _context.InscripcionCurso.Add(inscripcionCurso);
        //            try
        //            {
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateException)
        //            {
        //                if (InscripcionCursoExists(inscripcionCurso.IdEstudiante))
        //                {
        //                    return Conflict();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }

        //            return CreatedAtAction("GetInscripcionCurso", new { id = inscripcionCurso.IdEstudiante }, inscripcionCurso);
        //        }

        //        
        [HttpDelete("{periodo}/{estudiante}/{curso}")]
        public async Task<IActionResult> Delete(int periodo, int estudiante, string curso)
        {
            if (!await _context.Periodo.Where(x => x.IdPeriodo == periodo && x.Estado == true).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "El periodo se encuentra cerrado o no existe."));
            }

            int IdCurso = await _context.Curso.Where(x => x.Codigo == curso).AsNoTracking().Select(x => x.IdCurso).FirstOrDefaultAsync();

            var Inscripcion = await _context.InscripcionCurso.FindAsync(estudiante, periodo, IdCurso);
            if (Inscripcion == null)
            {
                return NotFound(ErrorHelper.Response(404, "El curso no se encuentra inscrito."));
            }

            _context.InscripcionCurso.Remove(Inscripcion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool InscripcionCursoExists(int id)
        {
            return _context.InscripcionCurso.Any(e => e.IdEstudiante == id);
        }
    }
}
