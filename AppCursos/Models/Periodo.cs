using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppCursos.Models
{
    public partial  class Periodo
    {
        [Key]
       
        public int IdPeriodo { get; set; }
        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(2021,3000,ErrorMessage ="El rango de fecha esta desde 2021 hasta 3000")]
        public int? Anio { get; set; }
        public bool? Estado { get; set; }


        

        public virtual ICollection<Matricula> Matricula { get; set; }
    }
}
