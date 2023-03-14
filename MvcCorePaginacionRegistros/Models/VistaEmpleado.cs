using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCorePaginacionRegistros.Models
{
    [Table("V_GRUPO_EMPLEADOS")]
    public class VistaEmpleado
    {
        [Key]
        [Column("EMP_NO")]
        public int IdEmpleado { get; set; }

        [Column("APELLIDO")]
        public string Apellido { get; set; }

        [Column("OFICIO")]
        public string Oficio { get; set; }

        [Column("DIR")]
        public int Direccion { get; set; }

        [Column("FECHA_ALT")]
        public DateTime FechaAlta { get; set; }

        [Column("SALARIO")]
        public int Salario { get; set; }

        [Column("COMISION")]
        public int Comision { get; set; }

        [Column("DEPT_NO")]
        public int IdDepartamento { get; set; }

        [Column("POSICION")]
        public int Posicion { get; set; }
    }
}
