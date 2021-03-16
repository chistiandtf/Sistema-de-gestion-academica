using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppCursos.Models
{
    public partial class Curso
    {
        

        [Key]
        public int IdCurso { get; set; }

        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(100)]
        public string Descripcion { get; set; }

        public bool? Estado { get; set; }

        public virtual ICollection<InscripcionCurso> InscripcionCurso { get; set; }

    }
}
