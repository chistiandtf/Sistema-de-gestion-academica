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
    [Route("periodos")]
    [ApiController]
    public class PeriodoesController : ControllerBase
    {
        private readonly CursosCTX _context;

        public PeriodoesController(CursosCTX context)
        {
            _context = context;
        }

        // GET: api/Periodoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Periodo>>> GetPeriodo()
        {
            return await _context.Periodo.ToListAsync();
        }

        // GET: api/Periodoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Periodo>> GetPeriodo(int id)
        {
            var periodo = await _context.Periodo.FindAsync(id);

            if (periodo == null)
            {
                return NotFound(ErrorHelper.Response(404,"El documento no ha sido encontrado"));
            }

            return Ok(periodo);
        }

       

        // PUT: api/Periodoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeriodo(int id, Periodo periodo)
        {
            if (id != periodo.IdPeriodo)
            {
                return BadRequest();
            }

            _context.Entry(periodo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodoExists(id))
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

        // POST: api/Periodoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        [HttpGet("activo")]
        public async Task<IActionResult> GetActivo() {

            var periodo = await _context.Periodo.Where(x => x.Estado == true).OrderByDescending(x => x.Anio).FirstOrDefaultAsync();
            if (periodo == null) {

                return BadRequest(ErrorHelper.Response(404, "No se encuentan periodos activos"));
            }

            return Ok(periodo);
        }


        [HttpPatch("activar/{id}")]
        public async Task<IActionResult> Activar(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Periodo = await _context.Periodo.FindAsync(id);
                    if (Periodo == null)
                    {
                        return NotFound(ErrorHelper.Response(404, $"No existe el periodo {id}."));
                    }

                    if (Periodo.Estado.Value)
                    {
                        await transaction.RollbackAsync();
                        return NoContent();
                    }
                    else
                    {
                        //var Periodos = await _context.Periodo.Where(x => x.IdPeriodo != id).ToListAsync();
                        foreach ( var periodo in await _context.Periodo.Where(x => x.IdPeriodo != id).ToListAsync()) {

                            periodo.Estado = false;
                        
                        }
                        Periodo.Estado = true;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return Ok();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ErrorHelper.Response(500, "Ha ocurrido un error en la base de datos."));
                }
            }
        }


        
        [HttpPatch("Desactivar/{id}")]


        public async Task<IActionResult> Desactivar(int id) {

            var periodo = await _context.Periodo.FindAsync(id);

            if (periodo == null) {

                return NotFound(ErrorHelper.Response(404, "El elemento no ha sido encotrado"));
            }

            if (!periodo.Estado.Value) { 

                return NoContent();
            
            }

            periodo.Estado = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
             




        [HttpPost]
        public async Task<ActionResult<Periodo>> PostPeriodo(Periodo periodo)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));

            }
            else
            {
                if (await _context.Periodo.Where(x => x.Anio ==periodo.Anio).AnyAsync())
                {

                    return BadRequest(ErrorHelper.Response(400, $"El anio {periodo.Anio} ya existe."));
                }
                _context.Periodo.Add(periodo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPeriodo", new { id = periodo.IdPeriodo }, periodo);
            }
        }

        // DELETE: api/Periodoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeriodo(int id)
        {
            var periodo = await _context.Periodo.FindAsync(id);
            if (periodo == null)
            {
                return NotFound();
            }

            _context.Periodo.Remove(periodo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeriodoExists(int id)
        {
            return _context.Periodo.Any(e => e.IdPeriodo == id);
        }
    }
}
