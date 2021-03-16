using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AppCursos.Models
{
    public class Estudiante
    {

        [Key]
        public int IdEstudiante { get; set; }

        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellido { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        // esto se usa para crear un campo que va definido en el sql automaticamente
        public string NombreApellido { get; set; }


        [Column(TypeName = "date")]
        public DateTime? FechaNacimiento { get; set; }

        

        public virtual ICollection<Matricula> Matricula { get; set; }
    }
}
