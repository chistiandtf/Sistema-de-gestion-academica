using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCursos.Models.vmodels
{
    public class InscripcionVM
    {

        public int IdEstudiante { get; set; }
        public string Codigo { get; set; }
        public Periodo Periodo { get; set; }
        public Curso Curso { get; set; }
    }
}
