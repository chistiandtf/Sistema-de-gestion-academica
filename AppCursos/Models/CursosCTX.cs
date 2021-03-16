using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppCursos.Models;

namespace AppCursos.Models
{
    public class CursosCTX:DbContext
    {
        public CursosCTX()
        {

        }
        public CursosCTX(DbContextOptions<CursosCTX> options) : base(options)
        {

        }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Estudiante> Estudiante { get; set; }
        public virtual DbSet<InscripcionCurso> InscripcionCurso { get; set; }
        public virtual DbSet<Matricula> Matricula { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<Usuarios> Usuarios {get; set;}



        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<InscripcionCurso>(entity =>
            {
                entity.HasKey(e => new { e.IdEstudiante, e.IdPeriodo, e.IdCurso })
                    .HasName("PK__Inscripc__994C4A9CC8A4D1D5");

                
            });

            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(e => e.IdCurso)
                    .HasName("PK__Curso__085F27D621F831CB");

                
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasKey(e => e.IdEstudiante)
                    .HasName("PK__Estudian__B5007C246E883F92");

               
            });

            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.HasKey(e => new { e.IdEstudiante, e.IdPeriodo })
                    .HasName("PK__Matricul__4E4415BB59276EF9");

               
            });

            modelBuilder.Entity<Periodo>(entity =>
            {
                entity.HasKey(e => e.IdPeriodo)
                    .HasName("PK__Periodo__B44699FEC5F12AB8");

                
            });

        }
    }
}
