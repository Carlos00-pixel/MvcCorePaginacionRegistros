using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{

    #region SQL
    /*
        CREATE VIEW V_DEPARTAMENTOS_INDIVIDUAL
        AS
	        SELECT CAST(
	        ROW_NUMBER() OVER (ORDER BY DEPT_NO) AS INT) AS POSICION,
	        ISNULL(DEPT_NO, 0) AS DEPT_NO, DNOMBRE, LOC 
	        FROM DEPT
        GO 
    */
    #endregion

    public class RepositoryEmpDept
    {
        private HospitalContext context;

        public RepositoryEmpDept(HospitalContext context)
        {
            this.context = context;
        }

        #region VISTA DEPARTAMENTOS

        public async Task<List<VistaDepartamento>>
            GetGrupoVistaDepartamentoAsync(int posicion)
        {
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        }

        public int GetNumeroRegistrosVistaDepartamentos()
        {
            return this.context.VistaDepartamentos.Count();
        }

        public async Task<VistaDepartamento> 
            GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await
                this.context.VistaDepartamentos.FirstOrDefaultAsync(x => x.Posicion == posicion);
            return vista;
        }
        #endregion

        #region DEPARTAMENTOS

        public async Task<List<Departamento>>
            GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @POSICION";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            var consulta =
                this.context.Departamentos.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

        public List<Departamento> GetDepartamentos()
        {
            return this.context.Departamentos.ToList();
        }

        #endregion

        #region VISTA EMPLEADOS
        public async Task<List<VistaEmpleado>>
            GetGrupoVistaEmpleadoAsync(int posicion)
        {
            var consulta = from datos in this.context.VistaEmpleados
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        }

        public int GetNumeroRegistrosVistaEmpleados()
        {
            return this.context.VistaEmpleados.Count();
        }
        #endregion

        #region EMPLEADOS

        public async Task<List<Empleado>>
            GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @POSICION";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

        public List<Empleado> FindEmpleado(int idDepartamento)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdDepartamento == idDepartamento
                           select datos;
            return consulta.ToList();
        }

        #endregion
    }
}
