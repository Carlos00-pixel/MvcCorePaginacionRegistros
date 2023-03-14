using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryEmpDept
    {
        private HospitalContext context;

        public RepositoryEmpDept(HospitalContext context)
        {
            this.context = context;
        }

        public List<Departamento> GetDepartamentos()
        {
            return this.context.Departamentos.ToList();
        }

        public List<Empleado> FindEmpleado(int idDepartamento)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdDepartamento == idDepartamento
                           select datos;
            return consulta.ToList();
        }
    }
}
