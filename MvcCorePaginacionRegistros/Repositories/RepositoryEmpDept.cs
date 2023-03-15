using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System.Data;

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



        CREATE PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO
        (@OFICIO NVARCHAR(50),
        @POSICION INT)
        AS
            SELECT * FROM 
                (SELECT CAST(
                    ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION,
                    EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
                FROM EMP 
                WHERE OFICIO = @OFICIO) AS QUERY
            WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 3)
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

        public async Task<List<Empleado>>
            GetEmpleadosOficioAsync(int posicion, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @OFICIO, @POSICION, @NUMEROREGISTROS OUT";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            SqlParameter pamoficio =
                new SqlParameter("@OFICIO", oficio);
            SqlParameter pamregistros =
                new SqlParameter("@NUMEROREGISTROS", -1);
            pamregistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamoficio, pamposicion, pamregistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)pamregistros.Value;
            return empleados;
        }

        public int GetNumeroEmpleadosOficio(string oficio)
        {
            return this.context.Empleados.Where(z => z.Oficio == oficio).Count();
        }

        #endregion
    }
}
