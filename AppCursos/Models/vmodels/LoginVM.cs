using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppCursos.Models.vmodels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }


    }
}
