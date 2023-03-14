using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryEmpDept repo;

        public PaginacionController(RepositoryEmpDept repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region PAGINAR GRUPO EMPLEADOS

        public async Task<IActionResult>
            PaginarGrupoEmpleados(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numeroregistros = this.repo.GetNumeroRegistrosVistaEmpleados();
            ViewData["REGISTROS"] = numeroregistros;
            List<Empleado> empleados =
                await this.repo.GetGrupoEmpleadosAsync(posicion.Value);
            return View(empleados);
        }

        #endregion

        #region PAGINAR GRUPO DEPARTAMENTOS
        public async Task<IActionResult>
            PaginarGrupoDepartamentos(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numeroregistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewData["REGISTROS"] = numeroregistros;
            List<Departamento> departamentos =
                await this.repo.GetGrupoDepartamentosAsync(posicion.Value);
            return View(departamentos);
        }

        public async Task<IActionResult>
            PaginarGrupoVistaDepartamento(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewData["REGISTROS"] = numRegistros;
            int numeroPagina = 1;
            //NECESITAMOS CREAR UN BUCLE QUE VAYA DE N A N
            //DEPENDIENDO DEL NUMERO DE REGISTROS A PAGINAR
            //LLEGAREMOS HASTA EL NUMERO DE REGISTROS
            string html = "<div>";
            for(int i = 1; i <= numRegistros; i += 2)
            {
                html +=
                    "<a href='PaginarGrupoVistaDepartamento?posicion="
                    + i + "'>Pagina " + numeroPagina + "</a> | ";
                numeroPagina += 1;
            }
            html += "</div>";
            ViewData["LINKS"] = html;
            List<VistaDepartamento> departamentos =
                await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }

        public async Task<IActionResult>
            PaginarRegistroVistaDepartamento(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            //ESTAMOS EN LA POSICION 1, QUE TENEMOS QUE DEVOLVER A LA VISTA?
            int siguiente = posicion.Value + 1;
            if(siguiente > numregistros)
            {
                //EFECTO OPTICO
                siguiente = numregistros;
            }
            int anterior = posicion.Value - 1;
            if(anterior < 1)
            {
                anterior = 1;
            }
            VistaDepartamento vistaDepartamento =
                await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(vistaDepartamento);
        }

        #endregion
    }
}
