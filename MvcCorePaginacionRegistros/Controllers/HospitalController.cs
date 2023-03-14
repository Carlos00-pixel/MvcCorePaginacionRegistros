using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class HospitalController : Controller
    {
        private RepositoryEmpDept repo;

        public HospitalController(RepositoryEmpDept repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int iddepartamento)
        {
            List<Empleado> emp = this.repo.FindEmpleado(iddepartamento);
            return View(emp);
        }
    }
}
